using System;
using TMPro;
using UnityEngine;

namespace Scripts.Managers
{
    public class DistanceController : IDisposable
    {
        private readonly TMP_Text _text;
        private readonly Transform _player;
        private readonly Vector3 startPos;
        public float Distance { get; private set; }

        public DistanceController(TMP_Text text, Transform player)
        {
            _text = text;
            _player = player;
            startPos = player.position;
            GameManager.Instance.OnUpdate += UpdateDistance;
        }

        public void Dispose()
        {
            GameManager.Instance.OnUpdate -= UpdateDistance;
        }

        private void UpdateDistance()
        {
            if (_player == null)
                return;
            Distance = (_player.position - startPos).x;
            if (Distance < 0)
                Distance = 0;
            _text.text = Distance.ToString("F0") + " m";
        }
    }
}
