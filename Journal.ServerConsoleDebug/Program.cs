using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

using Journal.Server.Database;
using Journal.Common.Entities;
using System.Linq;

namespace Journal.ServerConsoleDebug
{
    class Program
    {
        const string IMPORT_FILE_NAME = "importData.txt";
        static readonly string ExecutableDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        static async Task Main(string[] args)
        {
            string importFilePath = Path.Combine(ExecutableDirectory, IMPORT_FILE_NAME);
            if (!File.Exists(importFilePath))
            {
                Console.WriteLine($"Файл: \"{ importFilePath }\" не найден");
                return;
            }
            
            JournalDbContext.SetConnectionString("Server=localhost; Database=studentjournal; Uid=root; Pwd=root; CharSet=utf8");
            JournalDbContext dbContext = JournalDbContext.CreateContext();

            dbContext.Database.EnsureCreated();

            using (StreamReader sr = File.OpenText(importFilePath))
            {
                string line;
                for (int userId =1; (line = sr.ReadLine()) != null; userId++)
                {
                    string[] splittedLine = line.Split(' ');
                    string firstName = splittedLine[1]
                        , surname = splittedLine[0]
                        , lastName = splittedLine[2];

                    User user = new User(firstName, surname, $"student{userId}", Journal.Server.Security.Hash.GetFromString("user"), UserRole.Student)
                    { 
                        LastName = lastName
                    };

                    dbContext.Users.Add(user);

                    Student student = new Student(user);

                    dbContext.Students.Add(student);
                }
                
                await dbContext.SaveChangesAsync();
            }

            Console.WriteLine("done");
            
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
