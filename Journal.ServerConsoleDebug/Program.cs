using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Journal.Server.Database;
using Journal.Common.Entities;
using System.Linq;

namespace Journal.ServerConsoleDebug
{
    class Program
    {
        static async Task Main(string[] args)
        {
            JournalDbContext.SetConnectionString("Server=localhost; Database=studentjournal; Uid=root; Pwd=root; CharSet=utf8");
            JournalDbContext dbContext = JournalDbContext.CreateContext();

            dbContext.Database.EnsureCreated();

            List<Student> students = new List<Student>();
            for (int i = 0; i < 10; i++)
            {
                User user = _GenerateUser(Common.Entities.UserRole.Student);
                Student student = new Student(user);

                students.Add(student);
                
                dbContext.Users.Add(user);
                dbContext.Students.Add(student);
            }

            List<Subject> subjects = new List<Subject>();
            for (int i = 0; i < 10; i++)
            {
                Subject subject = new Subject(_GenerateRandomString(10));
                subjects.Add(subject);
            }
            dbContext.Subjects.AddRange(subjects);


            List<Specialty> specialties = new List<Specialty>();
            for (int i = 0; i < 10; i++)
            {
                int subjOffset = _rand.Next(0, subjects.Count - 1);
                int subjCount = _rand.Next(1, subjects.Count - subjOffset - 1);

                var specialty = new Specialty(_GenerateRandomString(10), _GenerateRandomString(5), 4, subjects.Skip(subjOffset).Take(subjCount));
                specialties.Add(specialty);
            }
            dbContext.Specialties.AddRange(specialties);

            List<StudentGroup> studentGroups = new List<StudentGroup>();
            for (int i = 0; i < 2; i++)
            {
                StudentGroup studentGroup = new StudentGroup(specialties[_rand.Next(0, specialties.Count)], 1, 1, students);
                studentGroups.Add(studentGroup);
                dbContext.Groups.Add(studentGroup);
            }

            List<Teacher> teachers = new List<Teacher>();
            for (int i = 0; i < 10; i++)
            {
                User user = _GenerateUser(Common.Entities.UserRole.Teacher);
                dbContext.Users.Add(user);
                Teacher teacher = new Teacher(user);
                dbContext.Teachers.Add(teacher);
                teachers.Add(teacher);
            }

            for (int i = 0; i < studentGroups.Count; i++)
                studentGroups[i].CuratorEnt = teachers[i].UserEnt;

            


            List<Grade> grades = new List<Grade>();
            for (int i = 0; i < students.Count; i++) 
            {
                Grade grade = new Grade(_GetRandom(teachers).UserEnt, _GetRandom(subjects), _GetRandom(students), _GetRandom<GradeLevel>(), DateTime.Now, _GenerateRandomString(15));
                grades.Add(grade);
            }
            dbContext.Grades.AddRange(grades);
            
            await dbContext.SaveChangesAsync();
        }

        static Random _rand = new Random();

        static string _GenerateRandomString(int length)
        {
            if (length == 0)
                return string.Empty;

            Random rand = new Random();
            string result = string.Empty;
            for (int i = 0; i < length; i++)
            {
                result += (rand.Next(0, 4) switch
                {
                    0 => (char)rand.Next('a','z' + 1),
                    1 => (char)rand.Next('A','Z' + 1),
                    2 => (char)rand.Next('0', '9' + 1),
                    3 => (char)rand.Next('а', 'я'),
                    _ => ' '
                });
            }
            return result;
        }
    
        static User _GenerateUser(UserRole role)
        {
            User user = new User(_GenerateRandomString(10), _GenerateRandomString(10), _GenerateRandomString(10), Journal.Server.Security.Hash.GetFromString("iamstudent"), role);
            return user;
        }
    
        static T _GetRandom<T>(IList<T> collection)
            => collection[_rand.Next(0, collection.Count)];
        
        static T _GetRandom<T>() where T : Enum
        {
            T[] values = (T[])Enum.GetValues(typeof(T));
            return _GetRandom<T>(values);
        }

    }
}
