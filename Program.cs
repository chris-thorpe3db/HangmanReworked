/* 
 * HangmanReworked is a simple hangman game that runs in a console.
 * Copyright (C) 2024 Christopher Thorpe.
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
 */

using static System.Environment;

namespace HangmanReworked 
{
    public static class Program 
    {
        private static string? _guessThisWord;
        private static readonly string Url = "https://random-word-api.herokuapp.com/word?number=1";
        private static string? _dashesToString;
        private static string? _consoleInput;
        private static string? _playAgain;
        private static readonly string Rstlne = "rstlne";
        private static char[]? _dashes;
        private static char _charGuessed = ' ';
        private static readonly char[] CharsToTrim = ['[', ']', '"'];
        private static readonly List<char> CharsGuessed = [];
        private static bool _wordIsNotExitOrAbout;
        private static bool _breakpointReached;
        private static bool _userWon;
        private static bool _containsChar;
        private static int _incorrectGuessesLeft = 10;
        
        public static void Main(string[] args) 
        {
            Console.Clear();
            Console.WriteLine("Hangman Reworked is licensed under GNU GPL 3.0. \nThis program comes with ABSOLUTELY NO WARRANTY; for details type 'show w'. This is free software, and you are welcome to redistribute it under certain conditions; type 'show c' for details. \nFor more information about this software itself, type 'about'. \nPress any key to continue.");
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine("Welcome to hangman! \nPlease enter one character at a time, or the entire word. \nThere are no numbers or punctuation. \nYou have already been given the letters RSTLNE. \nTo exit, click the x button on the window, type \"exit\" into the console, or type CTRL + C at any time. \nGood luck, and have fun!");
            
            // Plays Game, asks user if they would like to play again
            while (true) 
            {
                PlayGame();
                Console.WriteLine("Would you like to play again? (Y)es/(N)o");
                _playAgain = Console.ReadLine()!.ToLower();
                if (_playAgain == "n" || _playAgain == "no") 
                {
                    Console.WriteLine("Thanks for playing! \nCopyright (C) 2024 Christopher Thorpe. Licensed under GNU GPL v3.0.");
                    Thread.Sleep(3000);
                    break;
                }
            }
        }

        private static void PlayGame()
        {
            // Set all variables to defaults and clear char list, so we can play the game multiple times
            _guessThisWord = null;
            _dashesToString = null;
            _consoleInput = null;
            _dashes = null;
            _charGuessed = ' ';
            CharsGuessed.Clear();
            _wordIsNotExitOrAbout = false;
            _breakpointReached = false;
            _userWon = false;
            _containsChar = false;
            _incorrectGuessesLeft = 10;

            // Get the word to guess from the API
            while (!_wordIsNotExitOrAbout)
            {
                try
                {
                    _guessThisWord = HangClient.GetWord(Url).Result.Trim(CharsToTrim);
                    if (_guessThisWord != "exit" && _guessThisWord != "about")
                        _wordIsNotExitOrAbout = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception caught! Are you connected to the internet? \nDetails:\n");
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Press any key to exit...");
                    Console.ReadKey();
                    Exit(1);
                }
            }

            // Create a char array of the word to guess, convert to string, so we can directly compare to the word
            _dashes = new char[_guessThisWord!.Length];
            for (int i = 0; i < _guessThisWord.Length; i++)
            {
                _dashes[i] = '-';
            }


            // Seek through char array for RSTLNE

            for (int x = 0; x < _guessThisWord.Length; x++)
            {
                foreach (var t in Rstlne)
                {
                    if (_guessThisWord[x] == t)
                        _dashes[x] = t;
                }
            }

            // Add RSTLNE to guess chars list so the user doesn't accidentally guess them again
            CharsGuessed.AddRange(Rstlne);
            _dashesToString = new string(_dashes);

            // Game will NOT run while this is true, so set it to false
            _breakpointReached = false;

            // Game Loop
            while (!_breakpointReached)
            {
                _containsChar = false;
                Console.WriteLine(_dashesToString);
                Console.WriteLine("Incorrect guesses left: " + _incorrectGuessesLeft);

                // Get user input, determine string length, grab 1st char of console input when able/necessary
                _consoleInput = Console.ReadLine()!.ToLower();
                if (_consoleInput == "show c")
                {
                    // Show GNU GPL 3.0 Copyright/Distribution Information
                    Console.WriteLine("This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version. \nFor more information, visit https://www.gnu.org/licenses/. \n\nPress any key to continue.");
                    Console.ReadKey();
                    Console.Clear();
                    continue;
                }
                else if (_consoleInput == "show w")
                {
                    // Show GNU GPL 3.0 Warranty Information
                    Console.WriteLine("This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. \nFor more information, visit https://www.gnu.org/licenses/. \n\nPress any key to continue.");
                    Console.ReadKey();
                    Console.Clear();
                    continue;
                }
                else if (_consoleInput == _guessThisWord)
                {
                    // User guesses entire word
                    _userWon = true;
                    _dashesToString = _guessThisWord;
                    Console.Clear();
                    break;
                }
                else if (_consoleInput == "exit")
                {
                    // User wants to exit the game
                    Exit(0);
                }
                else if (_consoleInput == "about")
                {
                    // User wants to view the about page
                    Console.Clear();
                    Console.WriteLine("Hangman Reworked is a simple hangman game that runs in a console. \nThis program is written in C# and runs on the .NET 8 runtime. It was programmed by Christopher Thorpe. \nThis program is free under certain conditions. These conditions are detailed in the GNU GPL v3.0. \nYou can also view this program's source code in your web browser at \"https://github.com/chris-thorpe3db/HangmanReworked\". \n\nPress any key to continue.");
                    Console.ReadKey();
                    Console.Clear();
                    continue;
                }
                else if (_consoleInput.Length != 1)
                {
                    // Console input not 1 character, unable to parse
                    Console.Clear();
                    Console.WriteLine("Please enter only 1 letter!");
                    continue;
                }
                else if (CharsGuessed.Contains(_consoleInput[0]))
                {
                    // User already guessed this character
                    Console.Clear();
                    Console.WriteLine("You already guessed this character!");
                    continue;
                }

                // Parse string to char: allows us to compare to individual characters in a string
                _charGuessed = _consoleInput[0];
                CharsGuessed.Add(_charGuessed);

                // Compare guessed char to every char in the word; replace dashes as necessary
                for (int i = 0; i < _guessThisWord.Length; i++)
                {
                    if (_guessThisWord[i] == _charGuessed)
                    {
                        _dashes[i] = _charGuessed;
                        _containsChar = true;
                    }
                }

                // Subtract guesses based on the value of containsChar
                if (!_containsChar)
                    _incorrectGuessesLeft--;

                // Convert char array to string, so we can compare it easily
                _dashesToString = new string(_dashes);

                // Check if the user has won or lost
                if (_dashesToString == _guessThisWord)
                {
                    _userWon = true;
                    _breakpointReached = true;
                }
                else if (_incorrectGuessesLeft == 0)
                {
                    _userWon = false;
                    _breakpointReached = true;
                }

                Console.Clear();
            }

            if (_userWon)
            {
                Console.WriteLine("Congratulations! The word was: " + _guessThisWord + ". You had " + _incorrectGuessesLeft + " incorrect guesses left.");
            }
            else
            {
                Console.WriteLine("You lost! The word was: " + _guessThisWord);
            }
        }
    }
}