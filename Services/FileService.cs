using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;

namespace SocialNetwork.Services;

public class FileService
{
    private readonly string _targetFolder; //absolute folder
    private readonly int _maxFileSize = 5 * 1024 * 1024; // 5MB
    private readonly string _contentUrl; //url to content folder

    public FileService(IServer server, IWebHostEnvironment env)
    {
        _targetFolder = Path.Combine(env.WebRootPath, "file-storage");
        _contentUrl = server.Features.Get<IServerAddressesFeature>()?.Addresses.First() + "/file-storage";
    }

    public async Task<string> SaveImageAsync(IFormFile file, string userId, string dirName, string? existingName = null)
    {
        if (IsTooLarge(file))
        {
            throw new Exception("File size exceeded. The maximum allowed size is 5MB.");
        }
        if (!IsImage(file))
        {
            throw new Exception("Invalid file type. Only JPEG, PNG, and GIF images are allowed.");
        }

        if (existingName != null && !File.Exists(Path.Combine(_targetFolder, dirName, userId, existingName)))
        {
            existingName = null;
        }

        string fileExtension = GetImageFormat(file);
        string uniqueFileName = existingName ?? $"{Guid.NewGuid()}{fileExtension}";
        string filePath = Path.Combine(_targetFolder, dirName, userId, uniqueFileName);
        string directoryPath = Path.Combine(_targetFolder, dirName, userId);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        else if (existingName != null)
        {
            string oldFilePath = Path.Combine(directoryPath, existingName);
            if (File.Exists(oldFilePath))
            {
                File.Delete(oldFilePath);
            }
        }

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }

        string relativeFilePath = Path.GetRelativePath(_targetFolder, filePath);
        return relativeFilePath;
    }

    public string? GetImageFullUrl(string? url)
    {
        if (url == null)
            return url;
        return Path.Combine(_contentUrl, url);
    }

    private bool IsImage(IFormFile file)
    {
        try
        {
            Image.Identify(file.OpenReadStream());
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private string GetImageFormat(IFormFile file)
    {
        var format = Image.DetectFormat(file.OpenReadStream());
        return $".{format.Name.ToLower()}";
    }

    private bool IsTooLarge(IFormFile file)
    {
        return file.Length > _maxFileSize;
    }
}