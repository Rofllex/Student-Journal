namespace Journal.ClientLib.Infrastructure
{
    /// <summary>
    ///     Базовый класс менеджера контроллера.
    /// </summary>
    public interface IControllerManager
    {
        /// <summary>
        ///     Класс менеджера выполнения запросов.
        ///     У данного члена должен быть set акцессор.
        /// </summary>
        IClientQueryExecuter QueryExecuter { get; }
    }
}
