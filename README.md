# Craiyon.Net (DotNetCraiyon)
Craiyon.Net (also known as DotNetCraiyon on NuGet) is a library that allows you to interact with Craiyon. An AI Image generation service powered by DALL-E.

<br>[![NuGet version (DotNetCraiyon)](https://img.shields.io/badge/nuget-Latest-blue?style=for-the-badge&logo=appveyor)](https://www.nuget.org/packages/DotNetCraiyon)

<br>
Interacting with this library is simple, and there's two ways you can use it:

* Bulk-Gallery Downloads, where you can download all the images based on the given prompt
* Specific images from the gallery provided an index (starts at zero)

## Usage

Bulk gallery downloads can be done as such:

```cs
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
```

And specific image downloads can be done like this:

```cs
using Craiyon.Net;
namespace Craiyon.Net.Tests
{
    public class ImageDl
    {
        public static async Task Main(string[] args)
        {
            
            Console.WriteLine("Generating Image...");
            var craiyonService = new CraiyonService(1); // Get the 2nd image within the image gallery.
            
                                                        // Note:
                                                        // The index parameter in the constructor is optional. You can set the     
                                                        // wanted index using SetGalleryIndex(index).
            try
            {
                await craiyonService.DownloadImageSpecificAsync("Space man", "specific.jpg");
            } catch(Exception e) {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
```

Simple as that! Enjoy!
