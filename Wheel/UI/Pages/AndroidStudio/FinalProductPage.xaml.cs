using DocumentFormat.OpenXml.Bibliography;
using System.Text.Json;
using Wheel.Logic;
using Wheel.Logic.Docx;
using static Wheel.Logic.Docx.Jsons;
using static Wheel.Logic.MyUtils;
using Page = Wheel.Logic.Docx.Jsons.Page;

namespace Wheel.UI.Pages.AndroidStudio;

public partial class FinalProductPage : ContentPage
{
	public FinalProductPage()
	{
		InitializeComponent();
        //DocxView.Source = new HtmlWebViewSource { Html = MyUtils.ConvertDocxToHtml("C:\\Users\\User1\\AppData\\Local\\Temp\\Wheel\\final_product.docx") };

        /*        ParseDocx(FileFromTemp("test.docx"), FileFromTemp("test.pdf"),Aspose.Words.SaveFormat.Pdf);
                DocxView.Source = FileFromTemp("test.pdf");
                DocxView.Reload();*/
        UpdateFinalProduct();
    }
    public void UpdateFinalProduct()
    {
        //if no json file exists it will create it
        if(!File.Exists(FileFromTemp("android studio.json")))
            CopyLocalFileAsync("Templates\\android studio.json", FileFromTemp("android studio.json"));

        //DocxRoot docxRoot = JsonSerializer.Deserialize<DocxRoot>(File.ReadAllText(FileFromTemp("android studio.json")));
        bool s = DocxParser.CombineDocx(FileFromTemp("first docx.docx"), FileFromTemp("second docx.docx"));
        this.DebugAlert(s.ToString());
/*        foreach (Page page in docxRoot.Pages)
        {

        }*/
    }
}