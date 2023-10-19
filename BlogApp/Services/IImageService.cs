using System;
namespace BlogApp.Services
{
	public interface IImageService
	{
		Task<byte[]> EncodeImageAsync(IFormFile file);
		Task<byte[]> EncodeImageAsync(string filename);
		string DecodeImage(byte[] data, string type);
		string ContentType(IFormFile file);
		int Size(IFormFile file);



    }
}

