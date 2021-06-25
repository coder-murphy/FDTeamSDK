using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace FDSDK.Extensions
{
    /// <summary>
    /// Xml扩展方法
    /// </summary>
    public static class XmlExtensions
    {
        /// <summary>
        /// 获取一个xml节点所有属性
        /// </summary>
        public static string[] TakeAttributes(this XmlNode node)
        {
            var list = new List<string>();
            foreach (XmlAttribute att in node.Attributes)
            {
                list.Add(att.Value);
            }
            return list.ToArray();
        }

        /// <summary>
        /// 获取一个xml节点所有浮点属性
        /// </summary>
        public static float[] TakeFloatAttributes(this XmlNode node)
        {
            var list = new List<float>();
            foreach (XmlAttribute att in node.Attributes)
            {
                list.Add(float.Parse(att.Value));
            }
            return list.ToArray();
        }

        /// <summary>
        /// 为xml元素添加一个属性
        /// </summary>
        /// <param name="elem"></param>
        /// <param name="attName"></param>
        /// <param name="attValue"></param>
        /// <returns></returns>
        public static XmlAttribute AddAttribute(this XmlElement elem, string attName, string attValue)
        {
            if (elem == null || elem.OwnerDocument == null)
                return null;
            XmlAttribute att = elem.OwnerDocument.CreateAttribute(attName);
            att.Value = attValue;
            elem.Attributes.Append(att);
            return att;
        }
    }
}
