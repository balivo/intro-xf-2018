using BuscaCep.Clients;
using BuscaCep.Data;
using BuscaCep.Data.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BuscaCep.ViewModels
{
    class BuscaCepViewModel : ViewModelBase
    {
        private CepDto _Cep = null;

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

        public string CEP { get => _Cep?.Cep; }

        public string Logradouro { get => _Cep?.Logradouro; }

        public string Bairro { get => _Cep?.Bairro; }

        public string Localidade { get => _Cep?.Localidade; }

        public string UF { get => _Cep?.UF; }

        public bool HasCep { get => _Cep != null; }

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

                _Cep = new CepDto
                {
                    Bairro = result.bairro,
                    Cep = result.cep,
                    Complemento = result.complemento,
                    GIA = result.gia,
                    IBGE = result.ibge,
                    Id = Guid.NewGuid(),
                    Localidade = result.localidade,
                    Logradouro = result.logradouro,
                    UF = result.uf,
                    Unidade = result.unidade,
                };

                OnPropertyChanged(nameof(CEP));
                OnPropertyChanged(nameof(Logradouro));
                OnPropertyChanged(nameof(Bairro));
                OnPropertyChanged(nameof(Localidade));
                OnPropertyChanged(nameof(UF));
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
                AdicionarCommand.ChangeCanExecute();
            }
        }

        private Command _AdicionarCommand;
        public Command AdicionarCommand => _AdicionarCommand ?? (_AdicionarCommand = new Command(async () => await AdicionarCommandExecute(), () => IsNotBusy));

        async Task AdicionarCommandExecute()
        {
            try
            {
                if (IsBusy)
                    return;

                IsBusy = true;
                BuscarCommand.ChangeCanExecute();
                AdicionarCommand.ChangeCanExecute();

                await BuscaCepHttpClient.Current.PostCep(_Cep);

                // Salva o CEP pesquisado no banco de dados local (Aula 3 - Parte 2)
                //DatabaseService.Current.CepSave(_Cep);

                //Avisar a tela de lista de Ceps pesquisados que um novo CEP deve ser adicionado...
                MessagingCenter.Send(this, MessageKeys.CepsAtualizados);

                await PopAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                IsBusy = false;
                BuscarCommand.ChangeCanExecute();
                AdicionarCommand.ChangeCanExecute();
            }
        }
    }
}