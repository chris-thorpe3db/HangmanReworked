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

namespace HangmanReworked 
{
    public static class Program 
    {
        // Declare and initialize all static vars
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

        // Entry point
        public static void Main(string[] args) 
        {
            // Short GPL license notice
            Console.Clear();
            Console.WriteLine("Hangman Reworked is licensed under GNU GPL 3.0. \nThis program comes with ABSOLUTELY NO WARRANTY; for details type 'show w'. This is free software, and you are welcome to redistribute it under certain conditions; type 'show c' for details. \nFor more information about this software itself, type 'about'. \nPress any key to continue.");
            Console.ReadKey();
            
            // Short introduction
            Console.Clear();
            Console.WriteLine("Welcome to hangman! \nPlease enter one character at a time, or the entire word. \nThere are no numbers or punctuation. \nYou have already been given the letters RSTLNE. \nTo exit, click the x button on the window, type \"exit\" into the console, or type CTRL + C at any time. \nGood luck, and have fun!");
            
            // We use a while loop here to allow for repeat plays
            while (true) 
            {
                // (in mario voice) Let's-a-go!
                PlayGame();
                Console.WriteLine("Would you like to play again? (Y)es/(N)o");
                _playAgain = Console.ReadLine()!.ToLower();
                if (_playAgain == "n" || _playAgain == "no")
                {
                    Console.WriteLine("Thanks for playing! \nCopyright (C) 2024 Christopher Thorpe. Licensed under GNU GPL v3.0.");
                    Thread.Sleep(3000);

                    // Break loop instead of using Environment.Exit because program will automatically end gracefully with code 0 after Main() finishes executing
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

                    // 'About' and 'exit' are reserved words, so we need to ensure the API doesn't return them
                    if (_guessThisWord != "exit" && _guessThisWord != "about")
                        _wordIsNotExitOrAbout = true;
                }
                catch (Exception e)
                {
                    // The program WILL crash if there is no internet connection. Catch exeption, inform user, exit with code 1.
                    Console.WriteLine("Exception caught! Are you connected to the internet? \nDetails:\n");
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Press any key to exit...");
                    Console.ReadKey();
                    Exit(1);
                }
            }

            // Initialize the dashes char[] with a new array equal in length to the chosen word, fill with '-' characters
            _dashes = new char[_guessThisWord!.Length];
            foreach (char i in _dashes) {
                _dashes[i] = '_';
            }


            // Reveal RSTLNE characters in the dashes array, Wheel of Fortune style!
            for (int x = 0; x < _guessThisWord.Length; x++)
            {
                foreach (var t in Rstlne)
                {
                    if (_guessThisWord[x] == t)
                        _dashes[x] = t;
                }
            }

            // Add RSTLNE to the guessed chars list so the user isn't penalized for guessing them again
            CharsGuessed.AddRange(Rstlne);
            _dashesToString = new string(_dashes);

            // Game loop control variable
            _breakpointReached = false;

            // Game Loop
            while (!_breakpointReached)
            {
                // Make sure the game doesn't think we guessed a char yet
                _containsChar = false;
                
                // Prompt for user input
                Console.WriteLine(_dashesToString);
                Console.WriteLine("Incorrect guesses left: " + _incorrectGuessesLeft);

                _consoleInput = Console.ReadLine()!.ToLower();
                if (_consoleInput == "show c")
                {
                    // Show GNU GPL 3.0 Copyright Information
                    Console.WriteLine("This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version. \nFor more information, visit https://www.gnu.org/licenses/. \n\nPress any key to continue.");
                    Console.ReadKey();
                    Console.Clear();
                    continue;
                }
                if (_consoleInput == "show w")
                {
                    // Show GNU GPL 3.0 Warranty Information
                    Console.WriteLine("This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. \nFor more information, visit https://www.gnu.org/licenses/. \n\nPress any key to continue.");
                    Console.ReadKey();
                    Console.Clear();
                    continue;
                }
                if (_consoleInput == _guessThisWord)
                {
                    // User guessed the entire word correctly, set victory bool and break loop
                    _userWon = true;
                    Console.Clear();
                    break;
                }
                if (_consoleInput == "exit")
                {
                    // User wants to exit the program, exit gracefully with code 0
                    Exit(0);
                }
                if (_consoleInput == "about")
                {
                    // User wants to view the about page
                    Console.Clear();
                    Console.WriteLine("Hangman Reworked is a simple hangman game that runs in a console. \nThis program is written in C# and runs on the .NET 8 runtime. It was programmed by Christopher Thorpe. \nThis program is free under certain conditions. These conditions are detailed in the GNU GPL v3.0. \nYou can also view this program's source code in your web browser at \"https://github.com/chris-thorpe3db/HangmanReworked\". \n\nPress any key to continue.");
                    Console.ReadKey();
                    Console.Clear();
                    continue;
                }
                if (_consoleInput.Length != 1)
                {
                    // Console input greater than 1 character, but not entire word, repeat loop
                    Console.Clear();
                    Console.WriteLine("Please enter only 1 letter!");
                    continue;
                }
                if (CharsGuessed.Contains(_consoleInput[0]))
                {
                    // User already guessed this char, repeat loop
                    Console.Clear();
                    Console.WriteLine("You already guessed this character!");
                    continue;
                }

                // Default case: User guessed a single char, store it and add to guessed chars list
                _charGuessed = _consoleInput[0];
                CharsGuessed.Add(_charGuessed);

                // Check if the guessed char is in the word, update dashes array as needed
                for (int i = 0; i < _guessThisWord.Length; i++)
                {
                    if (_guessThisWord[i] == _charGuessed)
                    {
                        _dashes[i] = _charGuessed;
                        _containsChar = true;
                    }
                }

                // Check the containsChar bool from earlier - decrement incorrect guesses if needed
                if (!_containsChar)
                    _incorrectGuessesLeft--;

                // Convert dashes array to string for display and win/loss checking
                _dashesToString = new string(_dashes);

                // Win/Loss checking
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

                // Clear console output for next loop iteration
                Console.Clear();
            }

            // Game over, display win/loss message, back to Main() for replay prompt
            if (_userWon)
            {
                Console.WriteLine("Congratulations! The word was: " + _guessThisWord + ". You had " + _incorrectGuessesLeft + " incorrect guesses left.");
            }
            else
            {
                Console.WriteLine("You lost! The word was: " + _guessThisWord);
            }
        }

        private static void Exit(int? codeNull)
        {
            int code = codeNull ?? 0;
            Console.WriteLine("Thanks for playing! Copyright © 2026 Christopher Thorpe. All rights reserved.");
            Thread.Sleep(1000);
            Environment.Exit(code);
        }
    }
}