namespace HotRS.Tools.Core.Helpers.Office;

/// <summary>
/// Excel tools
/// </summary>
[ExcludeFromCodeCoverage]
public static class ExcelHelpers
{
    /// <summary>
    /// Gets the "name" of a column from its ordinal number. Use this to get the alphabetic value of an integer coulmn number. For instance 27 will return "AA"
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
