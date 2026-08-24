namespace AucaJobPortalDotNet.Helpers;

public static class FileUploadHelper
{
    private static readonly string[] AllowedExtensions = { ".pdf", ".doc", ".docx" };
    private static readonly long MaxFileSize = 5 * 1024 * 1024; //5MB

    public static async Task<string> SaveFileAsync(IFormFile file, string subFolder, string prefix)
    {
        if (file == null || file.Length == 0)
        {
            throw new Exception("No file provided.");
        }

        // Validate file extension
        var extension = Path.GetExtension(file.FileName).ToLower();
        if (!AllowedExtensions.Contains(extension))
        {
            throw new Exception($"Invalid file type. Allowed types: {string.Join(", ", AllowedExtensions)}");
        }

        // Validate file size
        if (file.Length > MaxFileSize)
        {
            throw new Exception("File size exceeds the maximum allowed size of 5MB.");
        }

        // Create uploads folder
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", subFolder);
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        // Generate unique file name
        var fileName = $"{prefix}_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadsFolder, fileName);

        // Save file
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return $"/uploads/{subFolder}/{fileName}";
    }

    public static async Task<bool> DeleteFileAsync(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            return false;
        }

        try
        {
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath.TrimStart('/'));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return true;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }
}