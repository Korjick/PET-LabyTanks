using LabyTanks.Network.Interface;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace LabyTanks.Game
{
    public class TankMovement : MonoBehaviour
    {
        [SerializeField] private float _speed = 12f;
        [SerializeField] private float _turnSpeed = 180f;
        [Header("Audio")]
        [SerializeField] private AudioSource _movementAudio;
        [SerializeField] private AudioClip _engineIdling;
        [SerializeField] private AudioClip _engineDriving;
        [SerializeField] private float _pitchRange = 0.2f;
        [Header("Input")]
        [SerializeField] private string _movementAxisName = "Vertical";
        [SerializeField] private string _turnAxisName = "Horizontal";
        [Space] 
        [SerializeField] private GameObject _playerConnectionObject;
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private ParticleSystem[] _particleSystems;
        
        private float _movementInputValue;
        private float _turnInputValue;
        private float _mOriginalPitch;

        private LifecycleBehaviour _lifecycleBehaviour;
        private IPlayerConnection _playerConnection;
        private bool _moving;
        
        [Inject]
        private void Construct(LifecycleBehaviour lifecycleBehaviour)
        {
            _lifecycleBehaviour = lifecycleBehaviour;
            _playerConnection = _playerConnectionObject.GetComponent<IPlayerConnection>();
        }

        private void Awake()
        {
            _playerConnection.Started += PlayerConnection_Started;
            _playerConnection.Stopped += PlayerConnection_Stopped;
        }

        private void OnDestroy()
        {
            _playerConnection.Started -= PlayerConnection_Started;
            _playerConnection.Stopped -= PlayerConnection_Stopped;
        }

        private void PlayerConnection_Started()
        {
            Camera playerCamera = Camera.main;
            if (_playerConnection.IsOwned && playerCamera != null)
            {
                playerCamera.transform.SetParent(_cameraTransform);
                playerCamera.transform.localRotation = Quaternion.identity;
                playerCamera.transform.localPosition = Vector3.zero;
            }
            
            _rigidbody.isKinematic = false;
            _movementInputValue = 0f;
            _turnInputValue = 0f;
            
            foreach (var system in _particleSystems)
                system.Play();

            _mOriginalPitch = _movementAudio.pitch;
            
            _lifecycleBehaviour.Updated += LifeCycleBehaviour_Updated;
            _lifecycleBehaviour.FixedUpdated += LifeCycleBehaviour_FixedUpdated;
        }
        
        private void PlayerConnection_Stopped()
        {
            _rigidbody.isKinematic = true;

            foreach (var system in _particleSystems)
                system.Stop();
            
            _lifecycleBehaviour.Updated -= LifeCycleBehaviour_Updated;
            _lifecycleBehaviour.FixedUpdated -= LifeCycleBehaviour_FixedUpdated;
        }
        
        private void LifeCycleBehaviour_Updated()
        {
            if (_playerConnection.IsOwned)
            {
                _movementInputValue = Input.GetAxis(_movementAxisName);
                _turnInputValue = Input.GetAxis(_turnAxisName);
            }
            
            EngineAudio();
        }

        private void LifeCycleBehaviour_FixedUpdated()
        {
            if (!_playerConnection.IsOwned)
                return;

            Move();
            Turn();
        }

        private void EngineAudio()
        {
            if (_moving)
            {
                if (_movementAudio.clip == null || _movementAudio.clip == _engineDriving)
                {
                    _movementAudio.clip = _engineIdling;
                    _movementAudio.pitch = Random.Range (_mOriginalPitch - _pitchRange, _mOriginalPitch + _pitchRange);
                    _movementAudio.Play();
                }
            }
            else
            {
                if (_movementAudio.clip == _engineIdling)
                {
                    _movementAudio.clip = _engineDriving;
                    _movementAudio.pitch = Random.Range(_mOriginalPitch - _pitchRange, _mOriginalPitch + _pitchRange);
                    _movementAudio.Play();
                }
            }
        }

        private void Move()
        {
            Vector3 movement = transform.forward * _movementInputValue * _speed * Time.deltaTime;
            _rigidbody.AddForce(movement, ForceMode.Force);
            _moving = movement.Equals(Vector3.zero);
        }
        
        private void Turn()
        {
            float turn = _turnInputValue * _turnSpeed * Time.deltaTime;
            Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
            _rigidbody.MoveRotation(_rigidbody.rotation * turnRotation);
        }
    }
}