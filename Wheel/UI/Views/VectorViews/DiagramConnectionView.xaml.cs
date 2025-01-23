using Microsoft.Msagl.Drawing;

namespace Wheel.UI;

public partial class DiagramConnectionView : ContentView
{
	Node _node;
	public DiagramConnectionView(Node node)
	{
		InitializeComponent();
		_node = node;

	}
}