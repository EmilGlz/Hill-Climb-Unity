using ScriptsPhysicsAndMechanics;
using UnityEngine;

public class TireDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != null && collision.gameObject.CompareTag("Coin500"))
            Utils.HideCoin(collision.gameObject);
        if (collision.gameObject != null && collision.gameObject.CompareTag("Fuel"))
            Utils.HideCoin(collision.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject != null && collision.gameObject.CompareTag("Ground"))
            VehicleController.Instance.TouchingGround = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject != null && collision.gameObject.CompareTag("Ground"))
            VehicleController.Instance.TouchingGround = false;
    }
}
