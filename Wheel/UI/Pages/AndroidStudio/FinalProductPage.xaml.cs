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
    private Thread _loadViewThread;


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
        _loadViewThread = new Thread(UpdateFinalProduct);
        _loadViewThread.Start();
    }
    public async void UpdateFinalProduct()
    {
        ConvertToPdfProgressBar.MaxValue = 8;
        ConvertToPdfProgressBar.ResetProgress();

        ConvertToPdfProgressBar.StepProgress("Locating Config file");
        //if no json file exists it will create it
        if (!File.Exists(FileFromTemp("android studio.json")))
            await CopyLocalFileAsync("Templates\\android studio.json", FileFromTemp("android studio.json"));


        ConvertToPdfProgressBar.StepProgress("Generating Source Classes");
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
        ConvertToPdfProgressBar.StepProgress("Saving and Updating Config file");

        CurrentProject.SaveConfig();
        CurrentProject.UpdateDocxRoot();

        ConvertToPdfProgressBar.StepProgress("Clearing Old PDF");
        string pdfPath = FinalFilePath + ".pdf";
        try
        {
            File.Delete(FinalFilePath);
            File.Delete(pdfPath);
        }
        catch{ }

        CurrentProject.Root.Pages.Sort();

        ConvertToPdfProgressBar.StepProgress("Adding Pages to Final DOCX file");
        foreach (Page page in CurrentProject.Root.Pages)
        {
            await page.AddPageToFinalDocx();
            page.SetupFileValues(FinalFilePath);
        }

        ConvertToPdfProgressBar.StepProgress("Converting DOCX file to PDF");
        bool converted = false;
        if(MauiProgram.IsWordInstalled)
            try
            {
                DocxParser.ConvertDocxToPdfWithWord(FinalFilePath, pdfPath);
                    converted = true;
            }
            catch
            {

            }

        if(!converted)
            try
            {
                DocxParser.ConvertDocxToPdfWithSmallWatermark(FinalFilePath, pdfPath);
            }
            catch
            {
                DocxParser.ParseDocx(FinalFilePath, FileFromTemp(pdfPath), Aspose.Words.SaveFormat.Pdf);
            }

        ConvertToPdfProgressBar.StepProgress("Refreshing View");
        await Dispatcher.DispatchAsync(() =>
        {
            DocxView.Source = FileFromTemp(FinalFileName + ".pdf");
        });


        ConvertToPdfProgressBar.StepProgress("All Ready, W_W");
    }

}