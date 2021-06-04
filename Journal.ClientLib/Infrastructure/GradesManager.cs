using Journal.ClientLib.Entities;
using Journal.Common.Entities;

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Journal.ClientLib.Infrastructure
{
    public class GradesManager : ControllerManagerBase
    {
        public GradesManager(IJournalClient client)
        {
            SetExecuter(client.QueryExecuter);
        }

        private GradesManager() { }

        public Task<Grade> Paste(int studentId, int subjectId, GradeLevel gradeLevel, DateTime? timestamp = null, string reason = null)
        {
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                ["studentId"] = studentId.ToString(),
                ["subjectId"] = subjectId.ToString(),
                ["level"] = ((int)gradeLevel).ToString()
            };

            if (timestamp.HasValue)
                args["timestamp"] = timestamp.Value.ToString();
            
            if (!string.IsNullOrWhiteSpace(reason))
                args["reason"] = reason;

            return QueryExecuter.ExecuteGetQuery<Grade>(CONTROLLER_NAME, "Paste", args);
        }

        public Task<Grade> Paste(Student student, Subject subject, GradeLevel gradeLevel, DateTime? timestamp = null, string reason = null)
            => Paste(student.UserId, subject.Id, gradeLevel, timestamp: timestamp, reason: reason);

        /// <summary>
        ///     Множественное выставление оценок
        /// </summary>
        /// <param name="studentIds">Идентификаторы студентов</param>
        /// <param name="subjectId">Идентификаторы предметов</param>
        /// <param name="grade">Уровень оценки</param>
        /// <param name="reason">Причина выставления</param>
        /// <exception cref="ArgumentException" />
        /// <exception cref="ArgumentNullException" />
        public Task<Grade[]> PasteMultiple(IEnumerable<int> studentIds, int subjectId, GradeLevel grade, DateTime? timestamp = null, string reason = null)
        {
            if (studentIds == null)
                throw new ArgumentNullException(nameof(studentIds));
            JArray idsArray = JArray.FromObject(studentIds);
            if (idsArray.Count == 0)
                throw new ArgumentException("Необходимо указать идентификаторы студентов", nameof(studentIds));
            if (!((GradeLevel[])Enum.GetValues(typeof(GradeLevel))).Contains(grade))
                throw new ArgumentException("Неверное значение", nameof(grade));

            Dictionary<string, string> getArgs = new Dictionary<string, string>()
            {
                ["subjectId"] = subjectId.ToString(),
                ["gradeLevel"] = ((int)grade).ToString(),
            };

            if (timestamp.HasValue)
                getArgs["timestamp"] = timestamp.ToString();
            
            if (!string.IsNullOrWhiteSpace(reason))
                getArgs["reason"] = reason;

            return QueryExecuter.ExecutePostQuery<Grade[]>(CONTROLLER_NAME, "PasteMultiple", idsArray, getArgs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="students"></param>
        /// <param name="subject"></param>
        /// <param name="grade"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public Task<Grade[]> PasteMultiple(IEnumerable<Student> students, Subject subject, GradeLevel grade, DateTime? timestamp = null, string reason = null)
            => PasteMultiple(students.Select(s => s.UserId), subject.Id, grade, timestamp: timestamp, reason: reason);

        public async Task<Grade[]> GetGradesInMonthAsync(DateTime date,  int groupId, int subjectId)
        {
            JObject response = await QueryExecuter.ExecuteGetQuery<JObject>(CONTROLLER_NAME, "GetMonthGrades", new Dictionary<string, string>()
            {
                ["year"] = date.Year.ToString(),
                ["month"] = date.Month.ToString(),
                ["groupId"] = groupId.ToString(),
                ["subjectId"] = subjectId.ToString()
            });

            Grade[] grades = response["grades"].ToObject<Grade[]>();
            User[] ratedByUsers = response["ratedByUsers"].ToObject<User[]>();

            foreach (Grade grade in grades)
                grade.RatedBy = ratedByUsers.FirstOrDefault(u => u.Id == grade.RatedById);

            /*return QueryExecuter.ExecuteGetQuery<Grade[]>(CONTROLLER_NAME, "GetMonthGrades", new Dictionary<string, string>() 
            {
                ["year"] = date.Year.ToString(), 
                ["month"] = date.Month.ToString(),
                ["groupId"] = groupId.ToString(),
                ["subjectId"] = subjectId.ToString()
            });*/

            return grades;
        }

        public Task<Grade[]> GetGradesInMonthAsync(DateTime date, StudentGroup group, Subject subject)
            => GetGradesInMonthAsync(date, group.Id, subject.Id);


        private const string CONTROLLER_NAME = "Grades";
    }
}
