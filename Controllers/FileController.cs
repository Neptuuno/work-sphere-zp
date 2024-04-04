using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Services;

namespace SocialNetwork.Controllers;

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
    
}