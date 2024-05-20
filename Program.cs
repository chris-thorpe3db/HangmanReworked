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
 * You should have recieved a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace HangmanReworked {
    public class Program {

        static string? guessThisWord = null;
        readonly static string url = "https://random-word-api.herokuapp.com/word?number=1";
        static string? dashesToString = null;
        static string? consoleInput = null;
        static string? playAgain = null;
        readonly static string rstlne = "rstlne";
        static char[]? dashes = null;
        static char charGuessed = ' ';
        readonly static char[] charsToTrim = { '[', ']', '"' };
        static List<char> charsGuessed = new List<char>();
        static bool wordIsNotExitOrAbout = false;
        static bool breakpointReached = false;
        static bool userWon = false;
        static bool containsChar = false;
        static int incorrectGuessesLeft = 10;
        
        public static void Main(string[] args) {
            Console.Clear();
            Console.WriteLine("Hangman Reworked is licensed under GNU GPL 3.0. \nThis program comes with ABSOLUTELY NO WARRANTY; for details type 'show w'. This is free software, and you are welcome to redistribute it under certain conditions; type 'show c' for details. \nFor more information about this software itself, type 'about'. \nPress any key to continue.");
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine("Welcome to hangman! \nPlease enter one character at a time, or the entire word. \nThere are no numbers or punctuation. \nYou have already been given the letters RSTLNE. \nTo exit, click the x button on the window, type \"exit\" into the console, or type CTRL + C at any time. \nGood luck, and have fun!");
            
            // Plays Game, asks user if they would like to play again
            while (true) {
                PlayGame();
                Console.WriteLine("Would you like to play again? (Y)es/(N)o");
                playAgain = Console.ReadLine()!.ToLower() ?? "   ";
                if (playAgain == "n" || playAgain == "no") {
                    Console.WriteLine("Thanks for playing! \nCopyright (C) 2024 Christopher Thorpe. Licensed under GNU GPL v3.0.");
                    Thread.Sleep(3000);
                    Exit(0);
                } else if (playAgain == "y" || playAgain == "yes") {
                    continue;
                } else {
                    continue;
                }
            }
        }

        public static void PlayGame()
        {
            // Set all variables to defaults and clear char list so we can play the game multiple times
            guessThisWord = null;
            dashesToString = null;
            consoleInput = null;
            dashes = null;
            charGuessed = ' ';
            charsGuessed.Clear();
            wordIsNotExitOrAbout = false;
            breakpointReached = false;
            userWon = false;
            containsChar = false;
            incorrectGuessesLeft = 10;

            // Get the word to guess from the API
            while (!wordIsNotExitOrAbout)
            {
                try
                {
                    guessThisWord = HangClient.GetWord(url).Result.Trim(charsToTrim);
                    if (guessThisWord != "exit" && guessThisWord != "about" && guessThisWord is not null)
                        wordIsNotExitOrAbout = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception caught! Are you connected to the internet? \nDetails:\n");
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Press any key to exit...");
                    Console.ReadKey();
                    Exit(0);
                }
            }

            // Create a char array of the word to guess, convert to string so we can directly compare to the word
            dashes = new char[guessThisWord!.Length];
            for (int i = 0; i < guessThisWord.Length; i++)
            {
                dashes[i] = '-';
            }


            // Seek thru char array for RSTLNE

            for (int x = 0; x < guessThisWord.Length; x++)
            {
                for (int y = 0; y < rstlne.Length; y++)
                {
                    if (guessThisWord[x] == rstlne[y])
                        dashes[x] = rstlne[y];
                }
            }

            // Add RSTLNE to guess chars list so the user doesn't accidentally guess them again
            charsGuessed.AddRange(rstlne);
            dashesToString = new string(dashes);

            // Game will NOT run while this is true, so set it to false
            breakpointReached = false;

            // Game Loop
            while (!breakpointReached)
            {
                containsChar = false;
                Console.WriteLine(dashesToString);
                Console.WriteLine("Incorrect guesses left: " + incorrectGuessesLeft);

                // Get user input, determine string length, grab 1st char of console input when able/necessary
                consoleInput = Console.ReadLine()!.ToLower();
                if (consoleInput == "show c")
                {
                    // Show GNU GPL 3.0 Copyright/Distribution Information
                    Console.WriteLine(
                        "This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version. \nFor more information, visit https://www.gnu.org/licenses/. \n\nPress any key to continue.");
                    Console.ReadKey();
                    Console.Clear();
                    continue;
                }
                else if (consoleInput == "show w")
                {
                    // Show GNU GPL 3.0 Warranty Information
                    Console.WriteLine(
                        "This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. \nFor more information, visit https://www.gnu.org/licenses/. \n\nPress any key to continue.");
                    Console.ReadKey();
                    Console.Clear();
                    continue;
                }
                else if (consoleInput == guessThisWord)
                {
                    // User guesses entire word
                    userWon = true;
                    dashesToString = guessThisWord;
                    Console.Clear();
                    break;
                }
                else if (consoleInput == "exit")
                {
                    // User wants to exit the game
                    Exit(0);
                }
                else if (consoleInput == "about")
                {
                    // User wants to view the about page
                    Console.Clear();
                    Console.WriteLine("Hangman Reworked is a simple hangman game that runs in a console. \nThis program is written in C# and runs on the .NET 8 runtime It was programmed by Christopher Thorpe. \nThis program is free under certain conditions. These conditions are detailed in the GNU GPL v3.0. \nYou can also view this program's soure code in your web browser at \"https://github.com/chris-thorpe3db/HangmanReworked\". \n\nPress any key to continue.");
                    Console.ReadKey();
                    Console.Clear();
                    continue;
                }
                else if (consoleInput.Length != 1)
                {
                    // Console input not 1 character, unable to parse
                    Console.Clear();
                    Console.WriteLine("Please enter only 1 letter!");
                    continue;
                }
                else if (charsGuessed.Contains(consoleInput[0]))
                {
                    // User already guessed this character
                    Console.Clear();
                    Console.WriteLine("You already guessed this character!");
                    continue;
                }

                // Parse string to char: allows us to compare to individual characters in a string
                charGuessed = consoleInput[0];
                charsGuessed.Add(charGuessed);

                // Compare guessed char to every char in the word; replace dashes as neccessary
                for (int i = 0; i < guessThisWord.Length; i++)
                {
                    if (guessThisWord[i] == charGuessed)
                    {
                        dashes[i] = charGuessed;
                        containsChar = true;
                    }
                }

                // Subtract guesses based on the value of containsChar
                if (!containsChar)
                    incorrectGuessesLeft--;

                // Convert char array to string so we can compare it easily
                dashesToString = new string(dashes);

                // Check if the user has won or lost
                if (dashesToString == guessThisWord)
                {
                    userWon = true;
                    breakpointReached = true;
                }
                else if (incorrectGuessesLeft == 0)
                {
                    userWon = false;
                    breakpointReached = true;
                }

                Console.Clear();
            }

            if (userWon)
            {
                Console.WriteLine("Congratulations! The word was: " + guessThisWord + ". You had " +
                                  incorrectGuessesLeft + " incorrect guesses left.");
            }
            else
            {
                Console.WriteLine("You lost! The word was: " + guessThisWord);
            }
        }

        public static void Exit(int code) {
            Environment.Exit(code);
        }
    }
}