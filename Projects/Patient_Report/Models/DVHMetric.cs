using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient_Report.Models
{
    public class DVHMetric
    {
        //prop + <tab,tab> + object type + <tab,tab> + variable name + <enter>
        public string StructureId { get; set; }
        public string DoseMetric { get; set; }
        public string OutputValue { get; set; }
    }
}
