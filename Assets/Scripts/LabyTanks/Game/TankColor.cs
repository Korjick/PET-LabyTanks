using LabyTanks.Network.Interface;
using UnityEngine;
using Zenject;

namespace LabyTanks.Game
{
    public class TankColor : MonoBehaviour
    {
        [SerializeField] private GameObject _playerConnectionObject;
        [SerializeField] private RendererRecolor _rendererRecolor;
        
        private IPlayerConnection _playerConnection;
        
        [Inject]
        private void Construct(LifecycleBehaviour lifecycleBehaviour)
        {
            _playerConnection = _playerConnectionObject.GetComponent<IPlayerConnection>();
        }
        
        private void Awake()
        {
            _playerConnection.Started += PlayerConnection_Started;
        }

        private void OnDestroy()
        {
            _playerConnection.Started -= PlayerConnection_Started;
        }
        
        private void PlayerConnection_Started()
        {
            _rendererRecolor.SetColor(_playerConnection.UniqueId);
        }
    }
}