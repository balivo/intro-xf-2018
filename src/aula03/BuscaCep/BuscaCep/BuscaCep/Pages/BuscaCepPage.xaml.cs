using BuscaCep.ViewModels;

using Xamarin.Forms;

namespace BuscaCep.Pages
{
    public partial class BuscaCepPage : ContentPage
    {
        public BuscaCepPage()
        {
            InitializeComponent();

            BindingContext = new BuscaCepViewModel();
        }
    }
}