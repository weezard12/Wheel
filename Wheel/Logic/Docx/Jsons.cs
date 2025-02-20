using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
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
            public List<Page> Pages { get; set; } = new List<Page>();

            public Page? GetDocxPage(string pageName)
            {
                return Pages.FirstOrDefault(page => page.Name == pageName);
            }

            public Page? GetDocxPageByID(string pageId)
            {
                return Pages.FirstOrDefault(page => page.ID == pageId);
            }

            public bool DoesPageExist(string pageId)
            {
                return Pages.Any(page => page.ID == pageId);
            }
        }

        public class Page : IComparable<Page>
        {
            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("id")]
            public string ID { get; set; }

            [JsonPropertyName("idx")]
            public int Index { get; set; }

            [JsonPropertyName("values")]
            public List<ValueBase> Values { get; set; } = new List<ValueBase>();

            public async Task<bool> AddPageToFinalDocx()
            {
                if (File.Exists(FinalFilePath))
                {
                    await MergeDocx(FinalFilePath, FileFromTemplates(Name) + ".docx");
                    return true;
                }
                return await CopyLocalFileAsync(GetLocalPagePath(Name), FinalFilePath);
            }

            public int CompareTo(Page other)
            {
                return this.Index.CompareTo(other.Index);
            }

            public void SetupFileValues(string path)
            {
                foreach (var value in Values)
                {
                    value.SetValueInDocx(path);
                }
            }

            public void SetValueByName(string valueName, string currentValue)
            {
                var editingValue = Values.OfType<Value>().FirstOrDefault(v => v.Name == valueName);
                if (editingValue != null)
                {
                    editingValue.CurrentValue = currentValue;
                }
            }
        }


        // Apply JsonPolymorphic attribute for automatic polymorphism
        public abstract class ValueBase
        {
            [JsonPropertyName("display_name")]
            public string DisplayName { get; set; } = default!;

            [JsonPropertyName("name")]
            public string Name { get; set; } = default!;

            public virtual void SetValueInDocx(string path) { }
        }

        public class Value : ValueBase
        {
            [JsonPropertyName("baseValue")]
            public string BaseValue { get; set; } = default!;

            [JsonPropertyName("currentValue")]
            public string CurrentValue { get; set; } = default!;

            public override void SetValueInDocx(string path)
            {
                string setValue = string.IsNullOrEmpty(CurrentValue) ? BaseValue : CurrentValue;

                if (setValue.StartsWith("Path\\"))
                {
                    InsertAPicture(path, FileFromTemp(setValue.Substring(5)),5);
                    return;
                }
                if (string.IsNullOrEmpty(setValue))
                    DocxParser.SetEntryByName(path, Name, "No Value Entered");
                else if (setValue.Contains('\n'))
                    DocxParser.SetEntryByNameAndAddLines(path, Name, setValue);
                else
                    DocxParser.SetEntryByName(path, Name, setValue);

            }
        }

        public class TableValue : ValueBase
        {
            [JsonPropertyName("row1")]
            public List<string> Row1 { get; set; } = new();

            [JsonPropertyName("row2")]
            public List<string> Row2 { get; set; } = new();

            [JsonPropertyName("id")]
            public string ID { get; set; } = default!;

            public override void SetValueInDocx(string path)
            {
                DocxParser.SetEntryByName(path, Name, Row1.ToArray(), Row2.ToArray());
            }
        }

        // Custom converter for handling missing $type property
        public class ValueBaseConverter : JsonConverter<ValueBase>
        {
            public override ValueBase Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
                {
                    var root = doc.RootElement;

                    // Check if "$type" exists
                    if (!root.TryGetProperty("$type", out var typeProp))
                    {
                        // If no "$type", default to Value class
                        return JsonSerializer.Deserialize<Value>(root.GetRawText(), options)!;
                    }

                    // Handle the deserialization based on "$type"
                    var typeName = typeProp.GetString();
                    return typeName switch
                    {
                        "value" => JsonSerializer.Deserialize<Value>(root.GetRawText(), options)!,
                        "tableValue" => JsonSerializer.Deserialize<TableValue>(root.GetRawText(), options)!,
                        _ => throw new JsonException($"Unknown type: {typeName}")
                    };
                }
            }

            public override void Write(Utf8JsonWriter writer, ValueBase value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();

                // Manually write the type information
                if (value is Value)
                    writer.WriteString("$type", "value");
                else if (value is TableValue)
                    writer.WriteString("$type", "tableValue");

                // Serialize the rest of the properties
                var json = JsonSerializer.Serialize(value, value.GetType(), options);
                using (JsonDocument doc = JsonDocument.Parse(json))
                {
                    foreach (var property in doc.RootElement.EnumerateObject())
                    {
                        property.WriteTo(writer);
                    }
                }

                writer.WriteEndObject();
            }
        }
    }
}
