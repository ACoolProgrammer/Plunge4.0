using UnityEngine;
using TMPro;
using System.Collections;

public class TutorialTrigger : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI tutorialTextElement;

    [Header("Tutorial Content")]
    [TextArea(3,5)]
    [SerializeField] private string messageToDisplay;

    [Header("Typewriter Settings")]
    [SerializeField] private float typingSpeed = 0.05f;

    private Coroutine typingCoroutine;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
           if (typingCoroutine != null) StopCoroutine(typingCoroutine);

           typingCoroutine = StartCoroutine(TypeText(messageToDisplay));
        }
    }

    private IEnumerator TypeText(string textToType)
    {
        tutorialTextElement.text = "";

        foreach (char letter in textToType.ToCharArray())
        {
            tutorialTextElement.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
