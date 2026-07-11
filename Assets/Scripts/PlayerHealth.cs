using System.Collections; // Added so IEnumerator works
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 5;
    public int currentHealth; // Kept public from HEAD so other scripts can read it if needed

    [Header("Invincibility Settings")]
    public float invincibilityDuration = 1.5f;
    private bool isInvincible = false;

    [Header("Visual Effects")]
    public SpriteRenderer playerSprite;

    private HealthUI healthUIScript;

    void Start() // Fixed typo: capitalized 'S' so Unity runs this automatically
    {
        currentHealth = maxHealth;

        // Fixed typo: Looked for 'HealthUI' component class name instead of variable name
        healthUIScript = FindFirstObjectByType<HealthUI>();

        if (healthUIScript != null) healthUIScript.UpdateHealthUI(currentHealth, maxHealth);

        if (playerSprite == null) playerSprite = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int damageAmount)
    {
        if (isInvincible) return;

        currentHealth -= damageAmount;
        Debug.Log("Player took damage! Current health" + currentHealth);

        if (healthUIScript != null) healthUIScript.UpdateHealthUI(currentHealth, maxHealth); // Fixed typo: added missing semicolon

        if (currentHealth <= 0) // Changed to <= 0 so player dies exactly at 0 health
        {
            Die();
        }
        else
        {
            StartCoroutine(BecomeInvincible()); // Fixed typo: misspelled 'StartCoroutine'
        }
    }

    private IEnumerator BecomeInvincible()
    {
        isInvincible = true;
        float timer = 0f; // Fixed typo: added missing semicolon and 'f' literal

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
        SceneManager.LoadScene("MainMenu"); // Simplified scene loading for a Main Menu
    }
}