using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Laboratory5_TaskManager.ViewsModel
{

    internal abstract class BaseViewModel : INotifyPropertyChanged
    {
        public abstract void Refresher();

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
