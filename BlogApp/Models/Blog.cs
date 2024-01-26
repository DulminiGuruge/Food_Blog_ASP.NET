using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Models
{
	public class Blog
	{
		public Blog()
		{
		}

		public int ID { get; set; }
		public string? BlogUserId { get; set; }

		[Required]
		[StringLength(100,ErrorMessage = "The {0} must be at least {2} and at most {1} characters",MinimumLength = 2)]
		public string Name { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} and at most {1} characters", MinimumLength = 2)]
        public string Description { get; set; }

		[DataType(DataType.Date)]
		[Display(Name = "Created Date")]
		public DateTime? Created { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Updated Date")]
        public DateTime? Updated { get; set; }//nullable datetime field to see when updtaed


        [Display(Name = "Blog Image")]
        public byte[]? ImageData { get; set; } // type byte array to store image

        [Display(Name = "Image Type")]
        public string? ConentType { get; set; }

		[NotMapped]
		public IFormFile? Image { get; set; } //imgage data and the content type both get data from this file,should be excluded from the database
		

		//navigation property
		public virtual BlogUser? BlogUser { get; set; }
		public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();


	}
}

