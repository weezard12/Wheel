using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Wheel.Logic.CodeParser.enums;

namespace Wheel.Logic.CodeParser.Base
{
    public class Variable : INameable, IDescription
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        public string BaseValue { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }

        public override string ToString()
        {
            return String.Format("{0} {1}", Type, Name);
        }
    }
}
