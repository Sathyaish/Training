using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace CompiledTaskSample
{
    public class InlineTask : Microsoft.Build.Utilities.Task
    {
        [Required]
        public string Value { get; set; }

        [Required]
        public ITaskItem Item { get; set; }

        [Output]
        public string RetVal { get; set; }

        public override bool Execute()
        {
            Log.LogMessage(Value);
            Log.LogMessage(Item.GetMetadata("FullPath"));
            RetVal = Item.GetMetadata("FullPath");
            return true;
        }
    }
}
