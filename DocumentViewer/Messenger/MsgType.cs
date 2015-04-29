using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;



namespace DocumentViewer
{
    [Serializable, XmlRoot("MsgType")]
    public class AttributesList 
    {
        [XmlElement("MsgType")]
        public List<MsgType> MsgType { get; set; }
    }

    [Serializable]
    public class MsgType
    {
        [XmlAttribute]
        public string Key { get; set; }

        [XmlAttribute]
        public string Value { get; set; }
    }

}
