using UnityEngine;

public class CoinCollision : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Skull"))
        {
            Destroy(gameObject);
        }
    }
}