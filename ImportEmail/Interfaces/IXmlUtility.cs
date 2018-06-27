using System.Collections.Generic;
using System.Xml.Linq;

namespace ImportEmail.Interfaces
{
    public interface IXmlUtility
    {
        XDocument ExtractXmlValues(string emailText);
        bool HasTotalTag(string emailText);
        bool HasPairTags(string emailText);
    }
}