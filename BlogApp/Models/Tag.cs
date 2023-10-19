using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Models
{
	public class Tag
	{
	

        public int ID { get; set; }
        public int? PostId { get; set; }
        public string? BlogUserId { get; set; }

        [Required]
        [StringLength(25, ErrorMessage = "The {0} must be at least {2} and at most {1} characters", MinimumLength = 2)]
        public string? Text { get; set; }

        public virtual Post? Post { get; set; }
        public virtual BlogUser? BlogUser { get; set; }
    }
}

