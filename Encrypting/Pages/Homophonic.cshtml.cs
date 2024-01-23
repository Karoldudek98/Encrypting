using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Encrypting.Pages
{
    public class HomophonicModel : PageModel
    {
        [BindProperty]
        public string InputText { get; set; }

        [BindProperty]
        public string Operation { get; set; }

        public string ResultText { get; private set; }

        // Define the homophonic cipher key (replace with your own key)
        private readonly Dictionary<char, List<string>> homophonicKey = new Dictionary<char, List<string>>
        {
            {'a', new List<string> {"01", "02", "03"}},
            {'¹', new List<string> {"04", "05", "06"}},
            {'b', new List<string> {"07", "08", "09"}},
            {'c', new List<string> {"10", "11", "12"}},
            {'æ', new List<string> {"13", "14", "15"}},
            {'d', new List<string> {"16", "17", "18"}},
            {'e', new List<string> {"19", "20", "21"}},
            {'ê', new List<string> {"22", "23", "24"}},
            {'f', new List<string> {"25", "26", "27"}},
            {'g', new List<string> {"28", "29", "30"}},
            {'h', new List<string> {"31", "32", "33"}},
            {'i', new List<string> {"34", "35", "36"}},
            {'j', new List<string> {"37", "38", "39"}},
            {'k', new List<string> {"40", "41", "42"}},
            {'l', new List<string> {"43", "44", "45"}},
            {'³', new List<string> {"46", "47", "48"}},
            {'m', new List<string> {"49", "50", "51"}},
            {'n', new List<string> {"52", "53", "54"}},
            {'ñ', new List<string> {"55", "56", "57"}},
            {'o', new List<string> {"58", "59", "60"}},
            {'ó', new List<string> {"61", "62", "63"}},
            {'p', new List<string> {"64", "65", "66"}},
            {'q', new List<string> {"67", "68", "69"}},
            {'r', new List<string> {"70", "71", "72"}},
            {'s', new List<string> {"73", "74", "75"}},
            {'œ', new List<string> {"76", "77", "78"}},
            {'t', new List<string> {"79", "80", "81"}},
            {'u', new List<string> {"82", "83", "84"}},
            {'v', new List<string> {"85", "86", "87"}},
            {'w', new List<string> {"88", "89", "90"}},
            {'x', new List<string> {"91", "92", "93"}},
            {'y', new List<string> {"94", "95", "96"}},
            {'z', new List<string> {"97", "98", "99"}},
            {'Ÿ', new List<string> {"00", "01", "02"}},
            {'¿', new List<string> {"03", "04", "05"}},
        };

        public Dictionary<char, List<string>> HomophonicKey { get; private set; }

        public HomophonicModel()
        {
            HomophonicKey = homophonicKey;
        }

        public void OnPost()
        {
            if (Operation == "encrypt")
            {
                ResultText = EncryptHomophonic(InputText);
            }
            else if (Operation == "decrypt")
            {
                ResultText = DecryptHomophonic(InputText);
            }
        }

        private string EncryptHomophonic(string input)
        {
            StringBuilder result = new StringBuilder();

            foreach (char c in input.ToLower())
            {
                if (char.IsLetter(c))
                {
                    if (homophonicKey.TryGetValue(c, out List<string> codes))
                    {
                        // Randomly select one code for the letter
                        string selectedCode = codes[new Random().Next(codes.Count)];
                        result.Append(selectedCode + " ");
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

            return result.ToString().Trim();
        }

        private string DecryptHomophonic(string input)
        {
            StringBuilder result = new StringBuilder();

            // Split the input into individual codes
            string[] codeArray = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var code in codeArray)
            {
                // Find the letter corresponding to the code
                char decodedChar = homophonicKey.FirstOrDefault(kv => kv.Value.Contains(code)).Key;
                result.Append(decodedChar);
            }

            return result.ToString();
        }
    }
}
