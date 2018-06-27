using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Text.RegularExpressions;
using ImportEmail.Interfaces;

namespace ImportEmail.Controllers
{
    public class ImportEmailController : ApiController
    {
        private IXmlUtility _xml;

        public ImportEmailController(IXmlUtility xml)
        {
            _xml = xml;
        }

        [HttpGet]
        public void Import(string emailText)
        {
            try
            {
                _xml.CheckOpenCloseTags(emailText);
            }
            catch (Exception ex)
            {

            }
        }


    }
}
