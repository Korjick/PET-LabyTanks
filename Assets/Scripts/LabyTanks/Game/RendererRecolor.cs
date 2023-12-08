using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LabyTanks.Game
{
    public class RendererRecolor : MonoBehaviour
    {
        [SerializeField] private Renderer[] _renderers;
        [SerializeField] private ColorData[] _colorData;

        private Dictionary<int, Color> _idToColor;
        
        private void Awake()
        {
            _idToColor = _colorData.ToDictionary(data => data.Id, data => data.Color);
        }

        public void SetColor(int id)
        {
            Color tankColor = GetTankColor(id);
            Array.ForEach(_renderers, r => r.material.color = tankColor);
        }
        
        private Color GetTankColor(int id)
        {
            return _idToColor.TryGetValue(id, out Color tmp) ? 
                tmp : _idToColor[int.MinValue];
        }
        
        //
        
        [Serializable]
        public struct ColorData
        {
            public int Id;
            public Color Color;
        }
    }
}