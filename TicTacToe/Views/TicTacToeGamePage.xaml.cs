using TicTacToe.ViewModel;

namespace TicTacToe.Views;

public partial class TicTacToeGamePage : ContentPage
{
	public TicTacToeGamePage()
	{
		InitializeComponent();
		this.BindingContext = new TicTacToeGamePageViewModel();
	}
}