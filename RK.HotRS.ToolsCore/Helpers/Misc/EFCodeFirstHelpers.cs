using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using RK.HotRS.ToolsCore.Exceptions;
using RK.HotRS.ToolsCore.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace RK.HotRS.ToolsCore.Helpers.Misc
{
	/// <summary>
	/// EF Core Code First Helper methods
	/// </summary>
	[ExcludeFromCodeCoverage]
	public static class EFCodeFirstHelpers
	{
		/// <summary>
		/// Locates a sql file that is embedded in the assembly and executes it.
		/// </summary>
		/// <param name="containingAssembly">The assembly in which the sql can be found.</param>
		/// <param name="context">A DbContext</param>
		/// <param name="resourceName">The name of the resource. This will follow he pattern
		/// assembyname.folder(s).filename</param>
		/// <param name="ignoreErrors">If set to true exceptions will not abort the process. This 
		/// is useful in cases where you do not care if the sql fails. For instance dropping a table 
		/// when it does not exist in the first place.</param>
		public static void RunCustomSql(Assembly containingAssembly, DbContext context, string resourceName, bool ignoreErrors = false)
		{
			containingAssembly.CheckForNull(nameof(containingAssembly));
			context.CheckForNull(nameof(context));
		
			var stream = containingAssembly.GetManifestResourceStream(resourceName);
			if (stream != null)
			{
				try
				{
					using (var rdr = new StreamReader(stream))
                    {
						var sql = rdr.ReadToEnd();
						context.Database.ExecuteSqlRaw(sql);
					}					
				}
				catch
				{
					if (!ignoreErrors)
					{
						throw;
					}
				}
			}
			else
			{
				if (!ignoreErrors)
				{
					throw new RKToolsException($"Could not find {resourceName} to execute. Looked in {containingAssembly.FullName}", ReflectionHelpers.GetCurrentMethod());
				}
			}
		}

		/// <summary>
		/// Locates a sql file that is embedded in the assembly and executes it.
		/// </summary>
		/// <param name="containingAssembly">The assembly in which the sql can be found.</param>
		/// <param name="builder">A MigrationContext</param>
		/// <param name="resourceName">The name of the resource. This will follow he pattern
		/// assembyname.folder(s).filename</param>
		/// <param name="ignoreErrors">If set to true exceptions will not abort the process. This 
		/// is useful in cases where you do not care if the sql fails. For instance dropping a table 
		/// when it does not exist in the first place.</param>
		public static void RunCustomSql(Assembly containingAssembly, MigrationBuilder builder, string resourceName, bool ignoreErrors = false)
		{
			containingAssembly.CheckForNull(nameof(containingAssembly));
			builder.CheckForNull(nameof(builder));
			var stream = containingAssembly.GetManifestResourceStream(resourceName);
			if (stream != null)
			{
				try
				{
					using (var rdr = new StreamReader(stream))
					{
						var sql = rdr.ReadToEnd();
						builder.Sql(sql);
					}
				}
				catch
				{
					if (!ignoreErrors)
					{
						throw;
					}
				}
			}
			else
			{
				if (!ignoreErrors)
				{
					throw new RKToolsException($"Could not find {resourceName} to execute. Looked in {containingAssembly.FullName}", ReflectionHelpers.GetCurrentMethod());
				}
			}
		}
	}

}
