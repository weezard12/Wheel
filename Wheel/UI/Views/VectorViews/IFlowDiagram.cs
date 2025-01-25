using Microsoft.Msagl.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wheel.UI.Views.VectorViews
{
    public interface IFlowDiagram
    {
         public FlowDiagramView FlowDiagram { get; protected set; }
         Graph DiagramGraph { get; }

    }
}
