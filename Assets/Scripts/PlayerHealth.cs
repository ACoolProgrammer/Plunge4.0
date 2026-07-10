using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth;

    void start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("Player took damage! Current health" + currentHealth);

        if (currentHealth < 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player Has Died");
        AsyncOperation op = SceneManager.LoadSceneAsync("MainMenu");
    }
}
