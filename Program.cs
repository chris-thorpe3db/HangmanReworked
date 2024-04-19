using System;
using System.Collections.Generic;

namespace HangmanReworked {
    public class Program {
        static string guessThisWord = null;
        static string url = "https://random-word-api.herokuapp.com/word?number=1";
        static string dashesToString = null;
        static string consoleInput = null;
        static string rstlne = "rstlne";
        static char[] dashes = null;
        static char charGuessed = ' ';
        static char[] charsToTrim = { '[', ']', '"' };
        static List<char> charsGuessed = new List<char>();
        static bool wordIsNotExit = false;
        static bool breakpointReached = false;
        static bool userWon = false;
        static bool containsChar = false;
        static int incorrectGuessesLeft = 10;
        
        public static void Main(string[] args) {
            
            while (!wordIsNotExit) {
                try {
                    guessThisWord = HangClient.GetWord(url).Result.Trim(charsToTrim);
                } catch (Exception e) {
                    Console.WriteLine("Exception caught! Are you connected to the internet? \nDetails:\n");
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Press any key to exit...");
                    Console.ReadKey();
                }
            }
            
            // Get the word to guess from the API

            // Create a char array of the word to guess, convert to string so we can directly compare to the word
            dashes = new char[guessThisWord.Length];
            for (int i = 0; i < guessThisWord.Length; i++) {
                dashes[i] = '-';
            }
            dashesToString = new string(dashes);

            // Seek thru char awway for RSTLNE

            for (int x = 0; x < guessThisWord.Length; x++) {
                for (int y = 0; y < rstlne.Length; y++) {
                    if (guessThisWord[x] == rstlne[y])
                        dashes[x] = rstlne[y];
                }
            }
            
            // Add RSTLNE to guess chars list so the user doesn't accidentally guess them again
            charsGuessed.AddRange(rstlne);

            // Game will NOT run while this is true, so set it to false
            breakpointReached = false;

            Console.Clear();
            Console.WriteLine("Welcome to hangman! \nPlease enter one character at a time, or the entire word. \nThere are no numbers or punctuation. \nYou have already been given the letters RSTLNE. \nTo exit, click the x button on the window, type \"exit\" into the console, or type CTRL + C at any time. \nGood luck, and have fun!");
			
            // Game Loop
            while (!breakpointReached) {
                containsChar = false;
                Console.WriteLine(dashesToString);
                Console.WriteLine("Incorrect guesses left: " + incorrectGuessesLeft);

                // Get user input, determine string length, parse when able/neccessary
                consoleInput = Console.ReadLine().ToLower();

                if (consoleInput == guessThisWord) { // User guesses entire word
                    userWon = true;
                    dashesToString = guessThisWord;
                    break;
                } else if (consoleInput == "exit") { // User wants to exit the game
                    Environment.Exit(0);
                } else if (consoleInput.Length != 1) { // Console input not 1 character, unable to parse
                    Console.Clear();
                    Console.WriteLine("Please enter only 1 letter!");
                    continue;
                } else if (charsGuessed.Contains(char.Parse(consoleInput))) { // User already guessed this character
                    Console.Clear();
                    Console.WriteLine("You already guessed this character!");
                    continue;
                } 

                // Parse string to char: allows us to compare to individual characters in a string
                charGuessed = char.Parse(consoleInput);
                charsGuessed.Add(charGuessed);

                // Compare guessed char to every char in the word; replace dashes as neccessary
                for (int i = 0; i < guessThisWord.Length; i++) {
                    if (guessThisWord[i] == charGuessed) {
                        dashes[i] = charGuessed;
                        containsChar = true;
                    }
                }
                
                // Subtract guesses based on the value of containsChar
                if (!containsChar)
                    incorrectGuessesLeft--;
                
                // Convert char array to string so we can compare it easily
            }
        }
    }
}