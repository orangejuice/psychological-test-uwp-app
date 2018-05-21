using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models
{
    public class Post
    {
        public Uri url { get; set; }
        public string title { get; set; }
        public Uri cate { get; set; }
        public string cate_name { get; set; }
        public Uri thumbnail { get; set; }
        public User author { get; set; }
        public string content { get; set; }
        public DateTime created { get; set; }
        public DateTime updated { get; set; }
    }

}
