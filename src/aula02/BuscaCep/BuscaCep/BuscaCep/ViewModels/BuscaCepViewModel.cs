using BuscaCep.Clients;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BuscaCep.ViewModels
{
    class BuscaCepViewModel : ViewModelBase
    {
        public BuscaCepViewModel() : base()
        {
            // Também é válido...
            //_BuscarCommand = new Command(async () => await BuscarCommandExecute());
        }

        private string _CEPBusca;
        public string CEPBusca
        {
            get => _CEPBusca;
            set
            {
                _CEPBusca = value;
                OnPropertyChanged();
            }
        }

        private string _CEP;
        public string CEP
        {
            get => _CEP;
            set
            {
                _CEP = value;
                OnPropertyChanged();
            }
        }

        private string _Logradouro;
        public string Logradouro
        {
            get => _Logradouro;
            set
            {
                _Logradouro = value;
                OnPropertyChanged();
            }
        }

        private string _Bairro;
        public string Bairro
        {
            get => _Bairro;
            set
            {
                _Bairro = value;
                OnPropertyChanged();
            }
        }

        private string _Localidade;
        public string Localidade
        {
            get => _Localidade;
            set
            {
                _Localidade = value;
                OnPropertyChanged();
            }
        }

        private string _UF;
        public string UF
        {
            get => _UF;
            set
            {
                _UF = value;
                OnPropertyChanged();
            }
        }

        public bool HasCep { get => !string.IsNullOrWhiteSpace(_CEP); }

        private Command _BuscarCommand;
        //public Command BuscarCommand
        //{
        //    get
        //    {
        //        if (_BuscarCommand == null)
        //            _BuscarCommand = new Command(async () => await BuscarCommandExecute());

        //        return _BuscarCommand;
        //    }
        //}
        public Command BuscarCommand => _BuscarCommand ?? (_BuscarCommand = new Command(async () => await BuscarCommandExecute(), () => IsNotBusy));

        async Task BuscarCommandExecute()
        {
            try
            {
                if (IsBusy)
                    return;

                IsBusy = true;
                BuscarCommand.ChangeCanExecute();

                var result = await ViaCepHttpClient.Current.BuscarCep(_CEPBusca);

                CEP = result.cep;

                if (HasCep)
                {
                    Logradouro = result.logradouro;
                    Bairro = result.bairro;
                    Localidade = result.localidade;
                    UF = result.uf;
                }

                OnPropertyChanged(nameof(HasCep));
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Ah não", ex.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
                BuscarCommand.ChangeCanExecute();
            }
        }
    }
}