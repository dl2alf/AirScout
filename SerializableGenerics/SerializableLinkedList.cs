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
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;

namespace SerializableGenerics
{
    /// <summary>
    /// Represents a strongly typed serializable list of objects that can be accessed by index.
    /// </summary>
    /// <typeparam name="T">The type of the items on the linked list.</typeparam>
    public class SerializableLinkedList<T> : LinkedList<T>, IXmlSerializable
    {
        // store list type
        private readonly Type m_type = typeof(T);

        /// <summary>
        /// Returns a string that represents the current SerializableDictionary.
        /// </summary>
        /// <returns>
        /// A string that represents the current SerializableDictionary.
        /// </returns>
        public override string ToString()
        {
            return SerializableGenerics.GetTypeName(GetType());
        }

        #region IXmlSerializable Members

        /// <summary>
        /// This property is reserved, apply the System.Xml.Serialization.XmlSchemaProviderAttribute
        /// to the class instead.
        /// </summary>
        /// <returns>
        /// An System.Xml.Schema.XmlSchema that describes the XML representation of the
        /// object that is produced by the System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)
        /// method and consumed by the System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)
        /// method.
        /// </returns>
        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The System.Xml.XmlReader stream from which the object is deserialized.</param>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            // Create xml serializer for type
            XmlSerializer typeSerializer = new XmlSerializer(m_type);

            // Read start element and move to content
            reader.ReadStartElement();
            reader.MoveToContent();

            // Loop through elements
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                // Deserialize type
                T value = (T)typeSerializer.Deserialize(reader);

                // Create a liked list node of T
                LinkedListNode<T> node = new LinkedListNode<T>(value);

                // Add node to linked list
                AddLast(node);
            }

            // Read end element and move to content
            reader.ReadEndElement();
            reader.MoveToContent();
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The System.Xml.XmlWriter stream to which the object is serialized.</param>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            // Create xml serializer for type
            XmlSerializer typeSerializer = new XmlSerializer(m_type);

            IEnumerator<T> enumerator = GetEnumerator();
            while (enumerator.MoveNext())
            {
                // Serialize type
                typeSerializer.Serialize(writer, enumerator.Current);
            }
        }

        #endregion
    }
}