using System;
using System.Collections.Generic;

namespace MaydSchedulerApp
{
    public class CoreSaveType
    {
        public SerializableDictionary<DateTime, Week> weekList = new SerializableDictionary<DateTime, Week>();
        public string GenerationDate;
        public string LastModified;

        public CoreSaveType() { }
    }
}