using Craiyon.Net;

namespace Craiyon.Net.Tests
{
    public class Dl
    {
        public static async Task Main(string[] args)
        {
            
            Console.WriteLine("Generating Image...");
            var craiyonService = new CraiyonService(4);

            try
            {
                await craiyonService.Generate("Space man", "bruh.jpg");
            } catch(Exception e) {
                Console.WriteLine(e.ToString());
            }
        }
    }
}