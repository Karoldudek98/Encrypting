using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Text;

namespace Encrypting.Pages
{
    public class PolybiusModel : PageModel
    {
        [BindProperty]
        public string InputText { get; set; }

        [BindProperty]
        public string Operation { get; set; }

        public string OutputText { get; private set; }

        public char[,] PolybiusGrid { get; set; }

        public void OnPost(string gridConfig)
        {
            var configParts = gridConfig.Split('-');
            if (configParts.Length == 2 && int.TryParse(configParts[0], out int rows) && int.TryParse(configParts[1], out int columns))
            {
                PolybiusGrid = GeneratePolybiusGrid(rows, columns);
                if (Operation == "encrypt")
                {
                    OutputText = EncryptPolybius(InputText);
                }
                else if (Operation == "decrypt")
                {
                    OutputText = DecryptPolybius(InputText);
                }
            }
            else
            {

            }
        }

        private char[,] GeneratePolybiusGrid(int rows, int columns)
        {
            if (rows <= 0 || columns <= 0)
            {
                return null;
            }

            char[,] grid = new char[rows, columns];
            int currentIndex = 0;

            string polishAlphabet = "a¹bcædeêfghijkl³mnñoópqrsœtuvwxyzŸ¿";

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    if (currentIndex < polishAlphabet.Length)
                    {
                        grid[row, col] = polishAlphabet[currentIndex];
                        currentIndex++;
                    }
                    else
                    {
                        grid[row, col] = ' ';
                    }
                }
            }

            return grid;
        }

        private string EncryptPolybius(string input)
        {
            StringBuilder result = new StringBuilder();

            if (PolybiusGrid != null)
            {
                foreach (char c in input.ToLower())
                {
                    if (char.IsLetter(c))
                    {
                        int row, col;
                        if (TryGetCharPosition(c, out row, out col))
                        {
                            row++;
                            col++;
                            result.Append($"{row:D2}{col:D2} ");
                        }
                        else
                        {
                            result.Append(c);
                        }
                    }
                    else
                    {
                        result.Append(c);
                    }
                }
            }

            return result.ToString().Trim();
        }

        private string DecryptPolybius(string input)
        {
            StringBuilder result = new StringBuilder();

            if (PolybiusGrid != null)
            {
                string[] digitPairs = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var digitPair in digitPairs)
                {
                    if (int.TryParse(digitPair.Substring(0, 2), out int row) && int.TryParse(digitPair.Substring(2, 2), out int col))
                    {
                        row--;
                        col--;

                        if (row >= 0 && row < PolybiusGrid.GetLength(0) && col >= 0 && col < PolybiusGrid.GetLength(1))
                        {
                            result.Append(PolybiusGrid[row, col]);
                        }
                        else
                        {
                            result.Append(digitPair);
                        }
                    }
                    else
                    {
                        result.Append(digitPair);
                    }
                }
            }

            return result.ToString();
        }

        private bool TryGetCharPosition(char c, out int row, out int col)
        {
            if (PolybiusGrid != null)
            {
                for (row = 0; row < PolybiusGrid.GetLength(0); row++)
                {
                    for (col = 0; col < PolybiusGrid.GetLength(1); col++)
                    {
                        if (PolybiusGrid[row, col] == c)
                        {
                            return true;
                        }
                    }
                }
            }

            row = col = -1;
            return false;
        }
    }
}
