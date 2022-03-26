using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using OfficeOpenXml;
using RK.HotRS.ToolsCore.Extensions;

namespace RK.HotRS.ToolsCore.Helpers.Office
{
    [ExcludeFromCodeCoverage]
    public static class ExcelHelpers
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="columnNumber"></param>
        /// <returns></returns>
        public static string GetExcelColumnName(int columnNumber)
        {
            var dividend = columnNumber;
            var columnName = String.Empty;
            int modulo;
            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString(CultureInfo.CurrentCulture) + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }
            return columnName;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="range"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string SetRange(ExcelNamedRange range, long length)
        {
            range.CheckForNull(nameof(range));
            var startRange = $"{GetExcelColumnName(range.Start.Column)}{range.Start.Row}";
            string endRange;
            if (length > 0)
            {
                endRange = $"{GetExcelColumnName(range.Start.Column)}{range.Start.Row + (length - 1)}";
            }
            else
            {
                endRange = startRange;
            }
            return startRange + ":" + endRange;
        }
    }
}
