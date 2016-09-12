using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TigeR.YuGiOh.Core
{
    public class EventArgs
    {
        public bool Succeeded { get; set; }

        public string SourceLocation { get; set; }

        public bool HasSourceLocation() => SourceLocation?.Length > 0;

        public EventArgs WithSourceLocation(string location)
        {
            var e = (EventArgs)MemberwiseClone();
            e.SourceLocation = location;
            return e;
        }

        public string TargetLocation { get; set; }

        public bool HasTargetLocation() => TargetLocation?.Length > 0;

        public EventArgs WithTargetLocation(string location)
        {
            var e = (EventArgs)MemberwiseClone();
            e.TargetLocation = location;
            return e;
        }
    }
}
