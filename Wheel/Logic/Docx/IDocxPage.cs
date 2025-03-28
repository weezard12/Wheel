﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wheel.Logic.Docx
{
    public interface IDocxPage
    {
        public PageType Type { get; }
        public enum PageType
        {
            MainPage = 0,
            TableOfContents = 1,
            ScreensDiagramPage,
            ScreensAboutPage,
            ClassesPage,
            SourceCodePage,
            LayoutSourceCodePage,
            HowToPlay
        }
    }
}
