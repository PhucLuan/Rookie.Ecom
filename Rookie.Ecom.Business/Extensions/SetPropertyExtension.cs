using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Rookie.Ecom.Business.Extensions
{
    public static class SetPropertyExtension
    {
        public static void SetProperty<T>(this T obj, string property, object value) where T : class
        {
            Type type = obj.GetType();
            PropertyInfo prop = type.GetProperty(property);
            prop.SetValue(obj, value, null);
        }
    }
}
