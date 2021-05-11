using System;
using System.Collections.Generic;
using Journal.ClientLib.Entities;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Journal.Common.Entities;
using System.Linq;

namespace Journal.ClientLib.Infrastructure
{
    public class DatabaseManager : ControllerManagerBase
    {
        public DatabaseManager(IJournalClient client)
        {
            base.SetExecuter(client.QueryExecuter ?? throw new ArgumentNullException(nameof(client)));
        }

        public async Task<Specialty> CreateSpecialtyAsync(string name, string code, int maxCourse, int[] subjectIds)
        {
            if (maxCourse <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxCourse));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentNullException(nameof(code));

            Specialty specialty = await QueryExecuter.ExecutePostQuery<Specialty>(CONTROLLER_NAME, "CreateSpecialty", subjectIds, getArgs: new Dictionary<string, string>()
            {
                ["name"] = name,
                ["code"] = code,
                ["maxCourse"] = maxCourse.ToString()
            });
            return specialty;
        }


        public Task<Specialty> CreateSpecialtyAsync(string name, string code, int maxCourse, ISubject[] subjects = null)
            => CreateSpecialtyAsync(name, code, maxCourse, subjects?.Select(s => s.Id).ToArray() ?? Array.Empty<int>());


        /// <summary>
        ///     Асинхронный метод получения специальностей.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<Specialty[]> GetSpecialtiesAsync(int offset, int count)
        {
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            else if (count == 0)
                return Array.Empty<Specialty>();

            JObject response = await QueryExecuter.ExecuteGetQuery<JObject>(CONTROLLER_NAME, "GetSpecialties", new Dictionary<string, string>()
            {
                ["count"] = count.ToString()
                , ["offset"] = offset.ToString()
            });
            return response["specialties"].ToObject<Specialty[]>();
        }

        public Task SetSpecialtySubject(int specialtyId, int subjectId)
            => QueryExecuter.ExecuteGetQuery(CONTROLLER_NAME, "SetSpecialtySubject", new Dictionary<string, string>()
            {
                ["specialtyId"] = specialtyId.ToString(),
                ["subjectId"] = subjectId.ToString()
            });

        public async Task SetSpecialtySubject(Specialty specialty, Subject subject)
        {
            await SetSpecialtySubject(specialty.Id, subject.Id);
            specialty.Subjects.Add(subject);
        }

        public Task RemoveSpecialtySubject(int specialtyId, int subjectId)
            => QueryExecuter.ExecuteGetQuery(CONTROLLER_NAME, "RemoveSpecialtySubject", new Dictionary<string, string>()
            {
                ["specialtyId"] = specialtyId.ToString(),
                ["subjectId"] = subjectId.ToString()
            });

        public async Task RemoveSpecialtySubject(Specialty specialty, Subject subject)
        {
            await RemoveSpecialtySubject(specialty.Id, subject.Id);
            specialty.Subjects.Remove(subject);
        }

        /// <summary>
        ///     Метод получения списка предметов у специальности.
        /// </summary>
        /// <param name="specialtyId">Идентификатор специальности</param>
        /// <returns></returns>
        public async Task<IReadOnlyCollection<Subject>> GetSpecialtySubjects(int specialtyId)
        {
            return await QueryExecuter.ExecuteGetQuery<List<Subject>>(CONTROLLER_NAME, "GetSpecialtySubjects", new Dictionary<string, string>()
            {
                ["specialtyId"] = specialtyId.ToString()
            });
        }

        /// <inheritdoc cref="GetSpecialtySubjects(int)"/>
        public Task<IReadOnlyCollection<Subject>> GetSpecialtySubjects(Specialty specialty)
            => GetSpecialtySubjects(specialty.Id);

        public Task<Specialty> GetSpecialtyById(int specialtyId)
        {
            return QueryExecuter.ExecuteGetQuery<Specialty>(CONTROLLER_NAME, "GetSpecialtyById", new Dictionary<string, string>() 
            {
                ["specialtyId"] = specialtyId.ToString()
            });
        }


        public async Task<Subject[]> GetSubjects(int offset, int count)
        {
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            else if (count == 0)
                return Array.Empty<Subject>();
            JObject response = await QueryExecuter.ExecuteGetQuery<JObject>(CONTROLLER_NAME, "GetSubjects", new Dictionary<string, string>()
            {
                ["offset"] = offset.ToString(),
                ["count"] = count.ToString()
            });
            return response["subjects"].ToObject<Subject[]>();
        }

        public async Task<Subject> GetSubjectById(int subjId)
        {
            if (subjId < 0)
                throw new ArgumentOutOfRangeException(nameof(subjId));
            Subject subj = await QueryExecuter.ExecuteGetQuery<Subject>(CONTROLLER_NAME, "GetSubjectById", new Dictionary<string, string>()
            {
                ["subjectId"] = subjId.ToString()
            });

            return subj;
        }

        public async Task<Subject> CreateSubject(string subjectName, int specialtyId)
        {
            if (string.IsNullOrWhiteSpace(subjectName))
                throw new ArgumentNullException(nameof(subjectName));
            if (specialtyId < 0)
                throw new ArgumentOutOfRangeException(nameof(specialtyId));

            return await QueryExecuter.ExecuteGetQuery<Subject>(CONTROLLER_NAME, "CreateSubject", new Dictionary<string, string>()
            {
                ["name"] = subjectName
                , ["specialtyId"] = specialtyId.ToString()
            });
        }

        public Task<Subject> CreateSubject(string subjectName, ISpecialty specialty)
            => CreateSubject(subjectName, specialty.Id);

        /// <summary>
        ///     Метод получения группы студентов по идентификатору
        /// </summary>
        /// <param name="groupId">Идентификатор группы</param>
        public Task<StudentGroup> GetStudentGroupAsync(int groupId)
            => QueryExecuter.ExecuteGetQuery<StudentGroup>(CONTROLLER_NAME, "GetGroupById", new Dictionary<string, string>()
            {
                ["groupId"] = groupId.ToString()
            });

        /// <summary>
        ///     Метод получения групп с сервера.
        /// </summary>
        /// <param name="offset">Смещение</param>
        /// <param name="count">Кол-во</param>
        /// <returns></returns>
        public async Task<StudentGroup[]> GetGroups(int offset = 0, int count = 30)
        {
            var result = await QueryExecuter.ExecuteGetQuery<JToken>(CONTROLLER_NAME, "GetGroups", new Dictionary<string, string>()
            {
                ["offset"] = offset.ToString(),
                ["count"] = count.ToString()
            });
            return result["groups"].ToObject<StudentGroup[]>();
        }

        public Task<StudentGroup[]> GetGroupsBySpecialty(int specialtyId)
            => QueryExecuter.ExecuteGetQuery<StudentGroup[]>(CONTROLLER_NAME, "GetGroupsBySpecialty", new Dictionary<string, string>()
            {
                ["specialtyId"] = specialtyId.ToString()
            });

        public Task<StudentGroup[]> GetGroupsBySpecialty(Specialty specialty)
            => GetGroupsBySpecialty(specialty.Id);

        public async Task<Student[]> GetStudentsInGroup(int groupId)
        {
            JToken response = await QueryExecuter.ExecuteGetQuery<JToken>(CONTROLLER_NAME, "GetStudentsInGroup", new Dictionary<string, string>() 
            {
                ["groupId"] = groupId.ToString()
            });

            return response["students"].ToObject<Student[]>();
        }

        public Task<Student[]> GetStudentsInGroup(StudentGroup group)
                => GetStudentsInGroup(group.Id);

        private const string CONTROLLER_NAME = "Database";
    }
}
