using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Web_Movie_BI_Analytics
{
    public class ObjectClasses
    {
        public class MovieData
        {
            public ObjectId _id { get; set; }
            public string Link { get; set; }
            public string Name { get; set; }
            public string Year { get; set; }
            public string Rating { get; set; }
            public string Overview { get; set; }
            public string Language { get; set; }
            public string Runtime { get; set; }
            public string Budget { get; set; }
            public string Revenue { get; set; }
            public string HomePage { get; set; }
            public string Release { get; set; }
            public string Genre { get; set; }

            public IEnumerable<Person> Cast { get; set; }
        }

        public class Person
        {
            public string Name { get; set; }
            public string Department { get; set; }
            public string Role { get; set; }
            public string Character { get; set; }
        }
    }
}
