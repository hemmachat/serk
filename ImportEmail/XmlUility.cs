using ImportEmail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;

namespace ImportEmail
{
    public class XmlUility : IXmlUtility
    {
        private readonly string[] parentTags = { "expense", "vendor", "description", "date" };
        private readonly string[] expenseTags = { "cost_centre", "total", "payment_method" };
        private const string MID_XML_CONTENT = @">[\S\s]*<\/";
        private const string EXPENSE_TEXT = "expense";
        private const string COST_CENTRE_TEXT = "cost_centre";
        private const string REQUEST_TEXT = "request";

        public bool HasPairTags(string emailText)
        {
            // check for the pair of openning/closing tags
            var allTags = parentTags.Concat(expenseTags);

            foreach (var tag in allTags)
            {
                Regex openCloseRegex = new Regex(@"<(/)?" + tag + ">");
                Regex pairRegex = new Regex("<" + tag + MID_XML_CONTENT + tag + ">");

                if (openCloseRegex.IsMatch(emailText) && !pairRegex.IsMatch(emailText))
                {
                    throw new Exception($"No matching XML tag: '{tag}'.");
                }
            }

            return true;
        }

        public bool HasTotalTag(string emailText)
        {
            if (emailText.IndexOf("<total>") == -1)
            {
                throw new Exception("Missing '<total>' XML tag.");
            }

            return true;
        }

        public XDocument ExtractXmlValues(string emailText)
        {
            var expenseText = "";
            XDocument xdoc = new XDocument(new XElement(REQUEST_TEXT));

            // parents
            foreach (var tag in parentTags)
            {
                Regex pairRegex = new Regex("<" + tag + MID_XML_CONTENT + tag + ">");

                if (pairRegex.IsMatch(emailText))
                {
                    var content = pairRegex.Match(emailText).Value;

                    if (tag == EXPENSE_TEXT)
                    {
                        expenseText = content;
                    }
                    else
                    {
                        var element = XElement.Parse(RemoveWhitespace(content));
                        xdoc.Root.Add(element);
                    }
                }
            }

            // children of expense
            xdoc.Root.Add(new XElement(EXPENSE_TEXT));

            foreach (var tag in expenseTags)
            {
                Regex pairRegex = new Regex("<" + tag + MID_XML_CONTENT + tag + ">");

                if (pairRegex.IsMatch(expenseText))
                {
                    var content = pairRegex.Match(expenseText).Value;
                    xdoc.Root.Element(EXPENSE_TEXT).Add(XElement.Parse(RemoveWhitespace(content)));
                }
            }

            UpdateCostCentre(xdoc);

            return xdoc;
        }

        private void UpdateCostCentre(XDocument xdoc)
        {
            if (xdoc.Root.Element(EXPENSE_TEXT).Element(COST_CENTRE_TEXT) == null)
            {
                xdoc.Root.Element(EXPENSE_TEXT).Add(new XElement(COST_CENTRE_TEXT, "UNKNOWN"));
            }
        }

        private string RemoveWhitespace(string text)
        {
            return Regex.Replace(Regex.Replace(text, @"\n|\r", ""), @"\t+|\s+", " ");
        }
    }
}