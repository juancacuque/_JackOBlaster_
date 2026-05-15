using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public float healAmount = 25f;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();

            if (playerController != null)
            {
                playerController.Heal(healAmount);
                Destroy(gameObject); 
            }
        }
    }
}

