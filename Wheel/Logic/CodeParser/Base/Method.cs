using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization; // For System.Text.Json
using Newtonsoft.Json; // For Newtonsoft.Json
using Wheel.Logic.CodeParser.enums;

namespace Wheel.Logic.CodeParser.Base
{
    [Serializable] // Marks the class as serializable
    public class Method : INameable, IContent, IDescription
    {
        [JsonPropertyName("name")] // System.Text.Json attribute
        [JsonProperty("name")] // Newtonsoft.Json attribute
        public string Name { get; set; }

        [JsonPropertyName("content")]
        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("parameters")]
        [JsonProperty("parameters")]
        public List<Variable> Parameters { get; set; } = new List<Variable>();

        public override string ToString()
        {
            string parameterString = string.Join(", ", Parameters.Select(p => $"{p.Type} {p.Name}"));
            return $"{Name}({parameterString})";
        }
    }
}
