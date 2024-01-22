using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;

namespace Encrypting.Pages
{
    public class PolybiusModel : PageModel
    {
        [BindProperty]
        public string InputText { get; set; }

        public string EncryptedText { get; private set; }

        public char[,] PolybiusGrid { get; set; }

        public void OnPost(string gridConfig)
        {
            var configParts = gridConfig.Split('-');
            if (configParts.Length == 2 && int.TryParse(configParts[0], out int rows) && int.TryParse(configParts[1], out int columns))
            {
                PolybiusGrid = GeneratePolybiusGrid(rows, columns);
                EncryptedText = EncryptPolybius(InputText);
            }
            else
            {
                // Handle invalid configuration
            }
        }

        private char[,] GeneratePolybiusGrid(int rows, int columns)
        {
            // Validate the input and ensure it's a valid grid size
            if (rows <= 0 || columns <= 0)
            {
                // Invalid grid size, return null or handle accordingly
                return null;
            }

            char[,] grid = new char[rows, columns];
            int currentIndex = 0;

            // Define the Polish alphabet
            string polishAlphabet = "a¹bcædeêfghijkl³mnñoópqrsœtuvwxyzŸ¿";

            // Fill the grid with characters from the Polish alphabet
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
                        // If the Polish alphabet is exhausted, fill the remaining cells with spaces or handle accordingly
                        grid[row, col] = ' ';
                    }
                }
            }

            return grid;
        }

        private string EncryptPolybius(string input)
        {
            StringBuilder result = new StringBuilder();

            // Ensure PolybiusGrid is not null before proceeding
            if (PolybiusGrid != null)
            {
                foreach (char c in input.ToLower())
                {
                    if (char.IsLetter(c))
                    {
                        int row, col;
                        if (TryGetCharPosition(c, out row, out col))
                        {
                            result.Append($"{row}{col} ");
                        }
                        else
                        {
                            // Character not found in the user-defined grid, keep it unchanged
                            result.Append(c);
                        }
                    }
                    else
                    {
                        // Non-letter characters or characters not in the Polish alphabet, keep them unchanged
                        result.Append(c);
                    }
                }
            }

            return result.ToString().Trim();
        }

        private bool TryGetCharPosition(char c, out int row, out int col)
        {
            if (PolybiusGrid != null)
            {
                for (row = 1; row <= PolybiusGrid.GetLength(0); row++)
                {
                    for (col = 1; col <= PolybiusGrid.GetLength(1); col++)
                    {
                        if (PolybiusGrid[row - 1, col - 1] == c)
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
