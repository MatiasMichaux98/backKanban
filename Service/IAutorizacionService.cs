using KanabanBack.Models.Custom;

namespace KanabanBack.Service
{
    public interface IAutorizacionService
    {
        Task<AutorizacionResponse> DevolverToken(AutorizacionRequest request);

        Task<AutorizacionResponse> DevolverTokenRefresh(RefreshTokenRequest refreshTokenRequest, int Id);
    }
}
