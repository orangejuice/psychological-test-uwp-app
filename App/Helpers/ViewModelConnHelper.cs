using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Helpers
{
    public static class ViewModelConnHelper
    {

        public static void BroadCast(string message)
        {
            OnMessageTransmitted?.Invoke(message);

            //if (OnMessageTransmitted != null)
            //    OnMessageTransmitted(message);
        }

        public static Action<string> OnMessageTransmitted;
    }
}
