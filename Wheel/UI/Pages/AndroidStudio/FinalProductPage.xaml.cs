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
    public const string FinalFileName = "Final Prodect.docx";
    public static string FinalFilePath => FileFromTemp(FinalFileName);
	public FinalProductPage()
	{
		InitializeComponent();
        //DocxView.Source = new HtmlWebViewSource { Html = MyUtils.ConvertDocxToHtml("C:\\Users\\User1\\AppData\\Local\\Temp\\Wheel\\final_product.docx") };

        /*        ParseDocx(FileFromTemp("test.docx"), FileFromTemp("test.pdf"),Aspose.Words.SaveFormat.Pdf);
                DocxView.Source = FileFromTemp("test.pdf");
                DocxView.Reload();*/

        UpdateFinalProduct();
    }
    public async void UpdateFinalProduct()
    {
        //if no json file exists it will create it
        if(!File.Exists(FileFromTemp("android studio.json")))
            await CopyLocalFileAsync("Templates\\android studio.json", FileFromTemp("android studio.json"));

        // parces the project json
        DocxRoot docxRoot = JsonSerializer.Deserialize<DocxRoot>(File.ReadAllText(FileFromTemp("android studio.json")));

        
        // sets up the finel product file
        docxRoot.Pages[0].CopyLocalPageTo(FinalFilePath);
        docxRoot.Pages[0].SetupFileValues(FinalFilePath);

        //bool s = DocxParser.CombineDocx(FileFromTemp("first docx.docx"), FileFromTemp("second docx.docx"));
        //this.DebugAlert(s.ToString());
        foreach (Page page in docxRoot.Pages)
        {

        }
    }
}