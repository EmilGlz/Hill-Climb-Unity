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
        public int Distance { get; private set; }

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
            var Distance = (_player.position - startPos);
            if (Distance.x < 0)
                Distance.x = 0;
            _text.text = Distance.x.ToString("F0") + " m";
        }
    }
}
