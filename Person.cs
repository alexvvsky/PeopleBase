using System;
using System.Collections.Generic;
using System.Text;

namespace PeopleBase
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; } = 0;
        public Person() {

        }
    }
}
