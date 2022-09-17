using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.Json;

namespace Craiyon.Net
{
    public class CraiyonService
    {
        private int _timeSpan = 60 * 5; // 5 minutes
        private int _galleryIndex = 0;
        private string _base_url = "https://backend.craiyon.com/generate";

        private class Craiyon
        {
            public string? prompt { get; set; }
        }

        /// <summary>
        /// Craiyon is an AI based Image Generation Service that uses DALL-E.
        /// </summary>
        /// <param name="galleryIndex">The index within the image gallery you would like to download.</param>
        public CraiyonService(int galleryIndex) {
            _galleryIndex = galleryIndex;
        }

        private async Task DataUrlToImageAsync(string b64_Data, string path)
        {
            var toBase64 = Convert.FromBase64String(b64_Data);
            await File.WriteAllBytesAsync(path, toBase64);
        }

        /// <summary>
        /// Generate creates a non-blocking call to craiyon, and downloads images from the response.
        /// </summary>
        /// <param name="prompt">The prompt you would like craiyon to generate on.</param>
        /// <param name="path">The pat to save the image to.</param>
        /// <returns></returns>
        /// <exception cref="CraiyonCountOutOfBounds"></exception>
        /// <exception cref="CraiyonInvalidPrompt"></exception>
        public async Task Generate(string prompt, string path)
        {
            if (prompt == null)
            {
                throw new CraiyonInvalidPrompt();
            }

            Craiyon c = new Craiyon();
            c.prompt = prompt;

            string jsonData = JsonSerializer.Serialize(c);
            StringContent requestHeaders = new StringContent(jsonData, Encoding.UTF8, "application/json");

            HttpResponseMessage respAsync = null;
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(_timeSpan);
                respAsync = await client.PostAsync(_base_url, requestHeaders);
            }

            var responseString = await respAsync.Content.ReadAsStringAsync();

            JObject gallery = JObject.Parse(responseString);

            var strippedImage = $"{gallery["images"][_galleryIndex]}";
            await DataUrlToImageAsync(strippedImage, path);
        }
    }
}