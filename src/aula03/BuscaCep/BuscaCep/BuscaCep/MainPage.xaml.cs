using BuscaCep.Clients;
using BuscaCep.ViewModels;
using System;
using System.Net.Http;
using Xamarin.Forms;

namespace BuscaCep
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            BindingContext = new BuscaCepViewModel();
        }

        private async void BtnBuscarCep_Clicked(object sender, EventArgs e)
        //=> ((BuscaCepViewModel)BindingContext).BuscarCommand.Execute(null);
        {
            try
            {
                var result = await ViaCepHttpClient.Current.BuscarCep(((BuscaCepViewModel)BindingContext).CEP);

                if (!string.IsNullOrWhiteSpace(result.cep))
                    await DisplayAlert("Eita", result.cep, "Ok");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ah não", ex.Message, "Ok");
            }
        }
    }
}
