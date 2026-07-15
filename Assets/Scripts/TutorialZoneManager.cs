using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class TutorialManager : MonoBehaviour
{
    // Make this class accessible from any other script easily
    public static TutorialManager Instance { get; private set; }

    [Header("UI Reference")]
    [SerializeField] private GameObject tutorialCanvas; 
    [SerializeField] private TextMeshProUGUI tutorialTextElement;
    [SerializeField] private CanvasGroup canvasGroup;

    // A structure to hold the text configuration sent by a trigger
    private struct TutorialData
    {
        public string text;
        public float typingSpeed;
        public float untypingSpeed;
        public float visibleDuration;
        public float fadeDuration;
    }

    // A queue list to stack up incoming text packages automatically
    private Queue<TutorialData> textQueue = new Queue<TutorialData>();
    private bool isProcessingQueue = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (tutorialCanvas != null)
        {
            tutorialCanvas.SetActive(false);
            if (canvasGroup != null) canvasGroup.alpha = 0f;
        }
    }

    // This is the public method your trigger zones will call to line up their text
    public void QueueMessage(string msg, float typeSpeed, float untypeSpeed, float duration, float fadeTime)
    {
        TutorialData newData = new TutorialData
        {
            text = msg,
            typingSpeed = typeSpeed,
            untypingSpeed = untypeSpeed,
            visibleDuration = duration,
            fadeDuration = fadeTime
        };

        textQueue.Enqueue(newData);

        // If the machine isn't running, turn it on!
        if (!isProcessingQueue)
        {
            StartCoroutine(ProcessQueueSequence());
        }
    }

    private IEnumerator ProcessQueueSequence()
    {
        isProcessingQueue = true;
        tutorialCanvas.SetActive(true);

        // Keep running as long as there are messages waiting in line
        while (textQueue.Count > 0)
        {
            TutorialData currentData = textQueue.Dequeue();

            // 1. Fade the canvas in smoothly if it isn't already visible
            if (canvasGroup != null && canvasGroup.alpha < 0.9f)
            {
                yield return StartCoroutine(FadeCanvas(1f, currentData.fadeDuration));
            }

            // 2. Type forward loop
            tutorialTextElement.text = ""; 
            foreach (char letter in currentData.text.ToCharArray())
            {
                tutorialTextElement.text += letter;
                yield return new WaitForSeconds(currentData.typingSpeed);
            }

            // 3. Read wait delay
            yield return new WaitForSeconds(currentData.visibleDuration);

            // 4. Undo typewriter deletion loop
            string currentText = tutorialTextElement.text;
            while (currentText.Length > 0)
            {
                currentText = currentText.Substring(0, currentText.Length - 1);
                tutorialTextElement.text = currentText;
                yield return new WaitForSeconds(currentData.untypingSpeed);
            }

            // If there is another message immediately waiting in the queue, 
            // skip fading out so the transition to the next text looks seamless.
            if (textQueue.Count > 0)
            {
                yield return new WaitForSeconds(0.1f); // Micro-gap between texts
            }
            else
            {
                // No more messages left! Safe to fade out completely
                if (canvasGroup != null)
                {
                    yield return StartCoroutine(FadeCanvas(0f, currentData.fadeDuration));
                }
                tutorialCanvas.SetActive(false);
            }
        }

        isProcessingQueue = false;
    }

    private IEnumerator FadeCanvas(float targetAlpha, float duration)
    {
        if (canvasGroup == null) yield break;

        float startAlpha = canvasGroup.alpha;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
    }
}