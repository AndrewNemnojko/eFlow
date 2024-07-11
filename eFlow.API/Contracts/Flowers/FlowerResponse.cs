namespace eFlow.API.Contracts.Flowers
{
    public record FlowerResponse<T>
    (
        bool? successful,
        T Value,
        string Message
    );
}
