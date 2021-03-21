using System;

#nullable enable

namespace Journal.ClientLib.Infrastructure
{
    /// <summary>
    ///     Фабрика создания менеджеров контроллеров
    /// </summary>
    public interface IControllerManagerFactory
    {
        /// <summary>
        ///     Создать менеджер контроллера.
        /// </summary>
        /// <typeparam name="TManager">
        ///     Тип менеджера.
        /// </typeparam>
        /// <param name="client">
        ///     Клиент журнала.
        /// </param>
        /// <returns>
        ///     Экземпляр класса менеджера.
        /// </returns>
        TManager Create<TManager>(IJournalClient client) where TManager : IControllerManager;
        
        /// <summary>
        ///     Создать менеджер контроллера
        /// </summary>
        /// <typeparam name="TManager">
        ///     Тип менеджера.
        /// </typeparam>
        /// <param name="client">
        ///     Клиент
        /// </param>
        /// <param name="instanceFactory">
        ///     Фабричная функция создания менеджера.
        /// </param>
        /// <returns>
        ///     Экземпляр класса менеджера.
        /// </returns>
        TManager Create<TManager>(IJournalClient client, Func<TManager> instanceFactory) where TManager : IControllerManager;

        bool CanCreate<TManager>(IJournalClient client) where TManager : IControllerManager;
    }
}
