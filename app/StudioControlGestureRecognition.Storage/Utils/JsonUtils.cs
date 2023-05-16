using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudioControlGestureRecognition.Storage.Utils
{
    internal static class JsonUtils
    {
        internal static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        internal static T? Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
