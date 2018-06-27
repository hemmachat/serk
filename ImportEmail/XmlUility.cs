using ImportEmail.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ImportEmail
{
    public class XmlUility : IXmlUtility
    {
        public bool CheckXmlTags(string emailText)
        {
            CheckTotalTag(emailText);

            string[] tags = { "expense", "cost_centre", "total", "payment_method", "vendor", "description", "date" };

            // check for the pair of openning/closing tags
            foreach (var tag in tags)
            {
                Regex regex = new Regex("<" + tag + @">[\S\s]*<\/" + tag + ">");

                if (!regex.IsMatch(emailText))
                {
                    throw new Exception($"No matching XML tag: '{tag}'.");
                }
            }

            return true;
        }

        private void CheckTotalTag(string emailText)
        {
            if (emailText.IndexOf("<total>") == -1)
            {
                throw new Exception("Missing '<total>' XML tag.");
            }
        }
    }
}