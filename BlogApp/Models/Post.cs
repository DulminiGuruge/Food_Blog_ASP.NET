using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BlogApp.Enum;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Models
{
	public class Post
	{
		
		public int Id { get; set; }
		[Display(Name ="Blog Name")]
		public int BlogId { get; set; }
		public string? BlogUserId { get; set; }

		[Required]
		[StringLength(75, ErrorMessage ="The {0} must be at least {2} and no more than {1} characters long",MinimumLength = 2)]
		public string Title { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} and no more than {1} characters long", MinimumLength = 2)]
        public string Abstract { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} and no more than {1} characters long", MinimumLength = 2)]
		public string Content { get; set; }

		[DataType(DataType.Date)]
		[Display(Name ="Created Date")]
		public DateTime? Created { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Updated Date")]
        public DateTime? Updated { get; set; }


		public ReadyStatus ReadyStaus { get; set; }

		public string? Slug { get; set; }

        [Display(Name = "Post Image")]
        public byte[]? ImageData { get; set; } // type byte array to store image

        [Display(Name = "Image Type")]
        public string? ContentType { get; set; }
      

		[NotMapped]//property excluded from database mapping
		public IFormFile? Image { get; set; }

		//navigation property
		public virtual Blog? Blog { get; set; }
		public virtual BlogUser? BlogUser { get; set; }

		public virtual ICollection<Tag>? Tags { get; set; } = new HashSet<Tag>();
		public virtual ICollection<Comment>? Comments { get; set; } = new HashSet<Comment>();


    }
}

