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

        public DateTime Timestamp { get; private set; }

        
        [JsonRequired]
        public int RatedById { get; private set; }
        public User RatedBy { get; private set; }


        [JsonRequired]
        public int SubjectId { get; private set; }
        
        public Subject Subject { get; private set; }
        public string Reason { get; private set; }

        [JsonRequired]
        public int StudentId { get; private set; }
        public Student Student { get; private set; }



        public GradeLevel GradeLevel { get; private set; }

        IUser IGrade.RatedBy => RatedBy;
        ISubject IGrade.Subject => Subject;
        IStudent IGrade.Student => Student;
    }
}
