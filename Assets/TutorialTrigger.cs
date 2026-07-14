using UnityEngine;
using TMPro;
using System.Collections;

public class TutorialTrigger : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private GameObject tutorialCanvas;
    [SerializeField] private TextMeshProUGUI tutorialTextElement;

    [Header("Tutorial Content")]
    [TextArea(3, 5)]
    [SerializeField] private string messageToDisplay;

    [Header("Typewriter Settings")]
    [SerializeField] private float typingSpeed = 0.05f; // Seconds between letters

    [Header("Display Timer")]
    [SerializeField] private float visibleDuration = 1.0f;
    private Coroutine tutorialCoroutine;

    private void Start()
    {
        if (tutorialCanvas != null)
        {
            tutorialCanvas.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (tutorialTextElement == null || tutorialTextElement == null)
            {
                Debug.LogError($"[TutorialTrigger] Missing Text Element on {gameObject.name}! Drag your UI Text into the Inspector slot.", gameObject);
                return; 
            }

            if (tutorialCoroutine != null) StopCoroutine(tutorialCoroutine);
            tutorialCoroutine = StartCoroutine(RunTutorialSequence());
        }
    }

    private IEnumerator RunTutorialSequence()
    {
        tutorialCanvas.SetActive(true);
        tutorialTextElement.text = ""; // Clear text
        
        foreach (char letter in messageToDisplay.ToCharArray())
        {
            tutorialTextElement.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(visibleDuration);

        tutorialCanvas.SetActive(false);
    }
}