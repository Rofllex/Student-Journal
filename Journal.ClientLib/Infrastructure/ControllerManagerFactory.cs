using System;
using System.Diagnostics;
using System.Reflection;

#nullable enable

namespace Journal.ClientLib.Infrastructure
{
    /// <inheritdoc cref="IControllerManagerFactory"/>
    public class ControllerManagerFactory : IControllerManagerFactory
    {
        public TManager Create<TManager>(IJournalClient client) where TManager : IControllerManager
            => Create<TManager>(client, () => 
            {
                object? instance = Activator.CreateInstance(typeof(TManager), true);
                if (instance != null)
                    return (TManager)instance;
                else
                    throw new Exception($"Не удалось создать экземлпяр тип \"{ typeof(TManager).FullName }\"");
            });

        public TManager Create<TManager>(IJournalClient client, Func<TManager> instanceFactory) where TManager : IControllerManager
        {
            if (!CanCreate<TManager>(client))
                throw new Exception($"Не удалось создать экземпляр класса \"{ typeof(TManager).FullName }\" т.к. экземпляр клиента не соответствует наложенным ограничениям.");

            TManager instance = instanceFactory.Invoke();
            ControllerManagerBase? managerBase = instance as ControllerManagerBase;
            if (managerBase != null)
            {
                managerBase.QueryExecuter = client.QueryExecuter;
                return instance;
            }
            else
            {
                Type type = typeof(TManager);
                PropertyInfo? queryExecuterProperty = type.GetProperty(nameof(IControllerManager.QueryExecuter));
                Debug.Assert(queryExecuterProperty != null);
                MethodInfo? setAccessor = queryExecuterProperty.GetSetMethod();
                if (setAccessor != null)
                {
                    setAccessor.Invoke(instance, new object[] { client.QueryExecuter });
                    return instance;
                }
                else
                    throw new Exception($"Invalid type \"{ type.FullName }\" cant get \"set\" accessor");
            }
        }

        public bool CanCreate<TManager>(IJournalClient client) where TManager : IControllerManager
        {
            Type type = typeof(TManager);
            foreach (Attribute attribute in type.GetCustomAttributes())
            {
                IControllerManagerRestriction? restriction = attribute as IControllerManagerRestriction;
                if (restriction != null && !restriction.Check(client))
                    return false;
            }
            return true;
        }
    }
}
