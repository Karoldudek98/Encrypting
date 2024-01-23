using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;

namespace Encrypting.Pages
{
    public class CaesarModel : PageModel
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
                ResultText = EncryptCaesar(InputText, Shift);
            }
            else if (Operation == "decrypt")
            {
                ResultText = DecryptCaesar(InputText, Shift);
            }
        }

        private string EncryptCaesar(string input, int shift)
        {
            string polishAlphabet = "a¹bcædeêfghijkl³mnñoópqrsœtuvwxyzŸ¿";

            shift = shift % polishAlphabet.Length;

            StringBuilder result = new StringBuilder();

            foreach (char c in input)
            {
                if (char.IsLetter(c))
                {
                    string alphabet = char.IsUpper(c) ? polishAlphabet.ToUpper() : polishAlphabet;

                    int index = alphabet.IndexOf(c);
                    if (index != -1)
                    {
                        int shiftedIndex = (index + shift) % alphabet.Length;
                        char shiftedChar = alphabet[shiftedIndex];
                        result.Append(shiftedChar);
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

            return result.ToString();
        }

        private string DecryptCaesar(string input, int shift)
        {
            shift = -shift;

            return EncryptCaesar(input, shift);
        }
    }
}
