namespace eFlow.API.Contracts.Auth
{
    public record AuthResponse
    (
        string Token,
        string RefreshToken   
    );
    

}
