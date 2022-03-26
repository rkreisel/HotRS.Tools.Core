namespace HotRS.Tools.Core.Helpers.Office;

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
}
