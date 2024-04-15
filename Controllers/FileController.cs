using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Services;

namespace SocialNetwork.Controllers;

[Authorize]
public class FileController : Controller
{
    private readonly FileService _fileService;
    private readonly ChatService _chatService;

    public FileController(FileService fileService, ChatService chatService)
    {
        _fileService = fileService;
        _chatService = chatService;
    }

    public IActionResult GetImageFullUrl(string url)
    {
        string? imageUrl = _fileService.GetImageFullUrl(url);
        if (imageUrl == null)
        {
            return NotFound();
        }
        return Ok(imageUrl);
    }
    
    [HttpPost]
    public async Task<IActionResult> UploadImage([FromForm] IFormFile file, string userId, string dirName, string? existingName = null)
    {
        try
        {
            string? imageUrl = await _fileService.SaveImageAsync(file, userId, dirName, existingName);
            return Ok(imageUrl);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e);
        }
    }
    
    [HttpDelete]
    public IActionResult DeleteImage(string imageUrl)
    {
        try
        {
            _fileService.DeleteImage(imageUrl);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e);
        }
    }
}