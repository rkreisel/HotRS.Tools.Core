using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace RK.HotRS.ToolsCore.Utilities.JSON
{
	/// <summary>
	/// Dynamically rename or ignore properties when serializing to JSON
	/// Usage:
	///     var jsonResolver = new PropertyIgnoreSerializerContractResolver();
	///	    jsonResolver.IgnoreProperty(typeof(Person), "Title");
	///	    jsonResolver.RenameProperty(typeof(Person), "FirstName", "firstName");
	///	
	///	    var serializerSettings = new JsonSerializerSettings();
	///	    serializerSettings.ContractResolver = jsonResolver;
	///	
	///	    var json = JsonConvert.SerializeObject(person, serializerSettings);
	/// </summary>
	public class PropertyRenameOrIgnoreSerializerContractResolver : DefaultContractResolver
	{
		private readonly Dictionary<Type, HashSet<string>> ignores;
		private readonly Dictionary<Type, Dictionary<string, string>> renames;

		/// <summary>
		/// Constructor
		/// </summary>
		public PropertyRenameOrIgnoreSerializerContractResolver()
		{
			ignores = new Dictionary<Type, HashSet<string>>();
			renames = new Dictionary<Type, Dictionary<string, string>>();
			this.IgnoreSerializableInterface = true;
		}

		/// <summary>
		/// Ignores a property
		/// </summary>
		/// <param name="type"></param>
		/// <param name="jsonPropertyNames"></param>
			public void IgnoreProperty(Type type, params string[] jsonPropertyNames)
		{
			if (!this.ignores.ContainsKey(type))
				ignores[type] = new HashSet<string>();

			foreach (var prop in jsonPropertyNames)
				ignores[type].Add(prop);
		}

		/// <summary>
		/// Renames a property
		/// </summary>
		/// <param name="type"></param>
		/// <param name="propertyName"></param>
		/// <param name="newJsonPropertyName"></param>
		[ExcludeFromCodeCoverage]
		public void RenameProperty(Type type, string propertyName, string newJsonPropertyName)
		{
			if (!renames.ContainsKey(type))
				renames[type] = new Dictionary<string, string>();

			renames[type][propertyName] = newJsonPropertyName;
		}

		/// <summary>
		/// Overrides the CreateProperty method
		/// </summary>
		/// <param name="member"></param>
		/// <param name="memberSerialization"></param>
		/// <returns></returns>
		protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
		{
			var property = base.CreateProperty(member, memberSerialization);

			if (IsIgnored(property.DeclaringType, property.PropertyName))
				property.ShouldSerialize = i => false;

			return property;
		}

		private bool IsIgnored(Type type, string jsonPropertyName)
		{
			if (!ignores.ContainsKey(type))
				return false;

			return ignores[type].Contains(jsonPropertyName);
		}

		//private bool IsRenamed(Type type, string jsonPropertyName, out string newJsonPropertyName)
		//{
		//	Dictionary<string, string> renames;

		//	if (!this.renames.TryGetValue(type, out renames) || !renames.TryGetValue(jsonPropertyName, out newJsonPropertyName))
		//	{
		//		newJsonPropertyName = null;
		//		return false;
		//	}

		//	return true;
		//}
	}
}