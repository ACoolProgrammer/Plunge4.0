using UnityEngine;
using UnityEngine.SceneManagement;

public class HazardDamage : MonoBehaviour
{
    public int damageToDeal = 1;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        
        if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageToDeal);
            }
        }
    }
    
    private void Die()
    {
        Debug.Log("Player Died");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
