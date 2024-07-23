using Atlas.Requests.Interfaces;

namespace Atlas.Requests.Model
{
    public class Response<T> : IResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public T? Result { get; set; }
    }
}
