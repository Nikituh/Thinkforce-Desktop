using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace thinkforce_desktop.Networking.Response
{

    public class LampResponse
    {
        const string LINKNOTPRESSED = "link button not pressed";

        public string Username { get; set; }

        public LampError Error { get; set; }

        public bool IsOk { get { return Error == null; } }

        public bool LinkNotPressedError
        {
            get
            {
                return Error.Description == LINKNOTPRESSED;
            }
        }
    }

    public class LampError
    {
        public int Type { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }
    }
}
