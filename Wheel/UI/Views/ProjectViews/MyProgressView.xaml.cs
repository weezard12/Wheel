
namespace Wheel.UI.Views;

public partial class MyProgressView : ContentView
{
	private int _maxValue = 10;
	public int MaxValue { get => _maxValue; set => SetMaxValue(value); }

	private int _currentValue = 0;
	public int CurrentValue { get => _currentValue; set => SetCurrentValue(value); }

	private string _text;
	public string ProgressBarText { get => _text; set => SetCurrentText(value); }


    public MyProgressView()
	{
		InitializeComponent();
	}
	public void ResetProgress()
	{
        CurrentValue = 0;
		ProgressBarText = String.Empty;
    }
	public void StepProgress(string progressBarText)
	{
        ProgressBarText = progressBarText;
		CurrentValue = CurrentValue + 1;
    }

	private void SetCurrentValue(int value)
	{
		_currentValue = value;
        try
        {
            Dispatcher.Dispatch(() =>
            {
                ProgressBarXml.Progress = (float) _currentValue / _maxValue;
            });
        }
        catch { }
	}

    private void SetMaxValue(int value)
    {
        _maxValue = (value != 0) ? value : 1;
        ProgressBarXml.Progress = _currentValue / _maxValue;
    }

    private void SetCurrentText(string value)
    {
        _text = value;
        try
        {
            Dispatcher.Dispatch(() =>
            {
                if (String.IsNullOrEmpty(_text))
                    ProgressBarTextXml.IsVisible = false;
                else
                {
                    ProgressBarTextXml.Text = _text;
                    ProgressBarTextXml.IsVisible = true;
                }
            });
        }
        catch (Exception ex)
        {

        }
    }
}