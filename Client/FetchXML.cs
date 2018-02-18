using System.Collections.Generic;
using System.Xml;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Client
{
    // see https://msdn.microsoft.com/en-us/library/gg328332.aspx
    /// <summary>
    /// Class to create XML using FetchXML for actions in Dynamics CRM  
    /// </summary>
    class FetchXML
    {
        // use to verify alias when in development
        static Regex rgx = new Regex("^[A-Za-z_][a-zA-Z0-9_]{0,}$");

        XmlDocument doc;
        XmlElement fetchElement;

        public FetchXML()
        {
            doc = new XmlDocument();

            fetchElement = doc.CreateElement("fetch");
            fetchElement.SetAttribute("version", "1.0");
            fetchElement.SetAttribute("mapping", "logical");
            doc.AppendChild(fetchElement);
        }

        /// <summary>
        /// Create a new element to fetch element
        /// </summary>
        /// <param name="element">Name of element</param>
        /// <returns>The new instance of XmlElement</returns>
        public XmlElement AddElement(string element)
        {
            return AddElement(fetchElement, element);
        }

        /// <summary>
        /// Create a new element to parent element
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element">Name of element</param>
        /// <returns></returns>
        public XmlElement AddElement(XmlElement parent, string element)
        {
            XmlElement newElement = doc.CreateElement(element);
            parent.AppendChild(newElement);
            return newElement;
        }

        /// <summary>
        /// Create attribute element to an entity element to return field value
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name">Name of attribute to be included</param>
        /// <param name="optionals">e.g. alias, aggregate</param>
        /// <returns></returns>
        public XmlElement AddField(XmlElement parent, string name, Dictionary<string, string> optionals = null)
        {
            XmlElement field = AddElement(parent, "attribute");
            field.SetAttribute("name", name);
            if (optionals != null)
            {
                foreach (KeyValuePair<string, string> item in optionals)
                {
                    // alias: Only characters within the ranges [A-Z], [a-z] or [0-9] or _ are allowed.
                    // The first character may only be in the ranges [A-Z], [a-z] or _.
                    if (item.Key == "alias")
                        Debug.Assert(rgx.Match(item.Value).Success);
                    field.SetAttribute(item.Key, item.Value);
                }
            }
            return field;
        }

        public XmlElement AddFilter(XmlElement entity, string type = "and")
        {
            XmlElement filterElement = AddElement(entity, "filter");
            filterElement.SetAttribute("type", type);
            return filterElement;
        }

        /// <summary>
        /// Add a condition element to a filter element
        /// </summary>
        /// <param name="filterElement"></param>
        /// <param name="target"></param>
        /// <param name="op"></param>
        /// <param name="value"></param>
        public void AddCondition(XmlElement filterElement, string target, string op, string value = null)
        {
            XmlElement condElement = AddElement(filterElement, "condition");
            condElement.SetAttribute("attribute", target);
            condElement.SetAttribute("operator", op);
            if (!string.IsNullOrEmpty(value))
                condElement.SetAttribute("value", value);
        }

        /// <summary>
        /// Add a link entity to an entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="anotherEntity"></param>
        /// <param name="sourceAttribute"></param>
        /// <param name="targetAttribute"></param>
        /// <param name="optionals">can be anything from the list: link-type (inner, outer), alias, intersect, visible etc</param>
        /// <returns></returns>
        public XmlElement AddLinkEntity(XmlElement entity, string anotherEntity, string sourceAttribute, string targetAttribute, Dictionary<string, string> optionals = null)
        {
            XmlElement linkingElement = AddElement(entity, "link-entity");
            linkingElement.SetAttribute("name", anotherEntity);
            linkingElement.SetAttribute("from", sourceAttribute);
            linkingElement.SetAttribute("to", targetAttribute);
            if (optionals != null)
            {
                foreach (KeyValuePair<string, string> item in optionals)
                {
                    linkingElement.SetAttribute(item.Key, item.Value);
                }
            }
            return linkingElement;
        }
    }
}
