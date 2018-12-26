using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFDisplay.Exceptions
{
    public class MFConfigurationException : Exception
    {
        public string ConfigurationMessage { get; set; }
        public Exception Exception { get; set; }
    }
}
