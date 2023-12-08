using System;
using UnityEngine;

namespace LabyTanks.Game
{
    public class LifecycleBehaviour : MonoBehaviour
    {
        public event Action Updated;
        public event Action FixedUpdated;

        private void Update() => 
            Updated?.Invoke();

        private void FixedUpdate() => 
            FixedUpdated?.Invoke();
    }
}
