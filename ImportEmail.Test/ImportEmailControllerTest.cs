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
using Newtonsoft.Json;
using ImportEmail.Models;
using System.Net;

namespace ImportEmail.Test
{
    public class ImportEmailControllerTest : WebApiTestBase
    {
        private const string URL = "/api/importemail/importtext";

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
            var response = CreateRequest(URL, HttpMethod.Post, json);
            var result = response.Content.ReadAsStringAsync().Result;
            var message = JsonConvert.DeserializeObject<XmlRoot>(result).Request;

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("DEV002", message.Expense.CostCentre);
            Assert.Equal(890.55m, message.Expense.Total);
            Assert.Equal("personal card", message.Expense.PaymentMethod);
            Assert.Equal("Viaduct Steakhouse", message.Vendor);
            Assert.Equal("development team’s project end celebration dinner", message.Description);
            Assert.Equal(new DateTime(2017, 4, 27), message.Date);
        }

        [Fact]
        public void Valid_Text_No_Cost_Centre()
        {
            var json = @"{""emailText"":""Hi Yvaine,
                Please create an expense claim for the below. Relevant details are marked up as requested…
                <expense> <total>890.55</total><payment_method>personal
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
            var response = CreateRequest(URL, HttpMethod.Post, json);
            var result = response.Content.ReadAsStringAsync().Result;
            var message = JsonConvert.DeserializeObject<XmlRoot>(result).Request;

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("UNKNOWN", message.Expense.CostCentre);
            Assert.Equal(890.55m, message.Expense.Total);
            Assert.Equal("personal card", message.Expense.PaymentMethod);
            Assert.Equal("Viaduct Steakhouse", message.Vendor);
            Assert.Equal("development team’s project end celebration dinner", message.Description);
            Assert.Equal(new DateTime(2017, 4, 27), message.Date);
        }

        [Fact]
        public void Valid_Partial_No_Cost_Centre()
        {
            var json = @"{""emailText"":""Hi Yvaine,
                    Please create an expense claim for the below. Relevant details are marked up as requested…
                    <expense><total>890.55</total><payment_method>personal
                    card</payment_method>
                    </expense>""}";

            var response = CreateRequest(URL, HttpMethod.Post, json);
            var result = response.Content.ReadAsStringAsync().Result;
            var message = JsonConvert.DeserializeObject<XmlRoot>(result).Request;

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("UNKNOWN", message.Expense.CostCentre);
            Assert.Equal(890.55m, message.Expense.Total);
            Assert.Equal("personal card", message.Expense.PaymentMethod);
        }

        [Fact]
        public void Empty_Text()
        {
            var json = "";

            var response = CreateRequest(URL, HttpMethod.Post, json);
            var result = response.Content.ReadAsStringAsync().Result;
            var message = JsonConvert.DeserializeObject<ResponseMessage>(result).Message;

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("Empty email text.", message);
        }

        [Fact]
        public void Missing_Total_Text()
        {
            var json = @"{""emailText"":""Hi Yvaine,
                    Please create an expense claim for the below. Relevant details are marked up as requested…
                    <expense><cost_centre>DEV002</cost_centre> 890.55</total><payment_method>personal
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

            var response = CreateRequest(URL, HttpMethod.Post, json);
            var result = response.Content.ReadAsStringAsync().Result;
            var message = JsonConvert.DeserializeObject<ResponseMessage>(result).Message;

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("Missing '<total>' XML tag.", message);
        }

        [Fact]
        public void Missing_Closing_Expense_Text()
        {
            var json = @"{""emailText"":""Hi Yvaine,
                    Please create an expense claim for the below. Relevant details are marked up as requested…
                    <expense><cost_centre>DEV002</cost_centre> <total>890.55</total><payment_method>personal
                    card</payment_method>
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

            var response = CreateRequest(URL, HttpMethod.Post, json);
            var result = response.Content.ReadAsStringAsync().Result;
            var message = JsonConvert.DeserializeObject<ResponseMessage>(result).Message;

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("No matching XML tag: 'expense'.", message);
        }
    }
}
