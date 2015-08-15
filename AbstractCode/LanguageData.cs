// This file is part of AbstractCode.
// 
// AbstractCode is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// AbstractCode is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with AbstractCode.  If not, see <http://www.gnu.org/licenses/>.
// 
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace AbstractCode
{
    [XmlRoot("LanguageData")]
    public class LanguageData : IXmlSerializable
    {
        private static XmlSerializer _serializer = new XmlSerializer(typeof (LanguageData));

        public static LanguageData FromXml(string xml)
        {
            using (var reader = new StringReader(xml))
            {
                return (LanguageData)_serializer.Deserialize(reader);
            }
        }

        public string[] Modifiers;
        public string[] Keywords;
        public string[] MemberIdentifiers;

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            var elementName = string.Empty;
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    elementName = reader.Name;
                }
                else if (reader.NodeType == XmlNodeType.Text)
                {
                    switch (elementName)
                    {
                        case "Modifiers":
                            Modifiers = reader.Value.Split(' ');
                            break;
                        case "MemberIdentifiers":
                            MemberIdentifiers = reader.Value.Split(' ');
                            break;
                        case "Keywords":
                            Keywords = reader.Value.Split(' ');
                            break;
                    }
                }
                else if (reader.NodeType == XmlNodeType.EndElement)
                {
                    elementName = string.Empty;
                }
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("Modifiers", string.Join(" ", Modifiers.ToArray()));
            writer.WriteElementString("MemberIdentifiers", string.Join(" ", MemberIdentifiers.ToArray()));
            writer.WriteElementString("Keywords", string.Join(" ", Keywords.ToArray()));
        }
    }

    [XmlRoot("TypeAlias")]
    public struct TypeAlias : IXmlSerializable
    {
        public TypeAlias(string newName, string original)
        {
            NewName = newName;
            OriginalType = original;
        }

        public string NewName;
        public string OriginalType;

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            var elementName = string.Empty;
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    elementName = reader.Name;
                }
                if (reader.NodeType == XmlNodeType.Text)
                {
                    if (elementName == "NewName")
                        NewName = reader.Value;
                    else if (elementName == "Original")
                        OriginalType = reader.Value;
                }
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("Original", OriginalType);
            writer.WriteElementString("NewName", NewName);
        }
    }
}