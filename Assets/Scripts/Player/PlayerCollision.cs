using UnityEngine;

public class PlayerCollision : MonoBehaviour 
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            GameManager.instance.AddCoins(1);
            collision.GetComponent<PickupCoin>().FlyToUI();
            Destroy(collision.gameObject, .55f);
        }
        
        if (collision.gameObject.CompareTag("Energy"))
        {
            GameManager.instance.AddEnergy(2);
            collision.GetComponent<PickupEnergy>().FlyToUI();
            Destroy(collision.gameObject, .55f);
        }

        if (collision.gameObject.CompareTag("Goal"))
        {
            SceneController.instance.NextLevel();
        }
        if (collision.gameObject.CompareTag("WinPoint"))
        {
            GameManager.instance.WinGame();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Dead"))
        {
            PlayerController.instance.TakeDamage(200);
        }
    }
}