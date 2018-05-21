using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models
{
    public class Pagination<T>
    {
        public int count { get; set; }
        public Uri next { get; set; }
        public Uri previous { get; set; }
        public IList<T> results { get; set; }
    }
}
