using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models
{

    public class Token
    {
        /// <summary>
        /// The access token
        /// </summary>
        public string key { get; set; }


        public string detail { get; set; }
    }
}
