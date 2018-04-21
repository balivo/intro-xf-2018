using BuscaCep.Data;
using BuscaCep.Data.Dtos;
using BuscaCep.Pages;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BuscaCep.ViewModels
{
    sealed class CepsViewModel : ViewModelBase
    {
        public CepsViewModel() : base()
        {

        }

        public ObservableCollection<CepDto> Ceps { get; private set; } = new ObservableCollection<CepDto>();

        private Command _BuscarCommand;
        public Command BuscarCommand => _BuscarCommand ?? (_BuscarCommand = new Command(async () => await BuscarCommandExecute()));

        async Task BuscarCommandExecute()
        {
            try
            {
                MessagingCenter.Subscribe<BuscaCepViewModel>(this, MessageKeys.CepsAtualizados, (sender) =>
                {
                    this.RefreshCommand.Execute(null);

                    MessagingCenter.Unsubscribe<BuscaCepViewModel>(this, MessageKeys.CepsAtualizados);
                });

                await PushAsync(new BuscaCepPage());
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        
        private Command _RefreshCommand;
        public Command RefreshCommand => _RefreshCommand ?? (_RefreshCommand = new Command(async () => await RefreshCommandExecute(), () => IsNotBusy));

        async Task RefreshCommandExecute()
        {
            try
            {
                await Task.FromResult<object>(null);

                if (IsBusy)
                    return;

                IsBusy = true;
                RefreshCommand.ChangeCanExecute();

                Ceps.Clear();

                foreach (var item in DatabaseService.Current.CepGetAll())
                {
                    Ceps.Add(item);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}
