using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private AudioManager audioManager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet"))
        {
            player player = GetComponent<player>();
            player.TakeDamage(10f);
        }
        else if (collision.CompareTag("Usb"))
        {
            gameManager.WinGame();
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Energy"))
        {
            gameManager.AddEneegy();
            Destroy(collision.gameObject);
            audioManager.PlayEnergySound();
        }
    }
}
