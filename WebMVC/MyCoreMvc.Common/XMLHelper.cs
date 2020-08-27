﻿using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;


namespace MyCoreMvc.Common
{
    public class XMLHelper
    {
        /// <summary>
        /// 对象序列化成 XML String  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string XmlSerialize<T>(T obj)
        {
            string xmlString = string.Empty;
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            MemoryStream ms = new MemoryStream();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "    ";
            settings.NewLineChars = "\r\n";
            settings.Encoding = Encoding.UTF8;
            settings.OmitXmlDeclaration = true;  // 不生成声明头

            using (XmlWriter xmlWriter = XmlWriter.Create(ms, settings))
            {
                // 强制指定命名空间，覆盖默认的命名空间
                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add(string.Empty, string.Empty);
                serializer.Serialize(xmlWriter, obj, namespaces);
                xmlWriter.Close();
            };
            xmlString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Dispose();
            return xmlString;
        }

        /// <summary>  
        /// XML String 反序列化成对象  
        /// </summary>  
        public static T XmlDeserialize<T>(string xmlString)
        {
            T t = default(T);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (Stream xmlStream = new MemoryStream(Encoding.UTF8.GetBytes(xmlString)))
            {
                using (XmlReader xmlReader = XmlReader.Create(xmlStream))
                {
                    Object obj = xmlSerializer.Deserialize(xmlReader);
                    t = (T)obj;
                }
            }
            return t;
        }
    }
}

