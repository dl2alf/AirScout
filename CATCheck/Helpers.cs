using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace CATCheck
{
    public static class Helpers
    {
        public static string GetEnumDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr =
                           Attribute.GetCustomAttribute(field,
                             typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }

        public static void BindToEnum<TEnum>(this ComboBox comboBox)
        {
            var enumType = typeof(TEnum);

            var fields = enumType.GetMembers()
                                    .OfType<FieldInfo>()
                                    .Where(p => p.MemberType == MemberTypes.Field)
                                    .Where(p => p.IsLiteral)
                                    .ToList();

            var valuesByName = new Dictionary<string, object>();

            foreach (var field in fields)
            {
                var descriptionAttribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false)[0] as DescriptionAttribute;

                var value = (int)field.GetValue(null);
                var description = string.Empty;

                if (!string.IsNullOrEmpty(descriptionAttribute?.Description))
                {
                    description = descriptionAttribute.Description;
                }
                else
                {
                    description = field.Name;
                }

                valuesByName[description] = value;
            }

            comboBox.DataSource = valuesByName.ToList();
            comboBox.DisplayMember = "Key";
            comboBox.ValueMember = "Value";
        }
    }
}
