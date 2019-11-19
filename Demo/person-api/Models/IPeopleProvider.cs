using System.Collections.Generic;

namespace person_api.Models
{
    public interface IPeopleProvider
    {
        public List<Person> GetPeople();
        public Person GetPerson(int id);
    }
}