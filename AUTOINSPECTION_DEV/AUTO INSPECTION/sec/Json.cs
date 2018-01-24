using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AutoInspection
{
    public static class Json
    {
        public static string GetString(object o)
        {
            string _sTestSpecString = JsonConvert.SerializeObject(o);     // cmd save
            string _sTestSpecStringIndented = JToken.Parse(_sTestSpecString).ToString(Formatting.Indented);
            return _sTestSpecStringIndented;
        }
    }
}
