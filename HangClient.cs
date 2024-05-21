/*
 * This file (HangClient.cs) is part of HangmanReworked.
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

using HangmanReworked.Exceptions;

namespace HangmanReworked {
    public static class HangClient {
        static readonly HttpClient Client = new HttpClient();
        private static HttpResponseMessage _response = new HttpResponseMessage();
        private static string _responseBody = "";
        
        public static async Task<string> GetWord(string? url) {
            // If the URL is null or empty, set it to the word API
            if (string.IsNullOrEmpty(url)) {
                url = "https://random-word-api.herokuapp.com/word?number=1";
            } 
            
            // Try to get the word from the API, return the word if successful
            try {
                 _response = await Client.GetAsync(url);
                _response.EnsureSuccessStatusCode();
                _responseBody = await _response.Content.ReadAsStringAsync();
                return _responseBody;
            } catch (Exception e) {
                throw new BadResponseCodeException(e.Message, url);
            }
        }
    }
}