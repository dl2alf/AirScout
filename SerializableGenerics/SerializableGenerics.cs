/*
 * SerializableGenerics
 * Copyright (c) 2009, Eduardo Sanchez-Ros
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, version 3 of the License.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Text;

namespace SerializableGenerics
{
    public static class SerializableGenerics
    {
        private const string OF = "Of";

        /// <summary>
        /// Gets the key-value pair name
        /// </summary>
        /// <param name="tKey">Key</param>
        /// <param name="tValue">Value</param>
        /// <returns></returns>
        public static String GetKeyValuePairName(Type tKey, Type tValue)
        {
            // return key-value pair name
            return new StringBuilder().Append(GetTypeName(tKey))
                                      .Append(GetTypeName(tValue))
                                      .ToString();
        }

        /// <summary>
        /// Returns the name of the type
        /// </summary>
        /// <param name="type">Type to generate the name from</param>
        /// <returns>The name of the type</returns>
        public static String GetTypeName(Type type)
        {
            StringBuilder typeName;

            // Is type generic
            if (type.IsGenericType)
            {
                // Get type name - Generics 
                typeName = new StringBuilder(type.Name.Substring(0, type.Name.Length - 2));
                typeName.Append(OF);

                // Get type's arguments
                Type[] types = type.GetGenericArguments();
                foreach (Type t in types)
                {
                    // Append type's name
                    typeName.Append(GetTypeName(t));
                }
            }
            else if (type.IsArray)
            {
                // Compose array name as "ArrayOf" and get array element's type name
                typeName = new StringBuilder(type.BaseType.Name);
                typeName.Append(OF);
                typeName.Append(GetTypeName(type.GetElementType()));
            }
            else
            {
                // Append type's name
                typeName = new StringBuilder(type.Name);
            }

            // return type's name
            return typeName.ToString();
        }
    }
}
