using System.Collections.Generic;
using System.Xml.Serialization;

namespace Model.XmlModel
{
    [XmlRoot(ElementName = "attribute")]
    public class Attribute
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "request")]
    public class Request
    {
        [XmlElement(ElementName = "attribute")]
        public List<Attribute> Attribute { get; set; }

        [XmlAttribute(AttributeName = "service")]
        public string Service { get; set; }

        [XmlAttribute(AttributeName = "function")]
        public Functions Function { get; set; }
    }

    public enum Functions
    {
        GetGoodsList,
        GetDescription,
        SendOrder,
        GetOrderConfirm,
        SetState,
        ConfirmOrder,
        GetOrderList,
        Authorization
    }
}