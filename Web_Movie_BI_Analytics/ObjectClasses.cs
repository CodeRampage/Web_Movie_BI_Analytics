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
            public string ReleaseStatus { get; set; }

            public List<Person> Cast { get; set; }
        }

        public class Person
        {
            public string Name { get; set; }
            public string Character { get; set; }
            public string Gender { get; set; }
            public string BirthPlace { get; set; }
            public string Credits { get; set; }
        }

        public class System_user
        {
            public string USERN { get; set; }
            public string PASS { get; set; }
            public string USERT { get; set; }
            public string LAST { get; set; }
            public string FIRST { get; set; }
            public string NAMES { get; set; }
        }
    }
}
