using System;


namespace BlogApp.Services
{//public interface IBlogEmailSender :IEmailSender
    public interface IBlogEmailSender 
	{
         Task SendContactEmailAsync(string emailFrom, string name, string subject, string htmlMessage);
         //Task SendEmailAsync(string emailFrom, string name, string subject, string htmlMessage);
         Task SendEmailAsync(string email, string v1, string v2);
    }
}

