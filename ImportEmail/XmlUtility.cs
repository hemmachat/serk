using ImportEmail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;

namespace ImportEmail
{
    public class XmlUtility : IXmlUtility
    {
        private const string DATE_TEXT = "date";
        private readonly string[] parentTags = { EXPENSE_TEXT, "vendor", "description", DATE_TEXT };
        private const string TOTAL_TEXT = "total";
        private readonly string[] expenseTags = { COST_CENTRE_TEXT, TOTAL_TEXT, "payment_method" };
        private const string MID_XML_CONTENT = @">[\S\s]*<\/";
        private const string EXPENSE_TEXT = "expense";
        private const string COST_CENTRE_TEXT = "cost_centre";
        private const string REQUEST_TEXT = "request";

        public bool HasPairTags(string emailText)
        {
            var allTags = parentTags.Concat(expenseTags);

            foreach (var tag in allTags)
            {
                Regex openCloseRegex = new Regex(@"<(/)?" + tag + ">");
                Regex pairRegex = new Regex("<" + tag + MID_XML_CONTENT + tag + ">");

                if (openCloseRegex.IsMatch(emailText) && !pairRegex.IsMatch(emailText))
                {
                    throw new ApiException($"No matching XML tag: '{tag}'.");
                }
            }

            return true;
        }

        public bool HasTotalTag(string emailText)
        {
            if (emailText.IndexOf($"<{TOTAL_TEXT}>") == -1)
            {
                throw new ApiException($"Missing '<{TOTAL_TEXT}>' XML tag.");
            }

            return true;
        }

        public XDocument ExtractXmlValues(string emailText)
        {
            XDocument xdoc = new XDocument(new XElement(REQUEST_TEXT));
            ExtractParents(emailText, xdoc);
            var expenseNode = ExtractExpenseNode(emailText);
            ExtractExpenseChildren(expenseNode, xdoc);
            UpdateCostCentre(xdoc);

            return xdoc;
        }

        private void ExtractExpenseChildren(string expenseText, XDocument xdoc)
        {
            xdoc.Root.Add(new XElement(EXPENSE_TEXT));

            foreach (var tag in expenseTags)
            {
                Regex pairRegex = new Regex($"<{tag}{MID_XML_CONTENT}{tag}>");

                if (pairRegex.IsMatch(expenseText))
                {
                    var content = pairRegex.Match(expenseText).Value;
                    var totalString = content;

                    if (tag == TOTAL_TEXT)
                    {
                        totalString = ParseTotalText(content);
                    }

                    xdoc.Root.Element(EXPENSE_TEXT).Add(XElement.Parse(RemoveWhitespace(totalString)));
                }
            }
        }

        private string ExtractExpenseNode(string emailText)
        {
            Regex expenseText = new Regex($"<{EXPENSE_TEXT}{MID_XML_CONTENT}{EXPENSE_TEXT}>");

            return expenseText.Match(emailText).Value;
        }

        private void ExtractParents(string emailText, XDocument xdoc)
        {
            foreach (var tag in parentTags)
            {
                // skip expense node as we have another function to handle it
                if (tag == EXPENSE_TEXT)
                {
                    continue;
                }

                Regex pairRegex = new Regex($"<{tag}{MID_XML_CONTENT}{tag}>");

                if (pairRegex.IsMatch(emailText))
                {
                    var content = pairRegex.Match(emailText).Value;
                    var dateString = content;

                    if (tag == DATE_TEXT)
                    {
                        dateString = ParseDateText(content);
                    }

                    xdoc.Root.Add(XElement.Parse(RemoveWhitespace(dateString)));
                }
            }
        }


        private string ParseTotalText(string totalNode)
        {
            var totalString = totalNode.Replace($"<{TOTAL_TEXT}>", "").Replace($"</{TOTAL_TEXT}>", "");
            decimal val;

            if (decimal.TryParse(totalString, out val))
            {
                return totalNode;
            }

            throw new ApiException($"Invalid total: '{totalString}'.");
        }

        private string ParseDateText(string dateNode)
        {
            DateTime val;
            var dateString = dateNode.Replace($"<{DATE_TEXT}>", "").Replace($"</{DATE_TEXT}>", "");
            var cleanDateString = RemoveDayOfTheWeek(dateString);

            if (DateTime.TryParse(cleanDateString, out val))
            {
                return $"<{DATE_TEXT}>{val.ToString("yyyy-MM-dd HH:mm:ss")}</{DATE_TEXT}>";
            }

            throw new ApiException($"Invalid date: '{dateString}'.");
        }

        private string RemoveDayOfTheWeek(string dateString)
        {
            string[] dayOfWeek = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            var cleanDateString = "";

            // remove day of week text
            foreach (var day in dayOfWeek)
            {
                var regex = new Regex(@"\s*" + day.ToLower() + @"\s*");

                if (regex.IsMatch(dateString.ToLower()))
                {
                    cleanDateString = regex.Replace(dateString.ToLower(), "");

                    break;
                }
            }

            return cleanDateString;
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