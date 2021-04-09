using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SafeAdmin.Data;
using Amazon.SimpleEmail.Model;
using Amazon.SimpleEmail;
using Microsoft.Extensions.Options;
using SafeAdmin.Utility;

namespace SafeAdmin.Services
{
    public interface ISendMail
    {
        Task<int> SendSignUpEmailAsync(string email, string key, string template);
    }

    public class SendMail : ISendMail
    {
        private readonly ILogger<SendMail> _logger;
        private readonly SafeContext _context;
        private readonly IOptions<AppConfig> _config;

        public SendMail(ILogger<SendMail> logger, SafeContext context, IOptions<AppConfig> config)
        {
            _logger = logger;
            _context = context;
            _config = config;
        }

        public Task<int> SendSignUpEmailAsync(string email, string key, string template)
        {
            _logger.LogInformation("Sending email to {email} with key {key} using template {template}", email, key, template);
            var t = _context.MailTemplate.FirstOrDefault(m => m.TemplateName == template);

            // should be replaced with email address we are going to use for signup
            string from = _config.Value.EmailSender;

            if (t == null)
            {
                return Task.FromResult(-1);
            }

            string subject_t = t.Subject;
            string body_t = t.Body;

            List<KeyValuePair<string, string>> tags = new List<KeyValuePair<string, string>>();
            //handle base url as parameter
            tags.Add(new KeyValuePair<string, string>("$LINK$", _config.Value.SiteUrl + "registration/" + key));

            body_t = ChangeTags(body_t, tags);

            // Construct an object to contain the recipient address.
            Destination destination = new Destination();
            destination.ToAddresses = (new List<string>() { email });

            // Create the subject and body of the message.
            Content subject = new Content(subject_t);
            Content textBody = new Content(body_t);
            Body body = new Body();
            body.Html = textBody;
            // Create a message with the specified subject and body.
            Message message = new Message(subject, body);
            
            // Assemble the email.
            SendEmailRequest request = new SendEmailRequest(from, destination, message);
            

            // Choose the AWS region of the Amazon SES endpoint you want to connect to. Note that your sandbox 
            // status, sending limits, and Amazon SES identity-related settings are specific to a given 
            // AWS region, so be sure to select an AWS region in which you set up Amazon SES. Here, we are using 
            // the US West (Oregon) region. Examples of other regions that Amazon SES supports are USEast1 
            // and EUWest1. For a complete list, see http://docs.aws.amazon.com/ses/latest/DeveloperGuide/regions.html 
            Amazon.RegionEndpoint REGION = Amazon.RegionEndpoint.USWest2;

            // Instantiate an Amazon SES client, which will make the service call.
            AmazonSimpleEmailServiceClient client;
            try
            {
                client = new AmazonSimpleEmailServiceClient(REGION);
            }
            catch (Exception exc)
            {
                _logger.LogInformation(exc.Message);
                throw;
            }
            
            _logger.LogInformation("LASLO-AFTER CLIENT");
            // Send the email.
            try
            {
                client.SendEmailAsync(request);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return Task.FromResult(-2);
            }

            return Task.FromResult(0);
        }


        private string ChangeTags(string content, List<KeyValuePair<string,string>> tags)
        {
            foreach (KeyValuePair<string, string> tag in tags)
            {
                content = content.Replace(tag.Key, tag.Value);
            }
            return content;
        }    

    }
}
