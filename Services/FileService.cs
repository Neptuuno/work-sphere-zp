using Microsoft.AspNetCore.Mvc;

namespace SocialNetwork.Services;

public class FileService
{
    private readonly string _targetFolder;
    private readonly int _maxFileSize = 5 * 1024 * 1024; // 5MB

    public FileService(string targetFolder)
    {
        _targetFolder = targetFolder;
    }

    public async Task<string> SaveImageAsync(IFormFile file, string userId, string dirName, string? existingName = null)
    {
        if (!IsImage(file))
        {
            throw new Exception("Invalid file type. Only JPEG, PNG, and GIF images are allowed.");
        }

        if (IsTooLarge(file))
        {
            throw new Exception("File size exceeded. The maximum allowed size is 5MB.");
        }

        string fileExtension = Path.GetExtension(file.FileName);
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

    private bool IsImage(IFormFile file)
    {
        var allowedContentTypes = new List<string> { "image/jpeg", "image/png", "image/gif" };
        return allowedContentTypes.Contains(file.ContentType);
    }

    private bool IsTooLarge(IFormFile file)
    {
        return file.Length > _maxFileSize;
    }
}