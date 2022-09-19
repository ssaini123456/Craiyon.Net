using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

namespace Craiyon.Net
{
    public class CraiyonService
    {
        private const int GALLERY_MAX = 9;  // The max amount of images in a "gallery"

        private int _timeSpan = 60 * 5; // 5 minutes
        private int _galleryIndex = 0;
        private string _baseUrl = "https://backend.craiyon.com/generate";

        private class Craiyon
        {   /* internal */
            public string? prompt { get; set; }
        }

        /// <summary>
        ///     Craiyon is an AI based Image Generation Service that uses DALL-E.
        /// </summary>
        /// <param name="galleryIndex">The index within the image gallery you would like to download.</param>
        public CraiyonService([Optional] int galleryIndex) {
            _galleryIndex = galleryIndex;
        }

        /// <summary>
        ///     Get the current gallery index
        /// </summary>
        /// <returns> The image index within the image gallery. </returns>
        public int GetGalleryIndex()
        {
            return _galleryIndex;
        }

        /// <summary>
        ///     Set the gallery index
        /// </summary>
        /// <param name="gIndex">The gallery index.</param>
        public void SetGalleryIndex(int gIndex)
        {
            _galleryIndex = gIndex;
        }

        /// <summary>
        ///     Writes the base64 image information returned by craiyon to an image
        ///     when provided a path.
        /// </summary>
        /// <param name="b64_Data"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private async Task DataUrlToImageAsync(string b64_Data, string path)
        {
            var toBase64 = Convert.FromBase64String(b64_Data);
            await File.WriteAllBytesAsync(path, toBase64);
        }

        /// <summary>
        ///     Download the image gallery with the provided prompt.
        /// </summary>
        /// <param name="prompt"></param>
        /// <returns></returns>
        private async Task<JObject> Download(string prompt)
        {
            Craiyon c = new Craiyon();
            c.prompt = prompt;

            string jsonData = JsonSerializer.Serialize(c);
            StringContent requestHeaders = new StringContent(jsonData, Encoding.UTF8, "application/json");

            HttpResponseMessage respAsync = null;
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(_timeSpan);
                respAsync = await client.PostAsync(_baseUrl, requestHeaders);
            }

            var responseString = await respAsync.Content.ReadAsStringAsync();

            JObject gallery = JObject.Parse(responseString);

            return gallery;
        }

        /// <summary>
        ///     Download all the images relating to the given prompt within the image gallery.
        ///     returns true if successful.
        /// </summary>
        /// <param name="prompt">The prompt you would like craiyon to generate on.</param>
        /// <param name="path">The pat to save the image to.</param>
        /// <returns></returns>
        public async Task DownloadGalleryAsync(string prompt, string folderPath)
        {
            if(prompt == null)
            {
                throw new CraiyonInvalidPrompt();
            }

            var gallery = await Download(prompt);

            // In the case where our path doesn't exist, create it.
            if(!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            for(int i = 0; i < GALLERY_MAX; i++)
            {
                string path = $"{folderPath}/{i}.jpg";
                var strippedImage = $"{gallery["images"][i]}";

                await DataUrlToImageAsync(strippedImage, path);
            }
        }

        /// <summary>
        ///     Download a specific image from the prompts image gallery provided an index.
        /// </summary>
        /// 
        /// <param name="prompt">The prompt you would like craiyon to generate on.</param>
        /// <param name="path">The pat to save the image to.</param>
        /// <exception cref="CraiyonCountOutOfBounds"></exception>
        /// <exception cref="CraiyonInvalidPrompt"></exception>
        public async Task DownloadImageSpecificAsync(string prompt, string path)
        {
            if (prompt == null)
            {
                throw new CraiyonInvalidPrompt();
            }

            var gallery = await Download(prompt);

            var strippedImage = $"{gallery["images"][_galleryIndex]}";
            await DataUrlToImageAsync(strippedImage, path);
        }
    }
}