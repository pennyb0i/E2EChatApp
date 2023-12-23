namespace E2EChatApp.Core.Domain.Responses;

public class ErrorResponse {
    public string Type { get; }
    public int Status { get; }
    public string TraceId { get; }
    public string Error { get; }

    public ErrorResponse(string type, int status, string traceId, string error) {
        Type = type;
        Status = status;
        TraceId = traceId;
        Error = error;
    }
}
