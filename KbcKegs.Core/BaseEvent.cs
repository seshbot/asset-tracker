using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KbcKegs.Core
{
    public class BaseEvent : BaseEntity
    {
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd H:mm}")]
        public DateTime DateTime { get; set; }
    }
}
