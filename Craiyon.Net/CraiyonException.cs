using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craiyon.Net
{
    internal class CraiyonCountOutOfBounds : Exception
    {
        public CraiyonCountOutOfBounds() : base("Cannot generate image: Count exceeds 6.")
        {
        }
    }

    internal class CraiyonInvalidPrompt : Exception
    {
        public CraiyonInvalidPrompt() : base("Cannot generate image: Invalid prompt provided.")
        {
        }
    }
}
