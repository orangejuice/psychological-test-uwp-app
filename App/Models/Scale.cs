using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models
{
    public class ScaleOpt
    {
        public int key { get; set; }
        public string value { get; set; }
    }

    public class ScaleItem
    {
        public string url { get; set; }
        public int sn { get; set; }
        public string question { get; set; }
        public IList<ScaleOpt> opts { get; set; }
        public int chose { get; set; }
        public bool isChose { get; set; }
    }

    public class Scale
    {
        public Uri url { get; set; }
        public int id { get; set; }
        public string title { get; set; }
        public IList<ScaleItem> items { get; set; }
        public string introduction { get; set; }
        public Uri thumbnail { get; set; }
        public DateTime created { get; set; }
        public bool is_top { get; set; }
        public Uri done { get; set; }
    }

}
