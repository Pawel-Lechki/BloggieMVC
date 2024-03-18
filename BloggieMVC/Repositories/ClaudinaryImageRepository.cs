namespace BloggieMVC.Repositories;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

public class ClaudinaryImageRepository : IImageRepository
{
    private readonly IConfiguration _configuration;
    private readonly Account account;

    public ClaudinaryImageRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        account = new Account(
            configuration.GetSection("Cloudinary")["CloudName"],
            configuration.GetSection("Cloudinary")["ApiKey"],
            configuration.GetSection("Cloudinary")["ApiSecret"]
        );
    }
    public async Task<string> UploadAsync(IFormFile file)
    {
        var client = new Cloudinary(account);
        
        var uploadParams = new ImageUploadParams()
        {
            File = new FileDescription(file.FileName, file.OpenReadStream()),
            DisplayName = file.FileName
        };

        var uploadResult = await client.UploadAsync(uploadParams);

        if (uploadResult != null && uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return uploadResult.SecureUrl.ToString();
        }

        return null;
    }
}