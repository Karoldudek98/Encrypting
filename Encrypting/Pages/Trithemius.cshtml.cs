using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Text;

namespace Encrypting.Pages
{
    public class TrithemiusModel : PageModel
    {
        [BindProperty]
        public string InputText { get; set; }

        [BindProperty]
        public int Shift { get; set; }

        [BindProperty]
        public string Operation { get; set; }

        public string ResultText { get; private set; }

        public void OnPost()
        {
            if (Operation == "encrypt")
            {
                ResultText = EncryptTrithemius(InputText, Shift);
            }
            else if (Operation == "decrypt")
            {
                ResultText = DecryptTrithemius(InputText, Shift);
            }
        }

        private string EncryptTrithemius(string input, int shift)
        {
            StringBuilder result = new StringBuilder();

            foreach (char c in input.ToUpper())
            {
                if (char.IsLetter(c))
                {
                    char baseChar = 'A';
                    char encryptedChar = (char)((c - baseChar + shift) % 26 + baseChar);
                    result.Append(encryptedChar);
                }
                else
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        }

        private string DecryptTrithemius(string input, int shift)
        {
            return EncryptTrithemius(input, 26 - shift);
        }
    }
}
