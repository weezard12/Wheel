using DocumentFormat.OpenXml.Bibliography;
using System.Text.Json;
using Wheel.Logic;
using Wheel.Logic.Docx;
using static Wheel.Logic.Docx.Jsons;
using static Wheel.Logic.MyUtils;
using Page = Wheel.Logic.Docx.Jsons.Page;
using static Wheel.Logic.Projects.AndroidStudioProject;
using Wheel.Logic.Projects;
using Wheel.UI.Views;
using Wheel.Logic.CodeParser.Base;
using static Wheel.Logic.Docx.IDocxPage;

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
        LoadFolderView exportView = new LoadFolderView("Export");
        exportView.OnSelectedFileSet += (ex) =>
        {
            if (ex != null)
                return;
            File.Copy(FinalFilePath,Path.Combine(exportView.SelectedFolderPath, CurrentProject.Name + ".docx"));
        };
        Grid.SetColumn(exportView, 0);
        ExportGrid.Add(exportView);

        UpdateFinalProduct();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        Thread loadViewThread = new Thread(UpdateFinalProduct);
        loadViewThread.Start();
    }
    public async void UpdateFinalProduct()
    {
        //if no json file exists it will create it
        if(!File.Exists(FileFromTemp("android studio.json")))
            await CopyLocalFileAsync("Templates\\android studio.json", FileFromTemp("android studio.json"));

        int a = CurrentProject.AllProjectFiles.Count;
        int b = CurrentProject.ClassFiles.Count;
        // add source code files
        foreach (ProjectFile file in CurrentProject.AllProjectFiles)
        {
            if(file is ContentProjectFile contentFile)
                if (!CurrentProject.Root.DoesPageExist(contentFile.Name))
                {
                    CurrentProject.Root.Pages.Add(new Page()
                    {
                        ID = contentFile.Name,
                        Name = "Source Code File",
                        Index = (int) PageType.SourceCodePage,
                        Values = new List<ValueBase>()
                        {
                            new Value(){ Name = "file_name", CurrentValue = contentFile.Name },
                            new Value(){ Name = "file_content", CurrentValue = contentFile.Content },
                        }
                        
                    });
                }
        }
        CurrentProject.SaveConfig();

        CurrentProject.UpdateDocxRoot();
        
        string pdfPath = FinalFilePath + ".pdf";
        try
        {
            File.Delete(FinalFilePath);
            File.Delete(pdfPath);
        }
        catch{ }

        foreach (Page page in CurrentProject.Root.Pages)
        {
            await page.AddPageToFinalDocx();
            page.SetupFileValues(FinalFilePath);
        }
        
        try
        {
            DocxParser.ConvertDocxToPdfWithSmallWatermark(FinalFilePath, pdfPath);
        }
        catch
        {
            DocxParser.ParseDocx(FinalFilePath, FileFromTemp(pdfPath), Aspose.Words.SaveFormat.Pdf);
        }

        DocxParser.RemoveTextFromPdf(pdfPath, pdfPath + "no.pdf", "The");

        await Dispatcher.DispatchAsync(() =>
        {
            DocxView.Source = FileFromTemp(FinalFileName + ".pdf");
        });
        
        
    }

}