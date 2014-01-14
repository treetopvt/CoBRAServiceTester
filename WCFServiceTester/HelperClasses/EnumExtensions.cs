using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WCFServiceTester.HelperClasses
{
    public static class EnumExtensions
    {
        public static string ToString(this Enum value, bool indicator)
        {
            if (indicator)
            {
                string description = string.Empty;
                FieldInfo field = value.GetType().GetField(value.ToString());
                DescriptionAttribute[] attributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attributes.Length > 0)
                    description = attributes[0].Description;
                else
                    description = value.ToString();
                return description;
            }
            return value.ToString();
        }
    }
}
