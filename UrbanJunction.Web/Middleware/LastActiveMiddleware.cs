using Microsoft.AspNetCore.Identity;

public class LastActiveMiddleware
{
    private readonly RequestDelegate _next;

    public LastActiveMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, UserManager<UrbanUser> userManager)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var user = await userManager.GetUserAsync(context.User);
            if (user != null && user.LastActiveOn < DateTime.UtcNow.AddMinutes(-5))
            {
                user.LastActiveOn = DateTime.UtcNow;
                await userManager.UpdateAsync(user);
            }
        }

        await _next(context);
    }
}