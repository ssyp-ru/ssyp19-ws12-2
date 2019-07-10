using JetBrains.Annotations;

namespace Ssyp.Communicator.Common.Requests
{
    public interface ICommunicatorRequest
    {
        [NotNull] string ApiKey { get; }
    }
}