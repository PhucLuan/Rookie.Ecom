using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rookie.Ecom.Business.Extensions
{
    public static class EnumConverExtension
    {
        public static string GetNameString<T>(this T enumType) where T : Enum
        {
            return Enum.GetName(typeof(T), enumType);
        }
        public static int GetValueInt<T>(string name) where T : Enum
        {
            return (int)Enum.Parse(typeof(T), name);
        }
    }
}
