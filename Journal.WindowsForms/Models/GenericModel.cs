
using System;
using System.ComponentModel;

namespace Journal.WindowsForms.Models
{
    public abstract class GenericModel<T>
    {
        public static explicit operator T(GenericModel<T> model)
            => model.Original;

        public GenericModel(T original)
        {
            Original = original ?? throw new ArgumentNullException();
        }

        public GenericModel() { }

        [Browsable(false)]
        public virtual T Original { get; protected set; }
    }
}
