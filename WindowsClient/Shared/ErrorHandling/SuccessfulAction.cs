using System.Collections.Generic;

namespace WindowsClient.Shared.ErrorHandling
{
    public class SuccessfulAction : IActionResult
    {
        public bool IsFailure => !IsSuccess;
        public bool IsSuccess => true;
        public IReadOnlyCollection<string> Errors { get; } = new List<string>();
    }

    public class SuccessfulAction<TPayload> : IActionResult<TPayload>
    {
        public SuccessfulAction(TPayload payload) => Payload = payload;
        public bool IsFailure => !IsSuccess;
        public bool IsSuccess => true;
        public IReadOnlyCollection<string> Errors { get; } = new List<string>();
        public TPayload Payload { get; }
    }
}
