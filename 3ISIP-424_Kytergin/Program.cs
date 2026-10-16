using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static _3ISIP_424_Kytergin.Program;

namespace _3ISIP_424_Kytergin
{
    internal class Program
    {
        static void Main(string[] args)
        {
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
                        AddStudent(students);
                        break;
                    case "2":
                        ShowStudents(students);
                        break;
                    case "3":
                        AddTeachers(teachers);
                        break;
                    case "4":
                        ShowTeachers(teachers);
                        break;
                    case "5":
                        AddCourse(teachers, courses);
                        break;
                    case "6":
                        ShowCourses(courses);
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
        static void AddCourse(List <Teacher> teachers, List <Course> courses)
        {
            Console.Write("Введите название курса: ");
            string name = Console.ReadLine();
            Console.WriteLine("Все преподаватели:");
            ShowTeachers(teachers);
            Console.Write("Впишите номер того, кто будет вести этот курс: ");
            int num = int.Parse(Console.ReadLine());
            Teacher teacher = teachers[num - 1];
            Course course = new Course(name, teacher);
            courses.Add(course);
            Console.WriteLine($"Курс {name} с учителем {teacher.name} успешно добавлен! ");
        }
        static void AddTeachers(List<Teacher> teachers)
        {
            Console.Write("Введите имя преподавателя: ");
            string name = Console.ReadLine();
            Console.Write("Введите возраст преподавателя: ");
            int age = int.Parse(Console.ReadLine());
            Teacher teacher = new Teacher(name, age);
            teachers.Add(teacher);
            Console.WriteLine($"Преподаватель {teacher.name} добавлен.");
        }
        static void ShowStudents(List<Student> students)
        {
            int i = 1;
            foreach (Student st in students)
            {
                Console.WriteLine($"Студент №{i}. {st.name}, {st.age} лет, группа: {st.group}");
                i++;
            }
        }
        static void ShowTeachers(List<Teacher> teachers)
        {
            int i = 1;
            foreach (Teacher t in teachers)
            {
                Console.WriteLine($"Преподаватель №{i}. {t.name}, {t.age} лет");
                i++;
            }
        }
        static void ShowCourses(List<Course> courses)
        {
            int i = 1;
            Console.WriteLine("Все курсы: ");
            foreach (Course course in courses)
            {
                Console.WriteLine($"№{i}. имя - {course.name}, учитель - {course.teacher.name}");
            }
        }
        static void AddStudent(List<Student> students)
        {
            Console.Write("Введите имя студента: ");
            string name = Console.ReadLine();
            Console.Write("Введите возраст студента: ");
            int age = int.Parse(Console.ReadLine());
            Console.Write("Введите группу студента: ");
            string group = Console.ReadLine();
            Student student = new Student(name, age, group);
            students.Add(student);
            Console.WriteLine($"Студент {student.name} добавлен.");
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
            public string name;
            public Teacher teacher;
            List<Student> students = new List<Student>();
            public Course(string name, Teacher teacher)
            {
                this.name = name;
                this.teacher = teacher;
            }
        }
    }
}
