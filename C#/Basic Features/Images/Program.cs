using GemBox.Spreadsheet;

class Program
{
    static void Main()
    {
        // If using Professional version, put your serial key below.
        SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");

        var workbook = new ExcelFile();
        var worksheet = workbook.Worksheets.Add("Images");

        worksheet.Cells[0, 0].Value = "Image examples:";

        // Small BMP added by using rectangle.
        worksheet.Pictures.Add("SmallImage.bmp", 50, 50, 48, 48, LengthUnit.Pixel);

        // Large JPG added by using one anchor.
        worksheet.Pictures.Add("FragonardReader.jpg", "B9");

        // PNG added by using two anchors.
        worksheet.Pictures.Add("Dices.png", "J16", "K20");

        // GIF added by using anchors. Notice that animation is lost in MS Excel.
        worksheet.Pictures.Add("Zahnrad.gif",
            new AnchorCell(worksheet.Columns[9], worksheet.Rows[21], 100000, 100000),
            new AnchorCell(worksheet.Columns[10], worksheet.Rows[23], 50000, 50000)).Position.Mode = PositioningMode.Move;

        // WMF added by using one anchor and size.
        worksheet.Pictures.Add("Graphics1.wmf", "J9", 250, 100, LengthUnit.Pixel).Position.Mode = PositioningMode.FreeFloating;

        workbook.Save("Images.xlsx");
    }
}