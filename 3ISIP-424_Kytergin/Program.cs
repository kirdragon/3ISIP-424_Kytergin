using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
                Console.WriteLine("9. Показать студентов курсов");
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
                        StudenToCourse(students, courses);
                        break;
                    case "8":
                        ShowStudentsCourses(students);
                        break;
                    case "9":
                        CoursesStudents(courses);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Ошибка ввода! Вводите только целые числа от 0 до 9!");
                        break;
                }
            }
        }
        public static bool CheckExistTeachers(List<Teacher> teachers)
        {
            if (teachers.Count == 0)
            {
                Console.WriteLine("Пока нет ни одного преподавателя!");
                return false;
            }
            return true;
        }
        public static bool CheckExistStudents(List<Student> students)
        {
            if (students.Count == 0)
            {
                Console.WriteLine("Пока нет ни 1 студента!");
                return false;
            }
            return true;
        }
        public static bool CheckExistCourses(List<Course> courses)
        {
            if (courses.Count == 0)
            {
                Console.WriteLine("Пока нет ни 1 курса!");
                return false;
            }
            return true;
        }
        public static string GetString(string message)
        {
            Console.Write(message);
            string input = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(input))
            {
                Console.Write("Ошибка! Значение не должно быть пустым!\nНапиши еще раз: ");
                input = Console.ReadLine();
            }
            return input.Trim();
        }
        public static int GetInt(string message, int min, int max)
        {
            Console.Write(message);
            while (true)
            {
                int nubmer;
                if (int.TryParse(Console.ReadLine(), out nubmer) && nubmer >= min && nubmer <= max)
                {
                    return nubmer;
                }
                Console.Write($"Ошибка! Напишите целое число от {min} до {max}: ");
            }
        }
        static void CoursesStudents(List<Course> courses)
        {
            if (!CheckExistCourses(courses))
            {
                return;
            }
            ShowCourses(courses);
            int num = GetInt("Введите номер курса, студентов которого хотите узнать: ", 1, courses.Count);
            Course course = courses[num - 1];
            if (course.students.Count == 0)
            {
                Console.WriteLine("На этом курсе пока нет студентов!");
                return;
            }
            int i = 1;
            foreach (Student student in course.students)
            {
                Console.WriteLine($"№{i}. {student.name}, группа {student.group}");
                i++;
            }
        }
        static void ShowStudentsCourses(List<Student> students)
        {
            if (!CheckExistStudents(students))
            {
                return;
            }
            ShowStudents(students);
            int num = GetInt("Введите номер студента, курсы которого хотите узнать: ", 1, students.Count);
            Student student = students[num - 1];
            if (student.courses.Count == 0)
            {
                Console.WriteLine("Студент пока не записан ни на один курс!");
                return;
            }
            int i = 1;
            foreach (Course course in student.courses)
            {
                Console.WriteLine($"№{i}. Название - {course.name}");
                i++;
            }
        }
        static void StudenToCourse(List<Student> students, List<Course> courses)
        {
            if (!CheckExistStudents(students) || !CheckExistCourses(courses))
            {
                return;
            }
            ShowStudents(students);
            int st_num = GetInt("Впишите номер студента, которого хотите записать: ", 1, students.Count);
            ShowCourses(courses);
            int course_num = GetInt("Впишите номер курса, куда хотите записать студента: ", 1, courses.Count);
            Student student = students[st_num - 1];
            Course course = courses[course_num - 1];
            if (student.courses.Contains(course))
            {
                Console.WriteLine("Студент уже записан на этот курс!");
                return;
            }
            student.courses.Add(course);
            course.students.Add(student);
            Console.WriteLine($"Студент {student.name} записан на курс {course.name}.");
        }
        static void AddCourse(List<Teacher> teachers, List<Course> courses)
        {
            if (!CheckExistTeachers(teachers))
            {
                return;
            }
            string name = GetString("Введите название курса: ");
            Console.WriteLine("Все преподаватели:");
            ShowTeachers(teachers);
            int num = GetInt("Впишите номер того, кто будет вести этот курс: ", 1, teachers.Count);
            Teacher teacher = teachers[num - 1];
            Course course = new Course(name, teacher);
            courses.Add(course);
            teacher.courses.Add(course);
            Console.WriteLine($"Курс {name} с учителем {teacher.name} успешно добавлен! ");
        }
        static void AddTeachers(List<Teacher> teachers)
        {
            string name = GetString("Введите имя преподавателя: ");
            int age = GetInt("Введите возраст преподавателя: ", 20, 100);
            Teacher teacher = new Teacher(name, age);
            teachers.Add(teacher);
            Console.WriteLine($"Преподаватель {teacher.name} добавлен.");
        }
        static void ShowStudents(List<Student> students)
        {
            int i = 1;
            if (students.Count == 0)
            {
                Console.WriteLine("Список студентов пуст!");
                return;
            }
            Console.WriteLine("Список студентов:");
            foreach (Student st in students)
            {
                Console.WriteLine($"Студент №{i}. {st.name}, {st.age} лет, группа: {st.group}");
                i++;
            }
        }
        static void ShowTeachers(List<Teacher> teachers)
        {
            int i = 1;
            if (teachers.Count == 0)
            {
                Console.WriteLine("Список преподавателей пуст!");
                return;
            }
            foreach (Teacher t in teachers)
            {
                Console.WriteLine($"Преподаватель №{i}. {t.name}, {t.age} лет");
                i++;
            }
        }
        static void ShowCourses(List<Course> courses)
        {
            int i = 1;
            if (courses.Count == 0)
            {
                Console.WriteLine("Список курсов пуст!");
                return;
            }
            Console.WriteLine("Все курсы: ");
            foreach (Course course in courses)
            {
                Console.WriteLine($"№{i}. имя - {course.name}, учитель - {course.teacher.name}");
                i++;
            }
        }
        static void AddStudent(List<Student> students)
        {
            string name = GetString("Введите имя студента: ");
            int age = GetInt("Введите возраст студента: ", 15, 100);
            string group = GetString("Введите группу студента: ");
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
            public List<Student> students = new List<Student>();
            public Course(string name, Teacher teacher)
            {
                this.name = name;
                this.teacher = teacher;
            }
        }
    }
}