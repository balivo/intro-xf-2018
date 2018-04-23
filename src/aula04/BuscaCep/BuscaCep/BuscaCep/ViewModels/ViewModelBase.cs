using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BuscaCep.ViewModels
{
    class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        //{
        //    if (PropertyChanged != null)
        //        PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        //protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private bool _IsBusy = false;
        public bool IsBusy
        {
            get => _IsBusy;
            set
            {
                _IsBusy = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsNotBusy));
            }
        }

        public bool IsNotBusy { get => !_IsBusy; }

        #region [ Navegação ]

        protected Task PushAsync(Page page, bool animated = true) => App.Current.MainPage.Navigation.PushAsync(page, animated);

        protected Task PopAsync(bool animated = true) => App.Current.MainPage.Navigation.PopAsync(animated);

        #endregion
    }
}
