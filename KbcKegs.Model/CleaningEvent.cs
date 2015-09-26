using KbcKegs.Core;
using System.Collections.Generic;

namespace KbcKegs.Model
{
    public class CleaningEvent : BaseEvent
    {
        public virtual ICollection<Asset> Assets { get; set; }
    }
}