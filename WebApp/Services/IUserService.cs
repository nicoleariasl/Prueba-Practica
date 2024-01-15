namespace WebApp.Services
{
    public interface IUserService
    {
        UserResponse Auth(AuthRequest model);
    }
}
