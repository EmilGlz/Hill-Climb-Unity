using UnityEngine;

public class TireDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != null && collision.gameObject.CompareTag("Coin500"))
            Utils.HideCoin(collision.gameObject);
    }
}
