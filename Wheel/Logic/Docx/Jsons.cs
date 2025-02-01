using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static Wheel.Logic.Docx.DocxParser;

namespace Wheel.Logic.Docx
{
    internal class Jsons
    {
        public class DocxRoot
        {
            [JsonPropertyName("pages")]
            public List<Page> Pages { get; set; }
        }

        public class Page
        {
            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("idx")]
            public int Index { get; set; }

            [JsonPropertyName("values")]
            public List<Value> Values { get; set; }

            public void CopyLocalPageTo(string copyPath)
            {
                File.Copy(GetLocalPagePath(Name), copyPath, true);
            }
            public void SetupFileValues(string path)
            {

                foreach (var value in Values)
                    DocxParser.SetEntryByName(path, value.Name,value.CurrentValue);
                
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
        }
    }
}
