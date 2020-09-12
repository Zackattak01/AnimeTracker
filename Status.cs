using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AnimeTracker
{
    public enum Status
    {
        [Description("Currently Watching")]
        CurrentlyWatching,

        [Description("Up To Date")]
        UpToDate,

        [Description("Finished")]
        Finished,

        [Description("Queue")]
        Queue,
    }
}
