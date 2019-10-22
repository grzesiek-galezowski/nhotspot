using System;
using System.Collections.Generic;
using System.Linq;

namespace ResultRendering
{
    //silly name on purpose, to remember to clean it up as soon as possible
    public static class TrashBinTrolololo
    {
        public static string AsJavaScriptArrayString(IEnumerable<string> dataEntries)
        {
            return string.Join(", ", dataEntries.Select(entry => "'" + entry + "'"));
        }
    }
}