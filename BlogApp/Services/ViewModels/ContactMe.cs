using System;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Services.ViewModels
{
	public class ContactMe
	{
		public ContactMe()
		{
		}

        [Required]
        [StringLength(80, ErrorMessage = "The {0} must be at least {2} and at most {1} characters", MinimumLength = 2)]
        public string Name { get; set; }

		[Required]
        [EmailAddress]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at most {1} characters", MinimumLength = 2)]
        public string Email { get; set; }

		[Phone]
		public string Phone { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at most {1} characters", MinimumLength = 2)]
        public string Subject { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} and at most {1} characters", MinimumLength = 2)]
        public string Message { get; set; }
	}
}

