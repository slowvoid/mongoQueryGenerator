using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests.CaseStudy.Models
{
    /// <summary>
    /// Represents the User entity
    /// </summary>
    public class User
    {
        [JsonProperty("user_id")]
        public int UserID { get; set; }
        [JsonProperty("user_name")]
        public string Name { get; set; }
    }
}
