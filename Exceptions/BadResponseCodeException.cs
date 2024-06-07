/*
 * This file (BadResponseCodeException.cs) is part of HangmanReworked.
 *
 * HangmanReworked is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * HangmanReworked is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with HangmanReworked. If not, see <https://www.gnu.org/licenses/>.
 */


// Full disclosure: I stole this code from the microsoft docs and has no idea what the fuck it does.
namespace HangmanReworked.Exceptions {
    public class BadResponseCodeException : Exception {
        public string URL { get; }
        public BadResponseCodeException() { }

        public BadResponseCodeException(string message) : base(message) { }

        public BadResponseCodeException(string message, Exception innerException) : base(message, innerException) { }

        public BadResponseCodeException(string message, string url) : this(message) { URL = url;}
    }
}