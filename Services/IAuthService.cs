using FPT_Booking_BE.DTOs;

namespace FPT_Booking_BE.Services
{
    public interface IAuthService
    {
        Task<string> Login(LoginRequest request);
    }
}