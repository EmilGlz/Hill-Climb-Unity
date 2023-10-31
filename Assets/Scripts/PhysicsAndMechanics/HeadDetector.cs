using Scripts.Managers;
using UnityEngine;
namespace ScriptsPhysicsAndMechanics
{
    public class HeadDetector : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision != null && collision.gameObject.CompareTag("Ground"))
                GameManager.Instance.GameOver(GameOverCause.HeadCrack);
        }
    }
}