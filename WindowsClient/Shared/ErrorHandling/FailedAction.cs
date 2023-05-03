using System.Collections.Generic;
using System.Linq;

namespace WindowsClient.Shared.ErrorHandling
{
    public class FailedResult : IActionResult
    {
        public FailedResult() { }
        public FailedResult(IList<string> errors) : base() => Errors = errors.ToList();
        public bool IsFailure => !IsSuccess;
        public bool IsSuccess => false;
        public IReadOnlyCollection<string> Errors { get; } = new List<string>();
    }

    public class FailedResult<TPayload> : IActionResult<TPayload>
    {
        public FailedResult() { }
        public FailedResult(IList<string> errors) => Errors = errors.ToList();

        public bool IsFailure => !IsSuccess;
        public bool IsSuccess => false;
        public IReadOnlyCollection<string> Errors { get; } = new List<string>();
        public TPayload Payload { get; } = default;
    }
}
