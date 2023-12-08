using System;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

namespace LabyTanks.Network
{
    [RequireComponent(typeof(NetworkIdentity))]
    public class Cell : NetworkBehaviour
    {
        [SerializeField] private GameObject _leftWall;
        [SerializeField] private GameObject _rightWall;
        [SerializeField] private GameObject _upWall;
        [SerializeField] private GameObject _downWall;

        [SyncVar] public bool _left;
        [SyncVar] public bool _right;
        [SyncVar] public bool _up;
        [SyncVar] public bool _down;
        
        public int ClusterId { get; private set; }

        public override void OnStartClient()
        {
            SetWallActive(WallDirection.Left, _left);
            SetWallActive(WallDirection.Right, _right);
            SetWallActive(WallDirection.Up, _up);
            SetWallActive(WallDirection.Down, _down);
        }

        [Server]
        public void Init(Cell cell)
        {
            SetClusterId(cell.ClusterId);
            SetWallActive(WallDirection.Down, cell.GetWallActive(WallDirection.Down));
            SetWallActive(WallDirection.Up, cell.GetWallActive(WallDirection.Up));
            SetWallActive(WallDirection.Left, cell.GetWallActive(WallDirection.Left));
            SetWallActive(WallDirection.Right, cell.GetWallActive(WallDirection.Right));
        }
        
        [Server]
        public void Init(int clusterId, bool left, bool right, bool up, bool down)
        {
            SetClusterId(clusterId);
            SetWallActive(WallDirection.Down, down);
            SetWallActive(WallDirection.Up, up);
            SetWallActive(WallDirection.Left, left);
            SetWallActive(WallDirection.Right, right);
        }
        
        [Server]
        public void SetClusterId(int clusterId) => 
            ClusterId = clusterId;
        
        public void SetWallActive(WallDirection direction, bool active)
        {
            switch (direction)
            {
                case WallDirection.Down:
                    _downWall.SetActive(active);
                    break;
                case WallDirection.Up:
                    _upWall.SetActive(active);
                    break;
                case WallDirection.Left:
                    _leftWall.SetActive(active);
                    break;
                case WallDirection.Right:
                    _rightWall.SetActive(active);
                    break;
            }
        }

        public bool GetWallActive(WallDirection direction)
        {
            return direction switch
            {
                WallDirection.Up => _upWall.activeSelf,
                WallDirection.Down => _downWall.activeSelf,
                WallDirection.Left => _leftWall.activeSelf,
                WallDirection.Right => _rightWall.activeSelf,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }

        //
        
        public enum WallDirection
        {
            Left,
            Right,
            Up,
            Down
        }
    }
}