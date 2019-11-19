using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace person_api.Models
{
    public class CSVPeopleProvider : IPeopleProvider
    {
        public string FileData { get; private set; }

        public CSVPeopleProvider()
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "People.txt";
            LoadFile(filePath);
        }

        public void LoadFile(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                FileData = reader.ReadToEnd();
            }
        }

        public List<Person> GetPeople()
        {
            var people = ParseString(FileData);
            return people;
        }

        public Person GetPerson(int id)
        {
            var people = GetPeople();
            return people?.FirstOrDefault(p => p.Id == id);
        }

        private List<Person> ParseString(string csvData)
        {
            var people = new List<Person>();

            var lines = csvData.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            foreach (string line in lines)
            {
                try
                {
                    var elems = line.Split(',');
                    var per = new Person()
                    {
                        Id = Int32.Parse(elems[0]),
                        GivenName = elems[1],
                        FamilyName = elems[2],
                        StartDate = DateTime.Parse(elems[3]),
                        Rating = Int32.Parse(elems[4]),
                        FormatString = elems[5],
                    };
                    people.Add(per);
                }
                catch (Exception)
                {
                    // Skip the bad record, log it, and move to the next record
                    // log.write("Unable to parse record", per);
                }
            }
            return people;
        }
    }
}
