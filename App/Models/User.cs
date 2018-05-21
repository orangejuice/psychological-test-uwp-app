using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models
{
    public class User
    {

        public string url { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public IList<object> groups { get; set; }
        public string avatar { get; set; }

    }
}
