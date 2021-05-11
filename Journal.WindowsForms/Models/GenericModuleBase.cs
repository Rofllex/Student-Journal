
using System;
using System.ComponentModel;

namespace Journal.WindowsForms.Models
{
    public abstract class GenericModuleBase<T> where T : class
    {
        public static explicit operator T(GenericModuleBase<T> model)
            => model.Original;

        public GenericModuleBase(T original)
        {
            Original = original ?? throw new ArgumentNullException();
        }

        public GenericModuleBase() { }

        [Browsable(false)]
        public virtual T Original { get; protected set; }
    }
}
