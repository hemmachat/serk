using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Text.RegularExpressions;
using ImportEmail.Interfaces;
using Newtonsoft.Json.Linq;

namespace ImportEmail.Controllers
{
    public class ImportEmailController : ApiController
    {
        private const string EMAIL_TEXT = "emailText";
        private readonly IXmlUtility _xml;

        public ImportEmailController(IXmlUtility xml)
        {
            _xml = xml;
        }

        [HttpGet]
        public IHttpActionResult ImportText()
        {
            return Ok("Please use POST.");
        }

        // using POST because we might have a data adding functionality in the future
        [HttpPost]
        public IHttpActionResult ImportText([FromBody] JObject body)
        {
            if (body == null || string.IsNullOrEmpty(body[EMAIL_TEXT].ToString()))
            {
                return BadRequest("Empty email text.");
            }

            try
            {
                var emailText = body["emailText"].ToString();
                var validTotal = _xml.HasTotalTag(emailText);
                var validPairs = _xml.HasPairTags(emailText);

                if (validTotal && validPairs)
                {
                    var xdoc = _xml.ExtractXmlValues(emailText);

                    // some adding functionality

                    return Json(xdoc);
                }

                return BadRequest("Unknown error.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
