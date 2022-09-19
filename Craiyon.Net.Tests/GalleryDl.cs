using Craiyon.Net;

namespace Craiyon.Net.Tests
{
    public class GalleryDl
    {
        public static async Task Main(string[] args)
        {

            Console.WriteLine("Give me a prompt to generate:");
            string prompt = Console.ReadLine();

            var craiyonService = new CraiyonService(); // Gallery index isn't needed if you are downloading the entire gallery.

            try
            {
                await craiyonService.DownloadGalleryAsync("Time and Space merging into one", "testFolder");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
