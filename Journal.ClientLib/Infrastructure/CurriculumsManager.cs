using Journal.ClientLib.Entities;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Journal.ClientLib.Infrastructure
{
    public class CurriculumsManager : ControllerManagerBase
    {
        public CurriculumsManager(IJournalClient client) : base(client.QueryExecuter) { }

        private CurriculumsManager() { }

        public Task UploadCurriculumAsync(Stream curriculumFile, int specialtyId, int subjectId) 
        {
            throw new NotImplementedException();
        }

        public Task UploadCurriculumAsync(Stream curriculumFile, Specialty specialty, Subject subject)
            => UploadCurriculumAsync(curriculumFile, specialty.Id, subject.Id);

        public Task<Curriculum[]> GetCurriculumsAsync(int offset, int count)
            => QueryExecuter.ExecuteGetQuery<Curriculum[]>(CONTROLLER_NAME, "GetCurriculums", new Dictionary<string, string>() 
            {
                ["offset"] = offset.ToString(),
                ["count"] = count.ToString()
            });

        public Task RemoveCurriculumAsync(int curriculumId)
            => QueryExecuter.ExecuteGetQuery(CONTROLLER_NAME, "RemoveCurriculum", new Dictionary<string, string>
            {
                ["curriculumId"] = curriculumId.ToString()
            });


        private const string CONTROLLER_NAME = "Subjects";
    }
}
