using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetProperties
{
    public enum Zone
    {
        LocalMachine = 0, // 0 is the Local Machine zone
        Intranet = 1, // 1 is the Intranet zone
        TrustedSites = 2, // 2 is the Trusted Sites zone
        Internet = 3, // 3 is the Internet zone
        RestrictedSites = 4 // 4 is the Restricted Sites zone
    }
}
