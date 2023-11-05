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
            var distance = (_player.position - startPos);
            if (distance.x < 0)
                distance.x = 0;
            _text.text = distance.x.ToString("F0") + " m";
        }
    }
}
