using System;
using System.Threading.Tasks;

namespace person_console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("One moment please...");

            var reader = new PersonReader();
            var people = await reader.GetAsync();
            foreach(var person in people)
            {
                Console.WriteLine(person);
            }

            Console.WriteLine("===============");
        }
    }
}
