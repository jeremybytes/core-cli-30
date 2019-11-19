using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using person_api.Models;

namespace person_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PeopleController : ControllerBase
    {
        IPeopleProvider provider;

        public PeopleController(IPeopleProvider provider)
        {
            this.provider = provider;
        }

        [HttpGet]
        public List<Person> Get()
        {
            return provider.GetPeople();
        }

        [HttpGet("{id}")]
        public Person Get(int id)
        {
            return provider.GetPerson(id);
        }
    }
}
