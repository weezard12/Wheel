using Microsoft.Msagl.Drawing;

namespace Wheel.UI;

public partial class FlowDiagramContent
{
	private FlowDiagramView _flowDiagram;
	private Graph _graph => _flowDiagram.Graph;

    public FlowDiagramContent(FlowDiagramView flowDiagram)
	{
		_flowDiagram = flowDiagram;
    }

}