using System;

namespace LabyTanks.Network.Interface
{
    public interface IPlayerConnection
    {
        event Action Started;
        event Action Stopped;
        bool IsOwned { get; }
        int UniqueId { get; }
        void SetUniqueId(int id);
    }
}
