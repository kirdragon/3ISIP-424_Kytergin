using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP_424_Kytergin
{
    internal class Program
    {
        static void Main(string[] args)
        {
        }
        public class Person
        {
            public int age;
            public string name;
            public Person(string name, int age)
            {
                this.name = name;
                this.age = age;
            }
        }
        public class Student: Person
        {
            public string group;
            public Student(string name, int age, string group) : base(name, age)
            {
                this.group = group;
            }
        }
        public class Course
        {
            string name;
            string teacher;
            List<Student> students = new List<Student>();
        }
    }
}
