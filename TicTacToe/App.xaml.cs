using TicTacToe.Views;

namespace TicTacToe
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            App.Current.MainPage = new TicTacToeGamePage();
        }
    }
}
