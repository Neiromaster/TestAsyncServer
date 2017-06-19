using System.Collections.Generic;
using System.Xml.Serialization;

namespace Model.XmlModel
{
    [XmlRoot(ElementName = "input")]
    public class Input
    {
        [XmlAttribute(AttributeName = "key")]
        public string Key { get; set; }

        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "nested")]
    public class Nested
    {
        [XmlElement(ElementName = "data")]
        public List<Data> Data { get; set; }

        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }

    [XmlRoot(ElementName = "data")]
    public class Data
    {
        [XmlElement(ElementName = "input")]
        public List<Input> Input { get; set; }

        [XmlElement(ElementName = "nested")]
        public Nested Nested { get; set; }
    }

    [XmlRoot(ElementName = "result")]
    public class Result
    {
        [XmlElement(ElementName = "data")]
        public Data Data { get; set; }

        [XmlAttribute(AttributeName = "code")]
        public int Code { get; set; }
    }
}