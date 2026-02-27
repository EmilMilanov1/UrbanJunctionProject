namespace UrbanJunction.Services.Interfaces
{
    public interface IRecaptchaService
    {
        Task<bool> VerifyAsync(string token);
    }
}
