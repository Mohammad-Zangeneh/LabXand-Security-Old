using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Kms.Security.Util
{
    public class EnumHelper
    {
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static T GetMaxValue<T>()
        {
            var last = Enum.GetValues(typeof(T)).Cast<T>().Max();
            return last;
        }
        public static IList<StatusDescription> Get<T>()
        {
            List<StatusDescription> Statuslist = new List<StatusDescription>();
            foreach (var t in Enum.GetValues(typeof(T)))
            {
                Statuslist.Add(new StatusDescription
                {
                    Value  = EnumHelper.GetEnumDescription((Enum)t).ToString(),
                    Id = (int)t
                });

            }
            return Statuslist;
        }
    }
}
