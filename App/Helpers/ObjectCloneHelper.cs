using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace App.Helpers
{
    class ObjectCloneHelper
    {
        public static async Task<T> CloneAsync<T>(T obj)
        {
            T ret = default(T);
            if (obj != null)
            {
                //XmlSerializer cloner = new XmlSerializer(typeof(T));
                //MemoryStream stream = new MemoryStream();
                //cloner.Serialize(stream, obj);
                //stream.Seek(0, SeekOrigin.Begin);
                //ret = (T)cloner.Deserialize(stream);

                var str = await Json.StringifyAsync(obj);
                ret = await Json.ToObjectAsync<T>(str);
            }
            return ret;
        }
    }
}
