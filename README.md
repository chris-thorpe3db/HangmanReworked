# Reworked C# Console Hangman game

Hangman is a game where the player guesses a word letter by letter until they have either guessed the word, or run out of letters. Oftentimes this is illustrated by a man hanging from his neck from the gallows, hence the word "Hangman."

This is a reworked and cleaner version of my older hangman game.

## Features

Grabs words from a web API rather than a built-in array of words. This does mean that the application requires an internet connection.

Allows the player to enter a whole word, guess letter by letter, or exit the application from the commandline using if-else statements and parsing methods.

Stores previously-guessed characters and doesn't deduct points if the user guesses an already-guessed character.

## Prerequisites

This program requires the .NET 8 Runtime/SDK, as well as a computer capable of running .NET 8.

This program also requires an internet connection, though you can substitute the HttpClient code with a dictionary of words.

## Usage

1. Run `git clone https://github.com/chris-thorpe3db/HangmanReworked.git` and navigate to the root directory.

2. Open a terminal and run `dotnet run`.

## Licensing

This software is licensed under GNU GPL v3.0.

This means that you can:

 - Sell this program
 - Modify it
 - Distribute it
 - Use it for Patent use
 - Use it for Private use

As long as you:

 - Add a copyright and license notice
 - State changes made
 - Disclose the source (this repo)
 - Use the same license (GNU GPL v3.0)

By modifying or distributing this software, you agree to this license.

The full license legal text can be found within [LICENSE](LICENSE.md).

This software comes with absolutely no liability or warranty.

This repository and all contents are Copyright © 2024 Christopher Thorpe.

[![GNU GPL v3 logo](https://www.gnu.org/graphics/gplv3-127x51.png)](https://www.gnu.org/licenses/gpl-3.0.en.html)
