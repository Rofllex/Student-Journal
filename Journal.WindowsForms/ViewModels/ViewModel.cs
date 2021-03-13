using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

#nullable enable

namespace Journal.WindowsForms.ViewModels
{
    /// <summary>
    ///     Базовый класс модель-представление. Данный класс наследуется от <see cref="INotifyPropertyChanged"/>
    /// </summary>
    public abstract class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (_,__)=> { };
    
        protected void InvokePropertyChanged([CallerMemberName] string memberName = "" )
            => PropertyChanged( this, new PropertyChangedEventArgs( memberName ) );

        protected void ChangeProperty<T>(ref T field, T newValue, [CallerMemberName] string memberName = "")
        {
            // Метод сравнения двух объектов
            Func<object?, bool> equalsFunc;
            // Объект, который необходимо подставить в equalsFunc.
            T objectToCompare;

            if (field != null)
            {
                equalsFunc = field.Equals;
                objectToCompare = newValue;
            }
            else if (newValue != null)
            {
                equalsFunc = newValue.Equals;
                objectToCompare = field;
            }
            else
            {
                // Предыдущее и новое значение null.
                return;
            }

            if (!equalsFunc(objectToCompare))
            {
                field = newValue;
                InvokePropertyChanged(memberName);
            }
        }
    }
}
