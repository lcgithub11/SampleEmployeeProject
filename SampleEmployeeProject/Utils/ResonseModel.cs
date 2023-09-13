public class ResponseModel
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public object Data { get; set; } = null;

    public ResponseModel() { }

    public ResponseModel(bool success, string message)
    {
        Success = success;
        Message = message;
    }

    public ResponseModel(bool success, string message, object data) : this(success, message)
    {
        Data = data;
    }
}
