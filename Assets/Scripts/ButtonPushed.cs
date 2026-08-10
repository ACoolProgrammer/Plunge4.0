using UnityEngine;
using System.Collections;

public class PortalButton : MonoBehaviour
{
    [Header("Assign Hierarchies")]
    [Tooltip("Drag the moving Red VisualButton child object here.")]
    public Transform movingVisualButton;

    [Header("Movement Settings")]
    [Tooltip("How far down the red visual button travels when compressed.")]
    public Vector3 pushedPositionOffset = new Vector3(0, -0.2f, 0); 
    public float moveSpeed = 5f;
    public float activationDelay = 2.0f; 

    private Vector3 originalLocalPos;
    private Vector3 targetLocalPos;
    private Coroutine activationCoroutine;
    private bool isFullyActivated = false;
    private int objectsPressingCount = 0; 

    void Start()
    {
        if (movingVisualButton == null)
        {
            Debug.LogError("Please assign the moving VisualButton child object in the inspector!", this);
            return;
        }

        // Capture local coordinates relative to the static system anchor parent
        originalLocalPos = movingVisualButton.localPosition;
        targetLocalPos = originalLocalPos;
    }

    void Update()
    {
        // Smoothly return or compress using precise frame-independent tracking
        movingVisualButton.localPosition = Vector3.MoveTowards(
            movingVisualButton.localPosition, 
            targetLocalPos, 
            moveSpeed * Time.deltaTime
        );
    }

    // Call these methods from a small bridge script placed on your TriggerZone child object,
    // OR change your TriggerZone setup to route collisions straight here.
    public void ObjectEnteredZone()
    {
        if (isFullyActivated) return;

        objectsPressingCount++;
        if (objectsPressingCount == 1)
        {
            targetLocalPos = originalLocalPos + pushedPositionOffset;

            if (activationCoroutine != null) StopCoroutine(activationCoroutine);
            activationCoroutine = StartCoroutine(ActivationTimer());
        }
    }

    public void ObjectExitedZone()
    {
        if (isFullyActivated) return;

        objectsPressingCount--;
        if (objectsPressingCount <= 0)
        {
            objectsPressingCount = 0;
            targetLocalPos = originalLocalPos; // Springs back up instantly

            if (activationCoroutine != null)
            {
                StopCoroutine(activationCoroutine);
                activationCoroutine = null;
                Debug.Log("Object left early! Resetting Portal button countdown.");
            }
        }
    }

    private IEnumerator ActivationTimer()
    {
        yield return new WaitForSeconds(activationDelay);
        TriggerPermanentClick();
    }

    private void TriggerPermanentClick()
    {
        isFullyActivated = true;
        targetLocalPos = originalLocalPos + pushedPositionOffset;
        movingVisualButton.localPosition = targetLocalPos;
        
        Debug.Log("Portal Button Clicked! Output triggered permanently.");
    }
}