using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TicTacToe.Models;

namespace TicTacToe.ViewModel
{
    public partial class TicTacToeGamePageViewModel : ObservableObject
    {
        [ObservableProperty]
        private int _player1Point;

        [ObservableProperty]
        private int _player2Point;

        [ObservableProperty]
        private string _playerWinOrDrawText;


        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Player1BackgroundColor))]
        [NotifyPropertyChangedFor(nameof(Player2BackgroundColor))]
        private int _playerTurn = 0;

        public Color Player1BackgroundColor => PlayerTurn == 0 ? Colors.LightBlue : Colors.White;
        public Color Player2BackgroundColor => PlayerTurn == 1 ? Colors.LightBlue : Colors.White;


        private bool _isAnyoneWin;
        private bool _isPlayerOneStarting = true;
        private List<int> MoveHistory = new List<int>();
        List<int[]> WinPossibilities = new List<int[]>();
        List<int[]> FourInARowPossibilities = new List<int[]>();
        [ObservableProperty]
        private bool _isVictoryButton1Enabled = false;
        [ObservableProperty]
        private bool _isVictoryButton2Enabled = false;
        private bool _isVictoryDeclared = false;

        public ObservableCollection<TicTacToeModel> TicTacList { get; set; } = new ObservableCollection<TicTacToeModel>();

        public TicTacToeGamePageViewModel()
        {
            SetupGameInfo();

        }

        public void SetupGameInfo()
        {
            WinPossibilities.Clear();
            FourInARowPossibilities.Clear();
            MoveHistory.Clear();

            for (int i = 0; i < 25; i += 5)
            {
                for (int j = 0; j < 2; j++)
                {
                    FourInARowPossibilities.Add(new int[] { i + j, i + j + 1, i + j + 2, i + j + 3 });
                }
            }

            // Vertikalne mogućnosti pobede za 4 u nizu
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 10; j += 5)
                {
                    FourInARowPossibilities.Add(new int[] { i + j, i + j + 5, i + j + 10, i + j + 15 });
                }
            }

            // Dijagonalne mogućnosti pobede za 4 u nizu
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    // Dijagonale od gornjeg levog do donjeg desnog
                    FourInARowPossibilities.Add(new int[] { i * 5 + j, (i + 1) * 5 + (j + 1), (i + 2) * 5 + (j + 2), (i + 3) * 5 + (j + 3) });
                    // Dijagonale od gornjeg desnog do donjeg levog
                    FourInARowPossibilities.Add(new int[] { i * 5 + (j + 3), (i + 1) * 5 + (j + 2), (i + 2) * 5 + (j + 1), (i + 3) * 5 + j });
                }
            }

            TicTacList.Clear();
            for (int i = 0; i < 25; i++)
            {
                TicTacList.Add(new TicTacToeModel(i));
            }
        }

        [RelayCommand]
        public void RestartGame()
        {
            _isAnyoneWin = false;
            PlayerWinOrDrawText = "";

            if (_isPlayerOneStarting)
            {
                PlayerTurn = 0; // Igrač 1 počinje
            }
            else
            {
                PlayerTurn = 1; // Igrač 2 počinje
            }
            _isPlayerOneStarting = !_isPlayerOneStarting;

            SetupGameInfo();
        }

        [RelayCommand]
        public void SelectedItem(TicTacToeModel selectedItem)
        {
            if (!string.IsNullOrEmpty(selectedItem.SelectedText) || _isAnyoneWin) return;

            if (PlayerTurn == 0)
            {
                selectedItem.SelectedText = "X";
            }
            else
            {
                selectedItem.SelectedText = "O";
            }
            selectedItem.Player = _playerTurn;
            MoveHistory.Add(selectedItem.Index);

            PlayerTurn = PlayerTurn == 0 ? 1 : 0;

            CheckForWin();
        }

        public void CheckForWin()
        {

            var player1IndexList = TicTacList.Where(f => f.Player == 0).Select(f => f.Index).ToList();
            var player2IndexList = TicTacList.Where(f => f.Player == 1).Select(f => f.Index).ToList();



            foreach (var fourInARowPossibility in FourInARowPossibilities)
            {
                int player1Count = 0;
                int player2Count = 0;

                foreach (var index in fourInARowPossibility)
                {
                    if (player1IndexList.Contains(index)) player1Count++;
                    if (player2IndexList.Contains(index)) player2Count++;
                }

                if (player1Count == 4)
                {
                    Player1Point++;
                    PlayerWinOrDrawText = "Player 1 Wins with 4 in a row";
                    _isVictoryDeclared = true;
                    _isAnyoneWin = true;

                }
                if (player2Count == 4)
                {
                    Player2Point++;
                    PlayerWinOrDrawText = "Player 2 Wins with 4 in a row";
                    _isVictoryDeclared = true;
                    _isAnyoneWin = true;

                }
            }

            // Proveri da li su sve pozicije popunjene
            if (TicTacList.Count(f => f.Player.HasValue) == 25 && !_isAnyoneWin)
            {

                PlayerWinOrDrawText = "Draw";

            }
        }

    }
}
