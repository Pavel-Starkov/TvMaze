using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace PageProcessor.Models
{
    public class ShowCastModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public ICollection<CastModel> cast => sourceCast?.OrderByDescending(p => DateTime.TryParse(p.person.birthday, out var dt) ? dt : (DateTime?)null).Select(p => p.person).ToList();

        [JsonIgnore]
        public ICollection<Person> sourceCast;

        public class Person
        {
            public CastModel person;
        }
    }
}
