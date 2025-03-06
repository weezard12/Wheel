using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wheel.UI.Views.AIViews
{
    public interface IAIViewHolder
    {
        public List<IAIViewHolder> AIViews { get; set; }
        public void Generate_Clicked(object sender, EventArgs e);
    }
}
