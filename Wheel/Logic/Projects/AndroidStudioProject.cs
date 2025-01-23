using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wheel.Logic.CodeParser.Base;
using Wheel.UI;

namespace Wheel.Logic.Projects
{
    public class AndroidStudioProject : ProjectBase
    {
        List<ClassFile> Classes;

        public AndroidStudioProject(string ProjectName) : base(ProjectName)
        {
            
        }

        public void SetupProjectClasses(List<ClassFile> classes)
        {
            this.Classes = classes;
        }

        public override void SetupUI()
        {
/*            AddEntry(new ProjectValueEntryView("Project Name"));
            AddEntry(new ProjectValueEntryView("Work ID",value: "עבודת גמר תכנון ותכנות מערכות 883589"));
            AddEntry(new ProjectValueEntryView("Publisher"));
            AddEntry(new ProjectValueEntryView("Publisher ID"));
            AddEntry(new ProjectValueEntryView("Publisher ID"));*/
        }
    }
}
