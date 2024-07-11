namespace eFlow.API.Contracts.Users
{
    public record UsersResponse<T>
    (
        bool? successful,
        T Value,
        string Message
    );
}
