﻿using Newtonsoft.Json;

namespace KIRTStudentJournal.Shared.Models
{
    public class AccountModel
    {
        /// <summary>
        /// Идентификатор аккаунта
        /// </summary>
        [JsonProperty("id")]
        public uint Id { get; set; }

        /// <summary>
        /// Роль
        /// </summary>
        [JsonProperty("role")]
        public RoleModel Role { get; set; }


        [JsonConstructor]
        public AccountModel()
        {
        }

        public AccountModel(uint id, RoleModel role)
        {
            Id = id;
            Role = role;
        }
    }
}
