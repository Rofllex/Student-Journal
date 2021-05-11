using Journal.Common.Entities;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Journal.ClientLib.Entities
{
    /// <summary>
    /// Реализация модели группы студентов.
    /// </summary>
    /// <inheritdoc cref="IStudentGroup"/>
    public class StudentGroup : IStudentGroup
    {
        [JsonConstructor]
        private StudentGroup() { }

        [JsonProperty("id")
            , JsonRequired]
        public int Id { get; private set; }

        [JsonProperty("curatorId")
            , JsonRequired]
        public int CuratorId { get; private set; }

        [JsonProperty("curatorEnt")]
        public User CuratorEnt { get; private set; }

        [JsonProperty("specialtyId")
            , JsonRequired]
        public int SpecialtyId  { get; private set; }

        [JsonProperty("specialtyEnt")]
        public Specialty SpecialtyEnt  { get; private set; }

        [JsonProperty("currentCourse")
            , JsonRequired]
        public int CurrentCourse  { get; private set; }

        [JsonProperty("subgroup")]
        public int Subgroup  { get; private set; }

        [JsonProperty("students")]
        public List<Student> Students  { get; private set; }

        [JsonProperty("graduatedDate")]
        public DateTime? GraduatedDate  { get; private set; }

        public IReadOnlyList<IStudent> GetStudentsList()
            => Students;

        [JsonIgnore]
        ISpecialty IStudentGroup.Specialty => SpecialtyEnt; 

        [JsonIgnore]
        IUser? IStudentGroup.Curator => CuratorEnt;
    }
}
