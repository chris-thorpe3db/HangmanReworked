using HangmanReworked.Exceptions;

using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HangmanReworked {
    public class HangClient {
        static readonly HttpClient client = new HttpClient();
        #nullable enable annotations
        public static async Task<string> GetWord(string? url) {
            // If the URL is null or empty, set it to the word API
            if (url == null || url == "") {
                url = "https://random-word-api.herokuapp.com/word?number=1";
            } 
            
            // Try to get the word from the API, return the word if successful
            try {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            } catch (Exception e) {
                throw new BadResponseCodeException(e.Message, url);
            }


        }
    }
}