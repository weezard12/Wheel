using Wheel.Logic.CodeParser.Base;

namespace Wheel.UI;

public partial class ClassView : ContentView
{
	ClassFile ClassFile { get; set; }

	public ClassView(ClassFile classFile)
	{
		InitializeComponent();

		this.ClassFile = classFile;
		UpdateClasView();
	}

	public void UpdateClasView()
	{
        ClassName.Text = ClassFile.Name;
        AIViewsHolder.AddAITextView(new ClassDescriptionAIView(ClassFile));
    }
}