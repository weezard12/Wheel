﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static Wheel.Logic.Docx.DocxParser;
using static Wheel.Logic.Projects.AndroidStudioProject;
using static Wheel.Logic.MyUtils;

namespace Wheel.Logic.Docx
{
    public class Jsons
    {
        public class DocxRoot
        {
            [JsonPropertyName("pages")]
            public List<Page> Pages { get; set; }

            public Page? GetDocxPage(string pageName)
            {
                return Pages.FirstOrDefault(name => name.Name == pageName);
            }
            public Page? GetDocxPageByID(string pageId)
            {
                return Pages.FirstOrDefault(name => name.ID == pageId);
            }
        }

        public class Page
        {
            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("id")]
            public string ID { get; set; }

            [JsonPropertyName("idx")]
            public int Index { get; set; }

            [JsonPropertyName("values")]
            public List<Value> Values { get; set; }

            public void AddPageToFinalDocx()
            {
                if (File.Exists(FinalFilePath))
                {
                    MergeDocx(FinalFilePath, GetLocalPagePath(Name));
                }
                else
                    File.Copy(GetLocalPagePath(Name), FinalFilePath, true);
            }
            public void SetupFileValues(string path)
            {

                foreach (var value in Values)
                {
                    value.SetValueInDocx(path);
                }
                    
                
            }
        }

        public class Value
        {
            [JsonPropertyName("display_name")]
            public string DisplayName { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("baseValue")]
            public string BaseValue { get; set; }

            [JsonPropertyName("currentValue")]
            public string CurrentValue { get; set; }

            public void SetValueInDocx(string path)
            {
                if(CurrentValue != null)
                {
                    if (CurrentValue.StartsWith("Path\\"))
                    {
                        InsertAPicture(path, FileFromTemp(CurrentValue.Substring(5)));
                        return;
                    }
                    DocxParser.SetEntryByName(path, Name, CurrentValue);
                }

            }
        }
    }
}
