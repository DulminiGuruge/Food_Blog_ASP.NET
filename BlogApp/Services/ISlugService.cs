using System;
namespace BlogApp.Services
{
	public interface ISlugService
	{
		string UrlFrienly(string title);

		bool IsUnique(string slug);
	}
}

