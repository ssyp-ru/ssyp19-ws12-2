using System;

namespace Ssyp.Communicator.Common
{
    public interface ICommunicatorRequest
    {
        Guid ApiKey { get; }
    }
}