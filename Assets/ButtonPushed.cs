using UnityEngine;
using System.Collections;

public class ButtonPushed : MonoBehaviour
{
    [Header("Movement Settings")]
    public Vector3 pushedPositionOffset = new Vector3(0, -0.2f, 0); // Distance to slide down
    public float moveSpeed = 5f;
    public float activationDelay = 2.0f; // Seconds required to trigger

    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private Coroutine activationCoroutine;
    private bool isActivated = false;

    void Start()
    {
        originalPosition = transform.localPosition;
        targetPosition = originalPosition;
    }

    void Update()
    {
        // Smoothly slide the button to its current target position
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * moveSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the pushing object has the right tag and button isn't already used
        if (collision.CompareTag("PushableObject") && !isActivated)
        {
            targetPosition = originalPosition + pushedPositionOffset; // Move down
            
            // Start the confirmation countdown
            activationCoroutine = StartCoroutine(ActivationTimer());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PushableObject") && !isActivated)
        {
            targetPosition = originalPosition; // Move back up

            // Stop the countdown because they left early
            if (activationCoroutine != null)
            {
                StopCoroutine(activationCoroutine);
            }
        }
    }

    private IEnumerator ActivationTimer()
    {
        yield return new WaitForSeconds(activationDelay);
        
        // If the coroutine finishes without being stopped, trigger success!
        TriggerSuccess();
    }

    private void TriggerSuccess()
    {
        isActivated = true;
        Debug.Log("Congrats!");
        
    }
}