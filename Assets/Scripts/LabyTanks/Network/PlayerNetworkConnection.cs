using System;
using LabyTanks.Network.Interface;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

namespace LabyTanks.Network
{
    [RequireComponent(typeof(NetworkTransformReliable))]
    public class PlayerNetworkConnection : NetworkBehaviour, IPlayerConnection
    {
        [SyncVar] public int _uniqueId;
        
        public event Action Started;
        public event Action Stopped;
        
        public bool IsOwned => isOwned;
        public int UniqueId => _uniqueId;

        public void SetUniqueId(int id) => 
            _uniqueId = id;

        public override void OnStartClient()
        {
            base.OnStartClient();
            Started?.Invoke();
        }

        public override void OnStopClient()
        {
            Stopped?.Invoke();
            base.OnStopClient();
        }
    }
}