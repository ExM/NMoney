using System.Xml.Serialization;

namespace NMoney.SourceCodeRenderer;

public class XmlCurrencyEntry
{
    [XmlElement("CtryNm")]
    public string? Country { get; set; }

    [XmlElement("Ccy")]
    public string? Code { get; set; }

    [XmlElement("CcyNbr")]
    public int? NumCode { get; set; }

    [XmlElement("CcyNm")]
    public string? Name { get; set; }

    [XmlElement("CcyMnrUnts")]
    public string? MinorUnit { get; set; }
}