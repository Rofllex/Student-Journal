using Journal.Common.Entities;

using Newtonsoft.Json;

namespace Journal.ClientLib.Entities
{
    public class Curriculum : ICurriculum
    {
        public int Id { get; private set; }

        public int SpecialtyId { get; private set; }

        public Specialty Specialty { get; private set; }
        
        public int SubjectId { get; private set; }

        public Subject Subject { get; private set; }

        [JsonIgnore]
        ISpecialty ICurriculum.Specialty => Specialty;
        
        [JsonIgnore]
        ISubject ICurriculum.Subject => Subject;
    }
}
