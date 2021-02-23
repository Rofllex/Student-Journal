using System.ComponentModel;
using System.Runtime.CompilerServices;

#nullable enable

namespace Journal.WindowsForms.ViewModels
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (_,__)=> { };
    
        protected void InvokePropertyChanged([CallerMemberName] string memberName = "" )
        {
            PropertyChanged( this, new PropertyChangedEventArgs( memberName ) );
        }
    }
}
