using System;
using BlogApp.Models;

namespace BlogApp.Services.ViewModels
{
	public class PostDetailViewModel
	{
		public PostDetailViewModel()
		{
		}
		public Post Post { get; set; }
		public List<string> Tags { get; set; } = new List<string>();
	}
}

