using Journal.Common.Entities;

using Newtonsoft.Json;

namespace Journal.ClientLib.Entities
{
    /// <summary>Модель предмета</summary>
    /// <inheritdoc cref="ISubject"/>
    public class Subject : ISubject
    {
        [JsonConstructor]
        private Subject() { }

        [JsonProperty("id")
            , JsonRequired]
        public int Id { get; private set; }

        [JsonProperty("name")
            , JsonRequired]
        public string Name { get; private set; }
    }
}
