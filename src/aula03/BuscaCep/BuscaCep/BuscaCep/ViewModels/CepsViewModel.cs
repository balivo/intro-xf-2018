using BuscaCep.Pages;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace BuscaCep.ViewModels
{
    sealed class CepsViewModel : ViewModelBase
    {
        public CepsViewModel() : base()
        {
            MessagingCenter.Subscribe<BuscaCepViewModel>(this, "ADICIONAR_CEP", (sender) =>
            {
                if (!Ceps.Any(lbda => lbda.Equals(sender.CEP)))
                    Ceps.Add(sender.CEP);
            });
        }

        public ObservableCollection<string> Ceps { get; private set; } = new ObservableCollection<string>();

        private Command _BuscarCommand;
        public Command BuscarCommand => _BuscarCommand ?? (_BuscarCommand = new Command(async () => await PushAsync(new BuscaCepPage())));
    }
}
