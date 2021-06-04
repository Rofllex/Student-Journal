using System;

using Newtonsoft.Json;

using Journal.Common.Entities;


namespace Journal.ClientLib.Entities
{
    public class Grade : IGrade
    {
        [JsonConstructor]
        private Grade() { }

        public int Id { get; private set; }

        [JsonProperty("timestamp")
            , JsonRequired]
        public DateTime Timestamp { get; private set; }

        
        [JsonRequired]
        public int RatedById { get; private set; }
        public User RatedBy { get; set; }


        [JsonRequired]
        public int SubjectId { get; private set; }
        
        public Subject Subject { get; private set; }

        [JsonProperty("reason")]
        public string Reason { get; private set; }

        [JsonRequired]
        public int StudentId { get; private set; }
        public Student Student { get; private set; }

        [JsonProperty("gradeLevel"),
            JsonRequired]
        public GradeLevel GradeLevel { get; private set; }

        IUser IGrade.RatedBy => RatedBy;
        ISubject IGrade.Subject => Subject;
        IStudent IGrade.Student => Student;

    }
}
