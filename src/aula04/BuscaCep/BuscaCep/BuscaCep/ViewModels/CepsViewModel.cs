using BuscaCep.Clients;
using BuscaCep.Data;
using BuscaCep.Data.Dtos;
using BuscaCep.Pages;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BuscaCep.ViewModels
{
    sealed class CepsViewModel : ViewModelBase
    {
        const string SEU_USUARIO = "SEU_USUARIO";
        const string SUA_SENHA = "SUA_SENHA";

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
                if (BuscaCepHttpClient.Current.IsAuthorized)
                {
                    MessagingCenter.Subscribe<BuscaCepViewModel>(this, MessageKeys.CepsAtualizados, (sender) =>
                    {
                        this.RefreshCommand.Execute(null);

                        MessagingCenter.Unsubscribe<BuscaCepViewModel>(this, MessageKeys.CepsAtualizados);
                    });

                    await PushAsync(new BuscaCepPage());
                }
                else if (await App.Current.MainPage.DisplayAlert("Ooops", "Você não está autenticado, deseja autenticar-se", "Sim", "Não"))
                    await BuscaCepHttpClient.Current.Autenticar(SEU_USUARIO, SUA_SENHA);
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
                if (IsBusy)
                    return;

                IsBusy = true;
                RefreshCommand.ChangeCanExecute();

                Ceps.Clear();

                // Recupera informações do banco de dados remoto API (Aula 4)
                foreach (var item in await BuscaCepHttpClient.Current.GetCeps())
                {
                    Ceps.Add(item);
                }

                // Recupera informações do banco de dados local (SQLite - Aula 3 parte 2)
                //foreach (var item in DatabaseService.Current.CepGetAll())
                //{
                //    Ceps.Add(item);
                //}
            }
            catch (UnauthorizedAccessException ex)
            {
                if (await App.Current.MainPage.DisplayAlert("Ooops", ex.Message, "Sim", "Não"))
                {
                    await BuscaCepHttpClient.Current.Autenticar(SEU_USUARIO, SUA_SENHA);

                    IsBusy = false;
                    RefreshCommand.ChangeCanExecute();

                    await RefreshCommandExecute();
                }
            }
            catch (InvalidOperationException ex)
            {
                await App.Current.MainPage.DisplayAlert("Ooops", ex.Message, "Ok");
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Ah não!", ex.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
                RefreshCommand.ChangeCanExecute();
            }
        }


        private Command _LocationCommand;
        public Command LocationCommand => _LocationCommand ?? (_LocationCommand = new Command(async () => await LocationCommandExecute()));

        async Task LocationCommandExecute()
        {
            try
            {
                var currentPosition = await Plugin.Geolocator.CrossGeolocator.Current.GetPositionAsync();

                if (currentPosition != null)
                    await App.Current.MainPage.DisplayAlert("Local", currentPosition.Latitude.ToString(), "Ok");
                else
                    await App.Current.MainPage.DisplayAlert("Ooops", "Coordenadas desconhecidas", "Ok");
            }
            catch (Exception ex)
            {
                throw;
            }
        }





        private Command _CameraCommand;
        public Command CameraCommand => _CameraCommand ?? (_CameraCommand = new Command(async () => await CameraCommandExecute()));

        async Task CameraCommandExecute()
        {
            try
            {
                var mediaFile = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions()
                {
                });

                if (mediaFile != null)
                    await App.Current.MainPage.DisplayAlert("Sucesso", "Imagem capturada", "Ok");
                else
                    await App.Current.MainPage.DisplayAlert("Ooops", "Coordenadas desconhecidas", "Ok");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
