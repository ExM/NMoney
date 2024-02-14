using System.Xml.Serialization;

namespace NMoney.SourceCodeRenderer;

[XmlRoot("ISO_4217")]
public class XmlCurrencyEntries
{
    [XmlAttribute("Pblshd")]
    public string? PublishDate {get; set;}

    [XmlArray("CcyTbl"), XmlArrayItem("CcyNtry")]
    public List<XmlCurrencyEntry>? List { get; set; }
}