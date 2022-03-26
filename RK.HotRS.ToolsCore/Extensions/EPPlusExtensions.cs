using OfficeOpenXml;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace RK.HotRS.ToolsCore.Extensions
{
	/// <summary>
	/// Provides extensions to the EPPlus package.
	/// </summary>
	[ExcludeFromCodeCoverage]
	public static class EPPlusExtensions
	{
		/// <summary>
		/// Ensures the byte array is a valid Excel file.
		/// This extension address a bug in EPPlus wherein a file, written from an EPPlus package object
		/// throws errors when opened in Excel. (but the file has no discernable errors)
		/// </summary>
		/// <param name="package">An EPPlus package object</param>
		/// <param name="workingPath">A directory path where the code will temporarily write the file.</param>
		/// <returns>A byte array version of the Excel content.</returns>
		public static byte[] GetValidByteArray(this ExcelPackage package, string workingPath = @".\")
		{
			package.CheckForNull(nameof(package));
			//This is a workaround for an EPPlus problem wherein saving a file results in a Excel file that reports
			//errors when opened. (Excel is able to correct these errors and resave the file, but the warning is disconcerting to the users.)
			//
			//The basic process here is to save the package to one file name, then open it and save it again with a different filename. 
			//Then return the byte array of the second file.
			//Then delete the two temporary files.
			if (!Directory.Exists(workingPath))
			{
				Directory.CreateDirectory(workingPath);
			}
			var now = DateTime.Now.ToString("yyyyMMddhhmmssfff", CultureInfo.CurrentCulture);
			var tempFile1 = new FileInfo(Path.Combine(workingPath, $"TS{now}-1.xlsx"));
			var tempFile2 = new FileInfo(Path.Combine(workingPath, $"TS{now}-2.xlsx"));
			try
			{
				package.SaveAs(tempFile1);
				using (var final = new ExcelPackage(tempFile1))
				{
					final.SaveAs(tempFile2);
					return File.ReadAllBytes(tempFile2.FullName);
				}
			}
			finally
			{
				File.Delete(tempFile1.FullName);
				File.Delete(tempFile2.FullName);
			}
		}

		/// <summary>
		/// Returns the column number of the last populated column in a row.
		/// Note: For performance reasons, the row is searched from left to 
		/// right and returns when it encounters the first empty cell. 
		/// </summary>
		/// <param name="sheet"></param>
		/// <param name="searchRow"></param>
		/// <returns></returns>
		public static int LastPopulatedColumnInRow(this ExcelWorksheet sheet, int searchRow)
		{
			sheet.CheckForNull(nameof(sheet));
			var result = sheet.Dimension.End.Column;
			while (result >= 1)
			{
                _ = sheet.Cells[searchRow, 1, searchRow, result];
                if (sheet.Cells[searchRow, result].Value != null)
				{
					break;
				}
				result--;
			}
			return result;
		}

		/// <summary>
		/// Returns the row number of the first row where all columns are empty.
		/// Note: For performance reasons the worksheet is searched top to bottom
		/// beginning at the row specified in <paramref name="beginWithRow"/> and
		/// returns when the searched row has no columns that are populated.
		/// Note: The columns searched are limited to the "used" columns as defined
		/// by the Worksheet.Dimensions.Column value;
		/// </summary>
		/// <param name="sheet"></param>
		/// <param name="beginWithRow"></param>
		/// <returns></returns>
		public static int FirstEmptyRow(this ExcelWorksheet sheet, int beginWithRow = 1)
		{
			sheet.CheckForNull(nameof(sheet));
			var result = beginWithRow;
			while (sheet.Cells[result, 1, result, sheet.Dimension.Columns].All(c => c.Value != null))
			{
				result++;
			}
			return result;
		}

		/// <summary>		
		/// Returns the row number of the first row where at least one column is empty.
		/// Note: For performance reasons the worksheet is searched top to bottom
		/// beginning at the row specified in <paramref name="beginWithRow"/> and
		/// returns when the searched row has any empty column.
		/// Note: The columns searched are limited to the "used" columns as defined
		/// by the Worksheet.Dimensions.Column value;
		/// </summary>
		/// <param name="sheet"></param>
		/// <param name="beginWithRow"></param>
		/// <returns></returns>
		public static int FirstRowWithEmptyColumn(this ExcelWorksheet sheet, int beginWithRow = 1)
		{
			sheet.CheckForNull(nameof(sheet));
			var result = beginWithRow;
			while (sheet.Cells[result, 1, result, sheet.Dimension.Columns].Any(c => c.Value != null))
			{
				result++;
			}
			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sheet">The Worksheet to be searched</param>
		/// <param name="row">The Row in the Worksheet to be searched.</param>
		/// <param name="maxCols">The maximum number of columns to search. Defaults to 100, can go to 16384.
		/// Consider using "sheet.Dimension.Columns" to reduce the nummber of columns searched.</param>
		/// <param name="blankSameAsNull">Consider an empty string as a null (ie the column is empty). Defaults = true.</param>
		/// <returns></returns>
		public static bool RowHasData(this ExcelWorksheet sheet, int row, int maxCols, bool blankSameAsNull = true)
		{
			sheet.CheckForNull(nameof(sheet));
			for (var ndx = 1; ndx <= maxCols; ndx++)
			{
				if (blankSameAsNull)
				{
					if (sheet.Cells[row, ndx].Value != null && !string.IsNullOrWhiteSpace(sheet.Cells[row, ndx].Value.ToString()))
					{
						return true;
					}
				}
				else
				{
					if (sheet.Cells[row, ndx].Value != null)
					{
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Returns the index of the first row with data
		/// </summary>
		/// <param name="sheet">The Worksheet to be searched</param>
		/// <param name="startRow">Start evaluating rows with this one.</param>
		/// <param name="stopRow"> Stop evaluating rows after this row.Defaults to 5, can go to 1,048,576.</param>
		/// <param name="maxCols"> The maximum number of columns to search. Defaults to 100, can go to 16384.
		/// Consider using "sheet.Dimension.Columns" to reduce the nummber of columns searched.</param>
		/// <param name="blankSameAsNull">Consider an empty string as a null (ie the column is empty). Defaults = true.</param>
		/// <returns>The index of the first row that has data.</returns>
		public static int FirstDataRow(this ExcelWorksheet sheet, int startRow = 1, int stopRow = 5, int maxCols = 100, bool blankSameAsNull = true)
		{
			var row = startRow;
			while (row < stopRow)
			{
				if (sheet.RowHasData(row, maxCols, blankSameAsNull))
				{
					return row;
				}
				row++;
			}
			return row;
		}
	}
}
