using System;
namespace BlogApp.Services.ViewModels
{
	public class MailSettings
	{
		public MailSettings()
		{
		}
		//To configure and use smtp server from google
		public string Mail { get; set; }
		public string DisplayName { get; set; }
		public string Password { get; set; }
		public string Host { get; set; }
		public int Port { get; set; }
	}
}

