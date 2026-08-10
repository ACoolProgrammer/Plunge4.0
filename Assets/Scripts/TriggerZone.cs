using UnityEngine;

public class PortalButtonTriggerBridge : MonoBehaviour
{
    private PortalButton masterScript;

    void Start()
    {
        masterScript = GetComponentInParent<PortalButton>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PushableObject"))
        {
            masterScript.ObjectEnteredZone();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PushableObject"))
        {
            masterScript.ObjectExitedZone();
        }
    }
}