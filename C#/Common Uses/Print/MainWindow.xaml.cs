using System.Windows;
using System.Windows.Controls;
using System.Windows.Xps.Packaging;
using GemBox.Spreadsheet;
using Microsoft.Win32;

partial class MainWindow : Window
{
    private ExcelFile workbook;

    public MainWindow()
    {
        SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");

        InitializeComponent();

        this.EnableControls();
    }

    private void LoadFileBtn_Click(object sender, RoutedEventArgs e)
    {
        var fileDialog = new OpenFileDialog();
        fileDialog.Filter = "XLSX files (*.xlsx, *.xlsm, *.xltx, *.xltm)|*.xlsx;*.xlsm;*.xltx;*.xltm|XLS files (*.xls, *.xlt)|*.xls;*.xlt|ODS files (*.ods, *.ots)|*.ods;*.ots|CSV files (*.csv, *.tsv)|*.csv;*.tsv|HTML files (*.html, *.htm)|*.html;*.htm";

        if (fileDialog.ShowDialog() == true)
        {
            this.workbook = ExcelFile.Load(fileDialog.FileName);

            this.ShowPrintPreview();
            this.EnableControls();
        }
    }

    private void SimplePrint_Click(object sender, RoutedEventArgs e)
    {
        // Print to default printer using default options
        this.workbook.Print();
    }

    private void AdvancedPrint_Click(object sender, RoutedEventArgs e)
    {
        // We can use PrintDialog for defining print options
        var printDialog = new PrintDialog();
        printDialog.UserPageRangeEnabled = true;

        if (printDialog.ShowDialog() == true)
        {
            var printOptions = new PrintOptions(printDialog.PrintTicket.GetXmlStream());

            printOptions.FromPage = printDialog.PageRange.PageFrom - 1;
            printOptions.ToPage = printDialog.PageRange.PageTo == 0 ? int.MaxValue : printDialog.PageRange.PageTo - 1;

            this.workbook.Print(printDialog.PrintQueue.FullName, printOptions);
        }
    }

    // We can use DocumentViewer for print preview (but we don't need).
    private void ShowPrintPreview()
    {
        // XpsDocument needs to stay referenced so that DocumentViewer can access additional required resources.
        // Otherwise, GC will collect/dispose XpsDocument and DocumentViewer will not work.
        var xpsDocument = workbook.ConvertToXpsDocument(SaveOptions.XpsDefault);
        this.DocViewer.Tag = xpsDocument;

        this.DocViewer.Document = xpsDocument.GetFixedDocumentSequence();
    }

    private void EnableControls()
    {
        var isEnabled = this.workbook != null;

        this.DocViewer.IsEnabled = isEnabled;
        this.SimplePrintFileBtn.IsEnabled = isEnabled;
        this.AdvancedPrintFileBtn.IsEnabled = isEnabled;
    }
}
