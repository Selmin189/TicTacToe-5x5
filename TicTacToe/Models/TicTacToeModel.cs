using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Models
{
    public partial class TicTacToeModel : ObservableObject
    {
        public TicTacToeModel(int index)
        {
            this.Index = index;
        }

        public int Index { get; set; }
        [ObservableProperty]
        private string _selectedText;
        
        public int? Player {  get; set; }

        [ObservableProperty]
        private bool _isWinningCell;

        public Color CellBackgroundColor => IsWinningCell ? Colors.LightGreen : Colors.White;
    }
}
