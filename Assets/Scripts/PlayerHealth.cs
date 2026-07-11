using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 5;
    private int currentHealth;

    [Header("Invincibility Settings")]
    public float invincibilityDuration = 1.5f;
    private bool isInvincible = false;

    [Header("Visual Effects")]
    public SpriteRenderer playerSprite;

    private HealthUI healthUIScript;

    void start()
    {
        currentHealth = maxHealth;

        healthUIScript = FindFirstObjectByType<healthUIScript>();

        if (healthUIScript != null) healthUIScript.UpdateHealthUI(currentHealth, maxHealth);

        if (playerSprite == null) playerSprite = GetComponent<SpriteRenderer>();
    
    }

    public void TakeDamage(int damageAmount)
    {
        if (isInvincible) return;

        currentHealth -= damageAmount;
        Debug.Log("Player took damage! Current health" + currentHealth);

        if (healthUIScript != null) healthUIScript.UpdateHealthUI(currentHealth, maxHealth)

        if (currentHealth < 0)
        {
            Die();
        }
        else
        {
            StartCprputine(BecomeInvincible());
        }
    }

    private IEnumerator BecomeInvincible()
    {
        isInvincible = true;
        float timer = 0

        while (timer < invincibilityDuration)
        {
            if (playerSprite != null) playerSprite.enabled = !playerSprite.enabled;
            yield return new WaitForSeconds(0.1f);
            timer += 0.1f;
        }

        if (playerSprite != null) playerSprite.enabled = true;
        isInvincible = false;
    }

    private void Die()
    {
        Debug.Log("Player Has Died");
        AsyncOperation op = SceneManager.LoadSceneAsync("MainMenu");
    }
}
