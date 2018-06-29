using ImportEmail.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.SelfHost;
using Xunit;
using Moq;

namespace ImportEmail.Test
{
    public class ImportEmailControllerTest : WebApiTestBase
    {
        public ImportEmailControllerTest() : base(typeof(ImportEmailController)) { }

        [Fact]
        public void Valid_Text()
        {
            var json = @"{""emailText"":""Hi Yvaine,
                Please create an expense claim for the below. Relevant details are marked up as requested…
                <expense><cost_centre>DEV002</cost_centre> <total>890.55</total><payment_method>personal
                card</payment_method>
                </expense>
                From: Ivan Castle
                Sent: Friday, 16 February 2018 10:32 AM
                To: Antoine Lloyd <Antoine.Lloyd@example.com>
                Subject: test
                Hi Antoine,
                Please create a reservation at the <vendor>Viaduct Steakhouse</vendor> our <description>development
                team’s project end celebration dinner</description> on <date>Tuesday 27 April 2017</date>. We expect to
                arrive around 7.15pm. Approximately 12 people but I’ll confirm exact numbers closer to the day.
                Regards,
                Ivan""}";
            var response = CreateRequest("/api/importemail/importtext", HttpMethod.Post, json);
            var result = response.Content.ReadAsStringAsync().Result;
        }
    }
}
