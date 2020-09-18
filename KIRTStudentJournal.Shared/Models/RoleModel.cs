using Newtonsoft.Json;
using System;

namespace KIRTStudentJournal.Shared.Models
{
    public class RoleModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("id")]
        public int Id { get; set; }

        private static readonly Type roleType = typeof(Role);
        public RoleModel(Role role)
        {
            Name = Enum.GetName(roleType, role);
            Id = (int)role;
        }

        public RoleModel()
        {
        }


        public static implicit operator Role(RoleModel model) => (Role)model.Id;
    }
}
