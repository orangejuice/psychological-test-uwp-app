using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models
{

    public class Conclusion
    {
        public Uri url { get; set; }
        public string key { get; set; }
        public string description { get; set; }
    }

    public class ScaleResult
    {
        public string url { get; set; }
        public string score { get; set; }
        public Dictionary<int, Conclusion> conclusion { get; set; }
        public DateTime created { get; set; }
    }
}
