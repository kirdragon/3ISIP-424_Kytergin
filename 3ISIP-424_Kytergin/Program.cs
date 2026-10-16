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
            string name;
            int age;
            string group;
            List<Student> students = new List<Student>();
            List<Teacher> teachers = new List<Teacher>();
            List<Course> courses = new List<Course>();
            while (true)
            {
                Console.WriteLine("\nСИСТЕМА УПРАВЛЕНИЯ УНИВЕРСИТЕТОМ ");
                Console.WriteLine("1. Добавить студента");
                Console.WriteLine("2. Показать всех студентов");
                Console.WriteLine("3. Добавить преподавателя");
                Console.WriteLine("4. Показать всех преподавателей");
                Console.WriteLine("5. Создать курс");
                Console.WriteLine("6. Показать все курсы");
                Console.WriteLine("7. Записать студента на курс");
                Console.WriteLine("8. Показать курсы студента");
                Console.WriteLine("9. Показать студентов курса");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите действие: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Введите имя студента: ");
                        name = Console.ReadLine();
                        Console.Write("Введите возраст студента: ");
                        age = int.Parse(Console.ReadLine());
                        Console.Write("Введите группу студента: ");
                        group = Console.ReadLine();
                        Student student = new Student(name, age, group);
                        students.Add(student);
                        Console.WriteLine($"Студент {student.name} добавлен.");
                        break;
                    case "2":
                    int i = 1;
                        foreach (Student st in students)
                        {
                            Console.WriteLine($"Студент №{i}. {st.name}, {st.age} лет, группа: {st.group}");
                            i++;
                        }
                            break;
                    case "3":
                        Console.Write("Введите имя преподавателя: ");
                        name = Console.ReadLine();
                        Console.Write("Введите возраст преподавателя: ");
                        age = int.Parse(Console.ReadLine());
                        Teacher teacher = new Teacher(name, age);
                        teachers.Add(teacher);
                        Console.WriteLine($"Преподаватель {teacher.name} добавлен.");
                        break;
                    case "4":
                        // Вывод преподавателей
                        break;
                    case "5":
                        // Создание курса
                        break;
                    case "6":
                        // Вывод курсов
                        break;
                    case "7":
                        // Запись студента на курс
                        break;
                    case "8":
                        // Курсы конкретного студента
                        break;
                    case "9":
                        // Студенты конкретного курса
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный пункт меню!");
                        break;
                }
            }
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
        public class Student : Person
        {
            public string group;
            public Student(string name, int age, string group) : base(name, age)
            {
                this.group = group;
            }
            public List<Course> courses = new List<Course>();

        }
        public class Teacher : Person
        {
        public List<Course> courses = new List<Course>();
        public Teacher(string name, int age) : base(name, age)
            {
            }
        }
        public class Course
        {
            string name;
            Teacher teacher;
            List<Student> students = new List<Student>();
            public Course(string name, Teacher teacher)
            {
                this.name = name;
                this.teacher = teacher;
            }
        }
    }
}
