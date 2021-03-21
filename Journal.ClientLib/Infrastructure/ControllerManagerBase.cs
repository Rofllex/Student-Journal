namespace Journal.ClientLib.Infrastructure
{
    /// <inheritdoc cref="IControllerManager"/>
    public abstract class ControllerManagerBase : IControllerManager
    {
        public IClientQueryExecuter QueryExecuter { get; internal set; }

        protected ControllerManagerBase() { }

        protected ControllerManagerBase(IClientQueryExecuter queryExecuter)
            => SetExecuter(queryExecuter ?? throw new System.ArgumentNullException(nameof(queryExecuter)));

        /// <summary>
        ///     Запись в <see cref="QueryExecuter"/>
        /// </summary>
        protected void SetExecuter(IClientQueryExecuter executer)
            => QueryExecuter = executer;
    }
}
