using System;
using System.Linq;
using Xunit;
using Moq;
using ImportEmail;

namespace ImportEmail.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Valid_Text()
        {
            var text = @"Hi Yvaine,
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
                    Ivan";

            var xml = new XmlUility();

            Assert.True(xml.CheckXmlTags(text));
        }

        [Fact]
        public void Missing_Total_Text()
        {
            var text = @"Hi Yvaine,
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
                    Ivan";

            var xml = new XmlUility();
            Exception ex = Assert.Throws<Exception>(() => xml.CheckXmlTags(text));

            Assert.Equal("Missing '<total>' XML tag.", ex.Message);
        }

        [Fact]
        public void Missing_Expense_Text()
        {
            var text = @"Hi Yvaine,
                    Please create an expense claim for the below. Relevant details are marked up as requested…
                    <cost_centre>DEV002</cost_centre> <total>890.55</total><payment_method>personal
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
                    Ivan";
            var xml = new XmlUility();
            Exception ex = Assert.Throws<Exception>(() => xml.CheckXmlTags(text));

            Assert.Equal($"No matching XML tag: 'expense'.", ex.Message);
        }

        [Fact]
        public void Missing_Closing_Expense_Text()
        {
            var text = @"Hi Yvaine,
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
                    Ivan";
            var xml = new XmlUility();
            Exception ex = Assert.Throws<Exception>(() => xml.CheckXmlTags(text));

            Assert.Equal($"No matching XML tag: 'expense'.", ex.Message);
        }

        [Fact]
        public void Missing_Openning_Expense_Text()
        {
            var text = @"Hi Yvaine,
                    Please create an expense claim for the below. Relevant details are marked up as requested…
                    <cost_centre>DEV002</cost_centre> <total>890.55</total><payment_method>personal
                    card</payment_method></expense>
                    From: Ivan Castle
                    Sent: Friday, 16 February 2018 10:32 AM
                    To: Antoine Lloyd <Antoine.Lloyd@example.com>
                    Subject: test
                    Hi Antoine,
                    Please create a reservation at the <vendor>Viaduct Steakhouse</vendor> our <description>development
                    team’s project end celebration dinner</description> on <date>Tuesday 27 April 2017</date>. We expect to
                    arrive around 7.15pm. Approximately 12 people but I’ll confirm exact numbers closer to the day.
                    Regards,
                    Ivan";
            var xml = new XmlUility();
            Exception ex = Assert.Throws<Exception>(() => xml.CheckXmlTags(text));

            Assert.Equal($"No matching XML tag: 'expense'.", ex.Message);
        }

        [Fact]
        public void Missing_Cost_Text()
        {
            var text = @"Hi Yvaine,
                    Please create an expense claim for the below. Relevant details are marked up as requested…
                    <expense>DEV002 <total>890.55</total><payment_method>personal
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
                    Ivan";
            var xml = new XmlUility();
            Exception ex = Assert.Throws<Exception>(() => xml.CheckXmlTags(text));

            Assert.Equal($"No matching XML tag: 'cost_centre'.", ex.Message);
        }

        [Fact]
        public void Missing_Closing_Cost_Text()
        {
            var text = @"Hi Yvaine,
                    Please create an expense claim for the below. Relevant details are marked up as requested…
                    <expense><cost_centre>DEV002 <total>890.55</total><payment_method>personal
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
                    Ivan";
            var xml = new XmlUility();
            Exception ex = Assert.Throws<Exception>(() => xml.CheckXmlTags(text));

            Assert.Equal($"No matching XML tag: 'cost_centre'.", ex.Message);
        }

        [Fact]
        public void Missing_Open_Cost_Text()
        {
            var text = @"Hi Yvaine,
                    Please create an expense claim for the below. Relevant details are marked up as requested…
                    <expense>DEV002</cost_centre> <total>890.55</total><payment_method>personal
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
                    Ivan";
            var xml = new XmlUility();
            Exception ex = Assert.Throws<Exception>(() => xml.CheckXmlTags(text));

            Assert.Equal($"No matching XML tag: 'cost_centre'.", ex.Message);
        }

        [Fact]
        public void Missing_Closing_Total_Text()
        {
            var text = @"Hi Yvaine,
                    Please create an expense claim for the below. Relevant details are marked up as requested…
                    <expense><cost_centre>DEV002</cost_centre> <total>890.55<payment_method>personal
                    card</payment_method></expense>
                    From: Ivan Castle
                    Sent: Friday, 16 February 2018 10:32 AM
                    To: Antoine Lloyd <Antoine.Lloyd@example.com>
                    Subject: test
                    Hi Antoine,
                    Please create a reservation at the <vendor>Viaduct Steakhouse</vendor> our <description>development
                    team’s project end celebration dinner</description> on <date>Tuesday 27 April 2017</date>. We expect to
                    arrive around 7.15pm. Approximately 12 people but I’ll confirm exact numbers closer to the day.
                    Regards,
                    Ivan";
            var xml = new XmlUility();
            Exception ex = Assert.Throws<Exception>(() => xml.CheckXmlTags(text));

            Assert.Equal($"No matching XML tag: 'total'.", ex.Message);
        }

        [Fact]
        public void Missing_Payment_Text()
        {
            var text = @"Hi Yvaine,
                    Please create an expense claim for the below. Relevant details are marked up as requested…
                    <expense><cost_centre>DEV002</cost_centre> <total>890.55</total>personal
                    card</expense>
                    From: Ivan Castle
                    Sent: Friday, 16 February 2018 10:32 AM
                    To: Antoine Lloyd <Antoine.Lloyd@example.com>
                    Subject: test
                    Hi Antoine,
                    Please create a reservation at the <vendor>Viaduct Steakhouse</vendor> our <description>development
                    team’s project end celebration dinner</description> on <date>Tuesday 27 April 2017</date>. We expect to
                    arrive around 7.15pm. Approximately 12 people but I’ll confirm exact numbers closer to the day.
                    Regards,
                    Ivan";
            var xml = new XmlUility();
            Exception ex = Assert.Throws<Exception>(() => xml.CheckXmlTags(text));

            Assert.Equal($"No matching XML tag: 'payment_method'.", ex.Message);
        }

        [Fact]
        public void Missing_Openning_Payment_Text()
        {
            var text = @"Hi Yvaine,
                    Please create an expense claim for the below. Relevant details are marked up as requested…
                    <expense><cost_centre>DEV002</cost_centre> <total>890.55</total>personal
                    card</payment_method></expense>
                    From: Ivan Castle
                    Sent: Friday, 16 February 2018 10:32 AM
                    To: Antoine Lloyd <Antoine.Lloyd@example.com>
                    Subject: test
                    Hi Antoine,
                    Please create a reservation at the <vendor>Viaduct Steakhouse</vendor> our <description>development
                    team’s project end celebration dinner</description> on <date>Tuesday 27 April 2017</date>. We expect to
                    arrive around 7.15pm. Approximately 12 people but I’ll confirm exact numbers closer to the day.
                    Regards,
                    Ivan";
            var xml = new XmlUility();
            Exception ex = Assert.Throws<Exception>(() => xml.CheckXmlTags(text));

            Assert.Equal($"No matching XML tag: 'payment_method'.", ex.Message);
        }

        [Fact]
        public void Missing_Closing_Payment_Text()
        {
            var text = @"Hi Yvaine,
                    Please create an expense claim for the below. Relevant details are marked up as requested…
                    <expense><cost_centre>DEV002</cost_centre> <total>890.55</total><payment_method>personal
                    card</expense>
                    From: Ivan Castle
                    Sent: Friday, 16 February 2018 10:32 AM
                    To: Antoine Lloyd <Antoine.Lloyd@example.com>
                    Subject: test
                    Hi Antoine,
                    Please create a reservation at the <vendor>Viaduct Steakhouse</vendor> our <description>development
                    team’s project end celebration dinner</description> on <date>Tuesday 27 April 2017</date>. We expect to
                    arrive around 7.15pm. Approximately 12 people but I’ll confirm exact numbers closer to the day.
                    Regards,
                    Ivan";
            var xml = new XmlUility();
            Exception ex = Assert.Throws<Exception>(() => xml.CheckXmlTags(text));

            Assert.Equal($"No matching XML tag: 'payment_method'.", ex.Message);
        }

        [Fact]
        public void Missing_Vendor_Text()
        {
            var text = @"Hi Yvaine,
                    Please create an expense claim for the below. Relevant details are marked up as requested…
                    <expense><cost_centre>DEV002</cost_centre> <total>890.55</total><payment_method>personal
                    card</payment_method></expense>
                    From: Ivan Castle
                    Sent: Friday, 16 February 2018 10:32 AM
                    To: Antoine Lloyd <Antoine.Lloyd@example.com>
                    Subject: test
                    Hi Antoine,
                    Please create a reservation at the Viaduct Steakhouse our <description>development
                    team’s project end celebration dinner</description> on <date>Tuesday 27 April 2017</date>. We expect to
                    arrive around 7.15pm. Approximately 12 people but I’ll confirm exact numbers closer to the day.
                    Regards,
                    Ivan";
            var xml = new XmlUility();
            Exception ex = Assert.Throws<Exception>(() => xml.CheckXmlTags(text));

            Assert.Equal($"No matching XML tag: 'vendor'.", ex.Message);
        }

        [Fact]
        public void Missing_Openning_Vendor_Text()
        {
            var text = @"Hi Yvaine,
                    Please create an expense claim for the below. Relevant details are marked up as requested…
                    <expense><cost_centre>DEV002</cost_centre> <total>890.55</total><payment_method>personal
                    card</payment_method></expense>
                    From: Ivan Castle
                    Sent: Friday, 16 February 2018 10:32 AM
                    To: Antoine Lloyd <Antoine.Lloyd@example.com>
                    Subject: test
                    Hi Antoine,
                    Please create a reservation at the Viaduct Steakhouse</vendor> our <description>development
                    team’s project end celebration dinner</description> on <date>Tuesday 27 April 2017</date>. We expect to
                    arrive around 7.15pm. Approximately 12 people but I’ll confirm exact numbers closer to the day.
                    Regards,
                    Ivan";
            var xml = new XmlUility();
            Exception ex = Assert.Throws<Exception>(() => xml.CheckXmlTags(text));

            Assert.Equal($"No matching XML tag: 'vendor'.", ex.Message);
        }

        [Fact]
        public void Missing_Closing_Vendor_Text()
        {
            var text = @"Hi Yvaine,
                    Please create an expense claim for the below. Relevant details are marked up as requested…
                    <expense><cost_centre>DEV002</cost_centre> <total>890.55</total><payment_method>personal
                    card</payment_method></expense>
                    From: Ivan Castle
                    Sent: Friday, 16 February 2018 10:32 AM
                    To: Antoine Lloyd <Antoine.Lloyd@example.com>
                    Subject: test
                    Hi Antoine,
                    Please create a reservation at the </vendor>Viaduct Steakhouse our <description>development
                    team’s project end celebration dinner</description> on <date>Tuesday 27 April 2017</date>. We expect to
                    arrive around 7.15pm. Approximately 12 people but I’ll confirm exact numbers closer to the day.
                    Regards,
                    Ivan";
            var xml = new XmlUility();
            Exception ex = Assert.Throws<Exception>(() => xml.CheckXmlTags(text));

            Assert.Equal($"No matching XML tag: 'vendor'.", ex.Message);
        }

        [Fact]
        public void Missing_Description_Text()
        {
            var text = @"Hi Yvaine,
                    Please create an expense claim for the below. Relevant details are marked up as requested…
                    <expense><cost_centre>DEV002</cost_centre> <total>890.55</total><payment_method>personal
                    card</payment_method>
                    </expense>
                    From: Ivan Castle
                    Sent: Friday, 16 February 2018 10:32 AM
                    To: Antoine Lloyd <Antoine.Lloyd@example.com>
                    Subject: test
                    Hi Antoine,
                    Please create a reservation at the <vendor>Viaduct Steakhouse</vendor> our development
                    team’s project end celebration dinner on <date>Tuesday 27 April 2017</date>. We expect to
                    arrive around 7.15pm. Approximately 12 people but I’ll confirm exact numbers closer to the day.
                    Regards,
                    Ivan";

            var xml = new XmlUility();
            Exception ex = Assert.Throws<Exception>(() => xml.CheckXmlTags(text));

            Assert.Equal($"No matching XML tag: 'description'.", ex.Message);
        }

        [Fact]
        public void Missing_Openning_Description_Text()
        {
            var text = @"Hi Yvaine,
                    Please create an expense claim for the below. Relevant details are marked up as requested…
                    <expense><cost_centre>DEV002</cost_centre> <total>890.55</total><payment_method>personal
                    card</payment_method>
                    </expense>
                    From: Ivan Castle
                    Sent: Friday, 16 February 2018 10:32 AM
                    To: Antoine Lloyd <Antoine.Lloyd@example.com>
                    Subject: test
                    Hi Antoine,
                    Please create a reservation at the <vendor>Viaduct Steakhouse</vendor> our development
                    team’s project end celebration dinner</description> on <date>Tuesday 27 April 2017</date>. We expect to
                    arrive around 7.15pm. Approximately 12 people but I’ll confirm exact numbers closer to the day.
                    Regards,
                    Ivan";

            var xml = new XmlUility();
            Exception ex = Assert.Throws<Exception>(() => xml.CheckXmlTags(text));

            Assert.Equal($"No matching XML tag: 'description'.", ex.Message);
        }

        [Fact]
        public void Missing_Closing_Description_Text()
        {
            var text = @"Hi Yvaine,
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
                    team’s project end celebration dinner on <date>Tuesday 27 April 2017</date>. We expect to
                    arrive around 7.15pm. Approximately 12 people but I’ll confirm exact numbers closer to the day.
                    Regards,
                    Ivan";

            var xml = new XmlUility();
            Exception ex = Assert.Throws<Exception>(() => xml.CheckXmlTags(text));

            Assert.Equal($"No matching XML tag: 'description'.", ex.Message);
        }

        [Fact]
        public void Missing_Date_Text()
        {
            var text = @"Hi Yvaine,
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
                    team’s project end celebration dinner</description> on Tuesday 27 April 2017. We expect to
                    arrive around 7.15pm. Approximately 12 people but I’ll confirm exact numbers closer to the day.
                    Regards,
                    Ivan";

            var xml = new XmlUility();
            Exception ex = Assert.Throws<Exception>(() => xml.CheckXmlTags(text));

            Assert.Equal($"No matching XML tag: 'date'.", ex.Message);
        }

        [Fact]
        public void Missing_Openning_Date_Text()
        {
            var text = @"Hi Yvaine,
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
                    team’s project end celebration dinner</description> on Tuesday 27 April 2017</date>. We expect to
                    arrive around 7.15pm. Approximately 12 people but I’ll confirm exact numbers closer to the day.
                    Regards,
                    Ivan";

            var xml = new XmlUility();
            Exception ex = Assert.Throws<Exception>(() => xml.CheckXmlTags(text));

            Assert.Equal($"No matching XML tag: 'date'.", ex.Message);
        }

        [Fact]
        public void Missing_Closing_Date_Text()
        {
            var text = @"Hi Yvaine,
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
                    team’s project end celebration dinner</description> on <date>Tuesday 27 April 2017. We expect to
                    arrive around 7.15pm. Approximately 12 people but I’ll confirm exact numbers closer to the day.
                    Regards,
                    Ivan";

            var xml = new XmlUility();
            Exception ex = Assert.Throws<Exception>(() => xml.CheckXmlTags(text));

            Assert.Equal($"No matching XML tag: 'date'.", ex.Message);
        }

    }
}
