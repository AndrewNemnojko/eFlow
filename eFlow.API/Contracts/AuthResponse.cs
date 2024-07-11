namespace eFlow.API.Contracts
{
    public record AuthResponse(
            string Token,
            string RefreshToken,
            bool Successful,
            string Description
        );
    
}
