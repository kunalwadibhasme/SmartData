namespace E_CommerceBackend.Interfaces
{
    public interface IImageFileService
    {
        Task<string> SaveFileAsync(IFormFile imageFile, string[] allowedFileExtensions);
    }
}
