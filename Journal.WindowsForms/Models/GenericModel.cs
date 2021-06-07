
using System;
using System.ComponentModel;

namespace Journal.WindowsForms.Models
{
    /// <summary>
    ///     Базовый класс модели.
    ///     Реализует интерфейс <see cref="IModel"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class GenericModel<T> : IModel
    {
        public static explicit operator T(GenericModel<T> model)
            => model.Original;

        public GenericModel(T original)
        {
            Original = original ?? throw new ArgumentNullException(nameof(original));
        }

        public GenericModel() { }

        [Browsable(false)]
        public virtual T Original { get; protected set; }

        object IModel.Original => Original;
    }
}
