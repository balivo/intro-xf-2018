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
        }

        private async void BtnBuscarCep_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtCep.Text))
                    throw new InvalidOperationException("Informe o CEP");

                using (var client = new HttpClient())
                {
                    using (var response = await client.GetAsync($"http://viacep.com.br/ws/{txtCep.Text}/json/"))
                    {
                        if (!response.IsSuccessStatusCode)
                            throw new InvalidOperationException("Algo de errado não de deu certo ao consultar o CEP");

                        var result = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrWhiteSpace(result))
                            await DisplayAlert("Eita", result, "Ok");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ah não!", ex.Message, "Ok");
            }
        }
    }
}
