namespace Atlas.Requests.Interfaces
{
    public interface IResponse<T>
    {
        bool IsSuccess { get; set; }
        string? Message { get; set; }
        T? Result { get; set; }
    }
}
