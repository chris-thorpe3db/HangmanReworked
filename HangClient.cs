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
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HangmanReworked {
    public class HangClient {
        static readonly HttpClient client = new HttpClient();
        private static HttpResponseMessage response = new HttpResponseMessage();
        private static string responseBody = "";
        #nullable enable annotations
        public static async Task<string> GetWord(string? url) {
            // If the URL is null or empty, set it to the word API
            if (string.IsNullOrEmpty(url)) {
                url = "https://random-word-api.herokuapp.com/word?number=1";
            } 
            
            // Try to get the word from the API, return the word if successful
            try {
                response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            } catch (Exception e) {
                throw new BadResponseCodeException(e.Message, url);
            }
        }
    }
}