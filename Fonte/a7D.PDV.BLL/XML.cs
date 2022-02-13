using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace a7D.PDV.BLL
{
    public class XML
    {
        public static String RetornarXML(Object obj)
        {
            XmlWriterSettings xmlWS = new XmlWriterSettings();
            xmlWS.OmitXmlDeclaration = true;

            XmlSerializerNamespaces xmlSN = new XmlSerializerNamespaces();
            xmlSN.Add(string.Empty, string.Empty);

            MemoryStream ms = new MemoryStream();
            XmlWriter writer = XmlWriter.Create(ms, xmlWS);

            XmlSerializer serializer = new XmlSerializer(obj.GetType());

            serializer.Serialize(writer, obj, xmlSN);

            ms.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(ms);
            return sr.ReadToEnd();
        }
    }
}
