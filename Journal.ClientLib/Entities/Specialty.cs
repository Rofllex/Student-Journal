using Journal.Common.Entities;

using Newtonsoft.Json;

using System.Collections.Generic;

namespace Journal.ClientLib.Entities
{
    /// <summary>
    ///     Реализация модели специальности.
    /// </summary>
    /// <inheritdoc cref="ISpecialty"/>
    public class Specialty : ISpecialty
    {
        [JsonConstructor]
        private Specialty() { }

        public Specialty(int id, string name, string code, int maxCourse)
        {
            this.Id = id;
            Name = name;
            Code = code;
            MaxCourse = maxCourse;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public int MaxCourse { get; set; }

        public List<Subject> Subjects { get; set; } = new List<Subject>();

        [JsonIgnore]
        IReadOnlyList<ISubject> ISpecialty.Subjects => Subjects;
    }
}
