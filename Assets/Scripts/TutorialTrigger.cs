using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [Header("Tutorial Content")]
    [TextArea(3, 5)]
    [SerializeField] private string messageToDisplay;

    [Header("Typewriter Settings")]
    [SerializeField] private float typingSpeed = 0.05f; 
    [SerializeField] private float untypingSpeed = 0.02f; 

    [Header("Display Timer")]
    [SerializeField] private float visibleDuration = 1.0f;
    [SerializeField] private float fadeDuration = 0.3f; 

    private Collider2D triggerCollider; 
    private bool isTriggered = false; 

    private void Start()
    {
        triggerCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true;

            // Turn off this zone's trigger right away
            if (triggerCollider != null)
            {
                triggerCollider.enabled = false;
            }

            // Hand the message settings over to the central manager queue
            if (TutorialManager.Instance != null)
            {
                TutorialManager.Instance.QueueMessage(
                    messageToDisplay, 
                    typingSpeed, 
                    untypingSpeed, 
                    visibleDuration, 
                    fadeDuration
                );
            }
            else
            {
                Debug.LogError("[TutorialTrigger] Could not find a TutorialManager instance in the scene!");
            }

            // The trigger zone's job is done, it can destroy itself instantly
            Destroy(gameObject);
        }
    }
}