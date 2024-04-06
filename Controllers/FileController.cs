using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Services;

namespace SocialNetwork.Controllers;

[Authorize]
public class FileController : Controller
{
    private readonly FileService _fileService;

    public FileController(FileService fileService)
    {
        _fileService = fileService;
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
        string? imageUrl = await _fileService.SaveImageAsync(file, userId, dirName, existingName);
        if (imageUrl == null)
        {
            return BadRequest();
        }
        return Ok(imageUrl);
    }
    
}