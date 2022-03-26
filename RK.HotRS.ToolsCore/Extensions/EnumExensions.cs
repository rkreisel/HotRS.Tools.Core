using RK.HotRS.ToolsCore.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RK.HotRS.ToolsCore.Extensions
{
	/// <summary>
	/// Provides exensions and methods for Enums
	/// </summary>
	public static class EnumExtensions
	{
		/// <summary>
		/// Returns the value of the Description attribute for the given enum value.
		/// </summary>
		/// <param name="useDisplayIfNoDesc">Returns the value of the Display attribute. Throws an exception if not present.</param>
		/// <param name="useDefaultIfNoDescOrDisplay">Returns the default value if the Display attribute is not present.</param>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		[ExcludeFromCodeCoverage]
		public static string GetEnumDescription<T>(this T value, bool useDisplayIfNoDesc = true, bool useDefaultIfNoDescOrDisplay = true) where T : struct
		{
			var valueType = value.GetType();

			if (!valueType.IsEnum) { throw new ArgumentException(Resources.TYPENOTINASSEMBLY, nameof(value)); }

			var valueField = valueType.GetField(Enum.GetName(valueType, value));

			if (valueField != null)
			{
				var descriptions = valueField.GetCustomAttributes(typeof(DescriptionAttribute), true);
				if (descriptions != null && descriptions.Length > 0)
				{
					return ((DescriptionAttribute)descriptions[0]).Description;
				}
				if (useDisplayIfNoDesc)
				{
					var displays = valueField.GetCustomAttributes(typeof(DisplayAttribute), true);
					if (displays != null && displays.Length > 0)
					{
						return ((DisplayAttribute)displays[0]).Name;
					}
				}
				if (useDefaultIfNoDescOrDisplay)
				{
					return value.ToString();
				}
			}
			return string.Empty;
		}

		/// <summary>
		/// Returns the DataType specified in the [DataType} attribute, or DataType.Text if none was specified.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		[ExcludeFromCodeCoverage]
		public static DataType GetDataType<T>(this T value) where T : struct
		{
			var valueType = value.GetType();

			if (!valueType.IsEnum) { throw new ArgumentException(Resources.TYPENOTINASSEMBLY, nameof(value)); }

			var valueField = valueType.GetField(Enum.GetName(valueType, value));

			if (valueField != null)
			{
				var dataTypes = valueField.GetCustomAttributes(typeof(DataTypeAttribute), true);
				if (dataTypes != null && dataTypes.Length > 0)
				{
					return ((DataTypeAttribute)dataTypes[0]).DataType;
				}
			}
			return DataType.Text;
		}

		/// <summary>
		/// Returns the Enum value based on the value of the description attribute.
		/// Throws an exception if the search value is not found on the Description attribute of any member of the enum.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <param name="description">The value to be used in the search.</param>
		[ExcludeFromCodeCoverage]
		public static T? GetValueFromDescription<T>(this T value, string description) where T : struct
		{
			//description.CheckParameterForNull("description");

			var valueType = typeof(T);

			if (!valueType.IsEnum) { throw new ArgumentException(Resources.TYPENOTINASSEMBLY, nameof(value)); }

			T? result = null;

			foreach (var enumValue in Enum.GetValues(valueType))
			{
				if (enumValue.ToString() != value.ToString())
					continue;

				var valueField = valueType.GetField(Enum.GetName(valueType, enumValue));

				var descriptions = valueField.GetCustomAttributes(typeof(DescriptionAttribute), true);

				if (descriptions != null && descriptions.Length > 0 && ((DescriptionAttribute)descriptions[0]).Description == description)
				{
					result = (T)enumValue;
					break;
				}
			}

			return result;
		}


        /// <summary>
        /// Provides additional methods for Enums
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public partial class Enum<T> where T : struct, IConvertible
        {
			/// <summary>
			/// Generates a generic List of strings from the values in an Enum
			/// 
			/// Usage:
			/// new Enum<typeparamref name="T"/>().AsList(true);
			/// </summary>
			/// <param name="useDescriptionIfAvailable">Defaults to False. If set to true this method will return the value of 
			/// any [Description] attribute (or the base enum value if there is no [Desciption] attribute).</param>
			/// <returns>A generic List of strings.</returns>
			[ExcludeFromCodeCoverage]
			public IList<string> AsList(bool useDescriptionIfAvailable = false)
            {
                var valueType = typeof(T);
                if (!valueType.IsEnum)
                    throw new ArgumentException(Resources.OBJNOTPROVIDEDTOENUM);

                var result = new List<string>();

                foreach (var enumValue in Enum.GetValues(valueType))
                {
                    var valueField = valueType.GetField(Enum.GetName(valueType, enumValue));
                    var descriptions = valueField.GetCustomAttributes(typeof(DescriptionAttribute), true);
                    if (useDescriptionIfAvailable)
                    {
                        if (descriptions != null && descriptions.Length > 0)
                        {
                            result.Add(((DescriptionAttribute)descriptions[0]).Description);
                        }
                        else
                        {
                            result.Add(enumValue.ToString());
                        }
                    }
                    else
                    {
                        result.Add(enumValue.ToString());
                    }
                }
                return result;
            }
        }
    }
}
