using OfficeOpenXml;
using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using RK.HotRS.ToolsCore.Exceptions;
using System.Globalization;
using System.Collections;
using RK.HotRS.ToolsCore.Properties;
using RK.HotRS.ToolsCore.Enums;
using System.Diagnostics.CodeAnalysis;

namespace RK.HotRS.ToolsCore.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ExcelExtensions
    {
#pragma warning disable CA1031 // Do not catch general exception types
        /// <summary>
        /// 
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        public static DataSet ToDataSet(this ExcelPackage package)
        {
            package.CheckForNull(nameof(package));
            var result = new DataSet();

            foreach (var workSheet in package.Workbook.Worksheets)
            {
                var table = new DataTable();
                foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
                {
                    table.Columns.Add(firstRowCell.Text);
                }
                for (var rowNumber = 2; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
                {
                    var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
                    var newRow = table.NewRow();
                    foreach (var cell in row)
                    {
                        newRow[cell.Start.Column - 1] = cell.Text;
                    }
                    table.Rows.Add(newRow);
                }
                table.TableName = workSheet.Name;
                result.Tables.Add(table);
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        public static DataTable ToDataTable(this ExcelPackage package)
        {
            package.CheckForNull(nameof(package));
            var workSheet = package.Workbook.Worksheets.First();
            var table = new DataTable();
            foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
            {
                table.Columns.Add(firstRowCell.Text);
            }

            for (var rowNumber = 2; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
            {
                var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
                var newRow = table.NewRow();
                foreach (var cell in row)
                {
                    newRow[cell.Start.Column - 1] = cell.Text;
                }
                table.Rows.Add(newRow);
            }
            return table;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="package">An EP Plus "package".</param>
        /// <param name="sheetName">By default, this method assumes the target table data is in the first worksheet in the workbook. Specify any other sheet by name here, to process it instead.</param>
        /// <param name="rangeName">The name of an existing range in the workbook/worksheet</param>
        /// <param name="includeBlankRows">Blank rows are excluded by default. Change this to true if you want them.</param>
        /// <param name="transformHeaders">Convert headers to lowercase and removed spaces and convert tick marks (') to ('')</param>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        public static DataTable ToDataTableEx(this ExcelPackage package,
            string sheetName = null,
            string rangeName = null,
            bool includeBlankRows = false,
            bool transformHeaders = false)
        {
            package.CheckForNull(nameof(package));
            ExcelWorksheet workSheet;
            ExcelNamedRange range;

            if (!string.IsNullOrWhiteSpace(sheetName))
            {
                workSheet = package.Workbook.Worksheets[sheetName];
            }
            else
            {
                workSheet = package.Workbook.Worksheets.First();
            }
            if (workSheet == null)
            {
                throw new RKToolsException($"Could not locate a worksheet named {sheetName}");
            }

            if (!string.IsNullOrWhiteSpace(rangeName))
            {
                try
                {
                    range = package.Workbook.Names[rangeName];
                }
                catch
                {
                    try
                    {
                        range = workSheet.Names[rangeName];
                    }
                    catch (Exception ex)
                    {
                        throw new RKToolsException($"Could not find requested Named Range ({rangeName}) in workbook.", ex);
                    }
                }
            }
            else
            {
                if (workSheet.Dimension == null)
                {
                    throw new RKToolsException($"No Excel Dimension in sheet. {workSheet.Name}");
                }
                range = new ExcelNamedRange($"targetRange{Guid.NewGuid()}", null, workSheet, workSheet.Dimension.Address, 1);
            }

            //Initialize output table
            var table = new DataTable
            {
                TableName = range.Name
            };

            //Headers
            foreach (var firstRowCell in workSheet.Cells[range.Start.Row, range.Start.Column, range.Start.Row, range.End.Column])
            {
                var headerText = transformHeaders
                    ? firstRowCell.Text.ToUpperInvariant()
                        .Replace(" ", string.Empty, StringComparison.InvariantCultureIgnoreCase)
                        .Replace("''", string.Empty, StringComparison.InvariantCultureIgnoreCase)
                    : firstRowCell.Text;
                table.Columns.Add(headerText);
            }

            //Data
            for (var rowNumber = range.Start.Row + 1; rowNumber <= range.End.Row; rowNumber++)
            {
                var row = workSheet.Cells[rowNumber, 1, rowNumber, range.End.Column];
                var newRow = table.NewRow();
                var rowHasData = false;
                foreach (var cell in row)
                {
                    newRow[cell.Start.Column - 1] = cell.Text;
                    if (!string.IsNullOrWhiteSpace(cell.Text))
                    {
                        rowHasData = true;
                    }
                }
                if (includeBlankRows || rowHasData)
                {
                    table.Rows.Add(newRow);
                }
            }
            return table;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="map"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        [ExcludeFromCodeCoverage]
        public static List<T> ToList<T>(this ExcelWorksheet worksheet, Dictionary<string, string> map = null) where T : new()
        {
            worksheet.CheckForNull(nameof(worksheet));

            //DateTime Conversion
            var convertDateTime = new Func<double, DateTime>(excelDate =>
            {
                if (excelDate < 1)
                    throw new ArgumentException("Excel dates cannot be smaller than 0.");

                var dateOfReference = new DateTime(1900, 1, 1);

                if (excelDate > 60d)
                    excelDate -= 2;
                else
                    excelDate -= 1;
                return dateOfReference.AddDays(excelDate);
            });

            var props = typeof(T).GetProperties()
            .Select(prop =>
            {
                var displayAttribute = (DisplayAttribute)prop.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();
                return new
                {
                    prop.Name,
                    DisplayName = displayAttribute == null ? prop.Name : displayAttribute.Name,
                    Order = displayAttribute == null || !displayAttribute.GetOrder().HasValue ? 999 : displayAttribute.Order,
                    PropertyInfo = prop,

                    prop.PropertyType,
                    HasDisplayName = displayAttribute != null
                };
            })
            .Where(prop => !string.IsNullOrWhiteSpace(prop.DisplayName))
            .ToList();

            var retList = new List<T>();
            var columns = new List<ExcelMap>();

            var start = worksheet.Dimension.Start;
            var end = worksheet.Dimension.End;
            var startCol = start.Column;
            var startRow = start.Row;
            var endCol = end.Column;
            var endRow = end.Row;

            // Assume first row has column names
            for (var col = startCol; col <= endCol; col++)
            {
                var cellValue = (worksheet.Cells[startRow, col].Value ?? string.Empty).ToString().Trim();
                if (!string.IsNullOrWhiteSpace(cellValue))
                {
                    columns.Add(new ExcelMap()
                    {
                        Name = cellValue,
                        MappedTo = map == null || map.Count == 0
                        ? cellValue
                        : map.ContainsKey(cellValue)
                            ? map[cellValue]
                            : string.Empty,
                        Index = col
                    });
                }
            }

            // Now iterate over all the rows
            for (var rowIndex = startRow + 1; rowIndex <= endRow; rowIndex++)
            {
                var item = new T();
                columns.ForEach(column =>
                {
                    var value = worksheet.Cells[rowIndex, column.Index].Value;
                    var valueStr = value == null ? string.Empty : value.ToString().Trim();
                    var prop = string.IsNullOrWhiteSpace(column.MappedTo) ?
                    null :
                    props.First(p => p.Name.Contains(column.MappedTo, StringComparison.InvariantCultureIgnoreCase));

                    // Excel stores all numbers as doubles, but we're relying on the object's property types
                    if (prop != null)
                    {
                        var propertyType = prop.PropertyType;
                        object parsedValue = null;

                        if (propertyType == typeof(int?) || propertyType == typeof(int))
                        {
                            if (!int.TryParse(valueStr, out var val))
                            {
                                val = default;
                            }

                            parsedValue = val;
                        }
                        else if (propertyType == typeof(short?) || propertyType == typeof(short))
                        {
                            if (!short.TryParse(valueStr, out var val))
                                val = default;
                            parsedValue = val;
                        }
                        else if (propertyType == typeof(long?) || propertyType == typeof(long))
                        {
                            if (!long.TryParse(valueStr, out var val))
                                val = default;
                            parsedValue = val;
                        }
                        else if (propertyType == typeof(decimal?) || propertyType == typeof(decimal))
                        {
                            if (!decimal.TryParse(valueStr, out var val))
                                val = default;
                            parsedValue = val;
                        }
                        else if (propertyType == typeof(double?) || propertyType == typeof(double))
                        {
                            if (!double.TryParse(valueStr, out var val))
                                val = default;
                            parsedValue = val;
                        }
                        else if (propertyType == typeof(DateTime?) || propertyType == typeof(DateTime))
                        {
                            parsedValue = convertDateTime((double)value);
                        }
                        else if (propertyType.IsEnum)
                        {
                            try
                            {
                                parsedValue = Enum.ToObject(propertyType, int.Parse(valueStr, CultureInfo.InvariantCulture));
                            }
                            catch
                            {
                                parsedValue = Enum.ToObject(propertyType, 0);
                            }
                        }
                        else if (propertyType == typeof(string))
                        {
                            parsedValue = valueStr;
                        }
                        else
                        {
                            try
                            {
                                parsedValue = Convert.ChangeType(value, propertyType, CultureInfo.InvariantCulture);
                            }
                            catch
                            {
                                parsedValue = valueStr;
                            }
                        }

                        //  try
                        //  {
                        prop.PropertyInfo.SetValue(item, parsedValue);
                        //  }
                        //catch (Exception ex)
                        //  {
                        // Indicate parsing error on row?
                        //}
                    }
                });

                retList.Add(item);
            }

            return retList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workSheet"></param>
        /// <param name="contentList"></param>
        /// <param name="addHeadingAsPerList"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        public static ExcelWorksheet ListToExcel<T>(this ExcelWorksheet workSheet, IList<T> contentList, bool addHeadingAsPerList)
        {
            workSheet.CheckForNull(nameof(workSheet));
            var start = addHeadingAsPerList ? "A1" : "A2";
            workSheet.Cells[start].LoadFromCollection(contentList, addHeadingAsPerList);
            return workSheet;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="map"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ValidationException"></exception>
        [ExcludeFromCodeCoverage]
        public static List<T> ToList<T>(this ExcelWorksheet worksheet, OrderedDictionary map = null) where T : new()
        {
            worksheet.CheckForNull(nameof(worksheet));
            //DateTime Conversion
            var convertDateTime = new Func<double, DateTime>(excelDate =>
            {
                if (excelDate < 1)
                    throw new ArgumentException("Excel dates cannot be smaller than 0.");

                var dateOfReference = new DateTime(1900, 1, 1);

                if (excelDate > 60d)
                    excelDate -= 2;
                else
                    excelDate -= 1;
                return dateOfReference.AddDays(excelDate);
            });

            var props = typeof(T).GetProperties()
            .Select(prop =>
            {
                var displayAttribute = (DisplayAttribute)prop.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();
                return new
                {
                    prop.Name,
                    DisplayName = displayAttribute == null ? prop.Name : displayAttribute.Name,
                    Order = displayAttribute == null || !displayAttribute.GetOrder().HasValue ? 999 : displayAttribute.Order,
                    PropertyInfo = prop,
                    prop.PropertyType,
                    HasDisplayName = displayAttribute != null
                };
            })
            .Where(prop => !string.IsNullOrWhiteSpace(prop.DisplayName))
            .ToList();

            var retList = new List<T>();
            var columns = new List<ExcelMap>();

            var start = worksheet.Dimension.Start;
            var end = worksheet.Dimension.End;
            var startCol = start.Column;
            var startRow = start.Row;
            var endCol = end.Column;
            var endRow = end.Row;

            // Assume first row has column names
            for (var col = startCol; col <= endCol; col++)
            {
                var cellValue = (worksheet.Cells[startRow, col].Value ?? string.Empty).ToString().Trim();
                if (!string.IsNullOrWhiteSpace(cellValue))
                {
                    var item = map.Cast<DictionaryEntry>().ElementAt(col - 1);

                    if (!item.Key.ToString().Contains(cellValue, StringComparison.InvariantCultureIgnoreCase))
                    {
                        throw new ValidationException(Resources.COLUMNSINUNEXPECTEDORDER);
                    }
                    columns.Add(new ExcelMap()
                    {
                        Name = cellValue,
                        MappedTo = (string)(map == null || map.Count == 0
                        ? cellValue
                        : map.Contains(cellValue)
                            ? map[cellValue]
                            : string.Empty),
                        Index = col
                    });
                }
            }

            // Now iterate over all the rows
            for (var rowIndex = startRow + 1; rowIndex <= endRow; rowIndex++)
            {
                var item = new T();
                columns.ForEach(column =>
                {
                    var value = worksheet.Cells[rowIndex, column.Index].Value;
                    var valueStr = value == null ? string.Empty : value.ToString().Trim();

                    var prop = string.IsNullOrWhiteSpace(column.MappedTo) ? null : props.First(p => p.Name.Contains(column.MappedTo, StringComparison.InvariantCultureIgnoreCase));

                    // Excel stores all numbers as doubles, but we're relying on the object's property types
                    if (prop != null)
                    {
                        var propertyType = prop.PropertyType;
                        object parsedValue = null;

                        if (propertyType == typeof(int?) || propertyType == typeof(int))
                        {
                            if (!int.TryParse(valueStr, out var val))
                            {
                                val = default;
                            }

                            parsedValue = val;
                        }
                        else if (propertyType == typeof(short?) || propertyType == typeof(short))
                        {
                            if (!short.TryParse(valueStr, out var val))
                                val = default;
                            parsedValue = val;
                        }
                        else if (propertyType == typeof(long?) || propertyType == typeof(long))
                        {
                            if (!long.TryParse(valueStr, out var val))
                                val = default;
                            parsedValue = val;
                        }
                        else if (propertyType == typeof(decimal?) || propertyType == typeof(decimal))
                        {
                            if (!decimal.TryParse(valueStr, out var val))
                                val = default;
                            parsedValue = val;
                        }
                        else if (propertyType == typeof(double?) || propertyType == typeof(double))
                        {
                            if (!double.TryParse(valueStr, out var val))
                                val = default;
                            parsedValue = val;
                        }
                        else if (propertyType == typeof(DateTime?) || propertyType == typeof(DateTime))
                        {
                            parsedValue = convertDateTime((double)value);
                        }
                        else if (propertyType.IsEnum)
                        {
                            var t = typeof(YesNoEnum);
                            if (propertyType.Equals(t))
                            {
                                try
                                {
                                    parsedValue = (int)Enum.Parse(propertyType, valueStr);
                                }
                                catch
                                {
                                    parsedValue = -1;
                                }
                            }
                            else
                            {
                                try
                                {
                                    parsedValue = Enum.ToObject(propertyType, int.Parse(valueStr, CultureInfo.InvariantCulture));
                                }
                                catch
                                {
                                    parsedValue = Enum.ToObject(propertyType, 0);
                                }
                            }
                        }
                        else if (propertyType == typeof(string))
                        {
                            parsedValue = valueStr;
                        }
                        else
                        {
                            try
                            {
                                parsedValue = Convert.ChangeType(value, propertyType, CultureInfo.InvariantCulture);
                            }
                            catch
                            {
                                parsedValue = valueStr;
                            }
                        }

                        // Some fields require lookup
                        //if (prop.Name == "PrimaryRegion" || prop.Name == "SecondaryRegion")
                        //{
                        //  var hiddenTabFriendlyColumnName = "Region Friendly Name";
                        //  var hiddenTabNonFriendlyColumnName = "Region";
                        //  parsedValue = RetrieveLookupString(value, lookupWorksheet, hiddenTabFriendlyColumnName, hiddenTabNonFriendlyColumnName);
                        //}

                        prop.PropertyInfo.SetValue(item, parsedValue);
                    }
                });

                retList.Add(item);
            }

            return retList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        [ExcludeFromCodeCoverage]
        public static bool HasExpectedColumns(this ExcelWorksheet worksheet, Dictionary<string, string> map = null)
        {
            worksheet.CheckForNull(nameof(worksheet));
            map.CheckForNull(nameof(map));
            var result = false;
            var columns = new List<ExcelMap>();
            var start = worksheet.Dimension.Start;
            var end = worksheet.Dimension.End;
            var startCol = start.Column;
            var startRow = start.Row;
            var endCol = end.Column;

            // Assume first row has column names
            for (var col = startCol; col <= endCol; col++)
            {
                var cellValue = (worksheet.Cells[startRow, col].Value ?? string.Empty).ToString().Trim();
                if (!string.IsNullOrWhiteSpace(cellValue))
                {
                    var item = map.Cast<DictionaryEntry>().ElementAt(col - 1);

                    if (!item.Key.ToString().Contains(cellValue, StringComparison.InvariantCultureIgnoreCase))
                    {
                        throw new ValidationException(Resources.COLUMNSINUNEXPECTEDORDER);
                    }
                    columns.Add(new ExcelMap()
                    {
                        Name = cellValue,
                        MappedTo = map.Any()
                            ? cellValue
                            : map.ContainsKey(cellValue)
                                ? map[cellValue]
                                : string.Empty,
                        Index = col
                    });
                }
            }
            foreach (var expectedColumn in map)
            {
                var rowResult = columns.Where(c => c.Name.Equals(expectedColumn.Key, StringComparison.InvariantCultureIgnoreCase));
                if (!rowResult.Any())
                {
                    result = false;
                    break;


                }

                result = true;
            }

            return result;
        }

        /// <summary>
        /// Sets the value of a single cell named range
        /// </summary>
        /// <param name="workbook">An ExcelWorkbook object</param>
        /// <param name="targetName">The name of the cell (single cell named ranges only) to update.</param>
        /// <param name="newValue">The new value</param>
        /// <param name="throwExceptionIfMissing">Default is false.</param>
        /// <returns></returns>
        public static bool SetNamedRangeValue(this ExcelWorkbook workbook, string targetName, string newValue, bool throwExceptionIfMissing = false)
        {
            workbook.CheckForNull(nameof(workbook));
            try
            {
                var targetCell = workbook.Names[targetName];
                if (throwExceptionIfMissing && targetCell.Start.Address != targetCell.End.Address)
                {
                    throw new RKToolsException($"Named Range ({targetName}) too large. This extension only processes single cell named ranges.");
                }
                targetCell.Value = newValue;
                return true;
            }
            catch (KeyNotFoundException)
            {
                if (throwExceptionIfMissing)
                {
                    throw;
                }
                return false;
            }
        }

        /// <summary>
        /// Return an ExcelNamedRange object if it exists or null if not.
        /// This extension method avoid the KeyNotFoundException if the requested named range is missing.
        /// </summary>
        /// <param name="workbook">An excelWorkbook object</param>
        /// <param name="rangeName">The name of the range to return.</param>
        /// <returns></returns>
        public static ExcelNamedRange GetNamedRange(this ExcelWorkbook workbook, string rangeName)
        {
            workbook.CheckForNull(nameof(workbook));
            try
            {
                return workbook.Names[rangeName];
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }

        public static void AddNamedRange(this ExcelWorkbook workbook, string sheetName, string rangeName, string address, object value = null, bool isGlobal = false, eWorkSheetHidden hiddenState = eWorkSheetHidden.Visible)
        {
            workbook.CheckForNull(nameof(workbook));
            var nr = workbook.GetNamedRange(rangeName);
            if (nr != null)
            {
                throw new RKToolsException($"Requested Named Range already exists at {nr.Address}");
            }
            if(isGlobal && workbook.Worksheets[sheetName] == null)
            {
                workbook.Worksheets.Add(sheetName);
                workbook.Worksheets[sheetName].Hidden = hiddenState;
            }
            using (ExcelNamedRange enr = isGlobal
                ? new ExcelNamedRange(rangeName, null, workbook.Worksheets[sheetName], address, 1)
                : new ExcelNamedRange(rangeName, workbook.Worksheets[sheetName], workbook.Worksheets[sheetName], $"{sheetName}!{address}", 1))
            { 
                workbook.Names.Add(rangeName, enr);
                if (value != null)
                {
                    nr = workbook.GetNamedRange(rangeName);
                    nr.Value = value;
                }
            }
        }
}

    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ExcelMap
{
    public string Name { get; set; }
    public string MappedTo { get; set; }
    public int Index { get; set; }
}
#pragma warning restore CA1031 // Do not catch general exception types
}
