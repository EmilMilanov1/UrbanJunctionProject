namespace UrbanJunction.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendContactEmailAsync(string name, string email, string message);
    }
}
