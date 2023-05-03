using System.Collections.Generic;

namespace WindowsClient.Shared.ErrorHandling
{
    public interface IActionResult
    {
        bool IsFailure { get; }
        bool IsSuccess { get; }
        IReadOnlyCollection<string> Errors { get; }
    }

    public interface IActionResult<TPayload> : IActionResult
    {
        TPayload Payload { get; }
    }
}
