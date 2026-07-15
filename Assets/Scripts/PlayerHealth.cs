using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour 
{ 
    [Header("Health Settings")] 
    public int maxHealth = 5; 
    public int currentHealth; 

    [Header("Invincibility Settings")] 
    public float invincibilityDuration = 1.5f; 
    private bool isInvincible = false; 

    [Header("Visual Effects")] 
    public SpriteRenderer playerSprite; 
    [Tooltip("Spawns every time the player takes damage")]
    public ParticleSystem hitEffectPrefab;
    [Tooltip("Spawns only when the player dies")]
    public ParticleSystem deathEffectPrefab; 
    [Tooltip("How long to wait for the explosion before switching scenes")]
    public float deathDelay = 1.0f;

    private HealthUI healthUIScript; 
    private bool isDead = false; 

    void Start() 
    { 
        currentHealth = maxHealth; 
        healthUIScript = FindFirstObjectByType<HealthUI>(); 
        if (healthUIScript != null) 
            healthUIScript.UpdateHealthUI(currentHealth, maxHealth); 
        
        if (playerSprite == null) 
            playerSprite = GetComponent<SpriteRenderer>(); 
    } 

    public void TakeDamage(int damageAmount) 
    { 
        if (currentHealth <= 0) 
        { 
            StartCoroutine(DieSequence()); 
        } 

        if (isInvincible || isDead) return; 

        currentHealth -= damageAmount; 
        Debug.Log("Player took damage! Current health " + currentHealth); 

        if (healthUIScript != null) 
            healthUIScript.UpdateHealthUI(currentHealth, maxHealth); 

        // --- SPAWN DAMAGE SPARKS HERE ---
        if (hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        }

        else 
        { 
            StartCoroutine(BecomeInvincible()); 
        } 
    } 

    private IEnumerator BecomeInvincible() 
    { 
        isInvincible = true; 
        float timer = 0f; 
        while (timer < invincibilityDuration) 
        { 
            if (playerSprite != null) playerSprite.enabled = !playerSprite.enabled; 
            yield return new WaitForSeconds(0.1f); 
            timer += 0.1f; 
        } 
        if (playerSprite != null) playerSprite.enabled = true; 
        isInvincible = false; 
    } 

    private IEnumerator DieSequence() 
    { 
        isDead = true;
        Debug.Log("Player Has Died"); 

        if (playerSprite != null) playerSprite.enabled = false; 
        if (TryGetComponent<Collider2D>(out var col)) col.enabled = false;

        if (deathEffectPrefab != null) 
        { 
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity); 
        } 

        yield return new WaitForSeconds(deathDelay); 
        SceneManager.LoadScene("MainMenu"); 
    } 
}