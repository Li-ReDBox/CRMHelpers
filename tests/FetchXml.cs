using System.Xml;
using Xunit;
using Client;

namespace tests
{
    public class FetchXmlTest
    {
        [Fact]
        public void CanProduceXml()
        {
            const string result = @"<fetch version='1.0' mapping='logical'><entity name='test'><attribute name='name' /><attribute name='description' /></entity></fetch>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(result);

                    var xml = new FetchXML("test");
            FetchElement entity = xml.EntityElement;
            entity.AddField("name")
                .AddField("description");

            Assert.Equal("?fetchXml=" + doc.OuterXml, xml.ToQueryString());
        }
    }
}
