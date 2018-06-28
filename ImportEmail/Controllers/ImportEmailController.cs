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
        private readonly IXmlUtility _xml;

        public ImportEmailController(IXmlUtility xml)
        {
            _xml = xml;
        }

        [Route("ImportText")]
        public IHttpActionResult ImportText(string emailText)
        {
            try
            {
                var validTotal = _xml.HasTotalTag(emailText);
                var validPairs = _xml.HasPairTags(emailText);

                if (validTotal && validPairs)
                {
                    var xdoc = _xml.ExtractXmlValues(emailText);

                    return Json(xdoc);
                }

                return BadRequest("Invalid email text.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
