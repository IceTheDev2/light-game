using UnityEngine;
using TMPro;
using System.Collections;
using System.Text;
using UnityEngine.SceneManagement;

public class TypewriterEffect : MonoBehaviour
{
    public float typingSpeed = 0.1f;  // Delay between each letter
    public float deletionSpeed = 0.05f;  // Delay between each letter during deletion
    public float delayBeforeDeletion = 2f;  // Delay before text starts disappearing
    public string[] texts;
    public LightCalculator lightCalculator;
    public bool voiceIsDone = false;

    private TextMeshProUGUI textMeshPro;
    private StringBuilder currentText;
    private Coroutine typingCoroutine;
    private Coroutine deletionCoroutine;
    private int textIndex = 0;
    
    /// <summary>
    /// Called when the object is neabled, this method sets voiceIsDone to false, gets the textMeshPro component, 
    /// and starts the TypeText() coroutine.
    /// </summary>
    private void OnEnable()
    {
        voiceIsDone = false;
        textMeshPro = GetComponent<TextMeshProUGUI>();
        currentText = new StringBuilder();

        typingCoroutine = StartCoroutine(TypeText());
    }

    /// <summary>
    /// Called after the object is enabled by the OnEnable() method, this coroutine starts typing the text if it's not fully typed at the typingSpeed,
    /// or calls the DeleteText() coroutine if it's fully typed.
    /// </summary>
    /// <returns></returns>
    private IEnumerator TypeText()
    {
        yield return new WaitForSeconds(delayBeforeDeletion);

        if (textIndex < texts.Length)
        {
            string fullText = texts[textIndex];

            for (int i = 0; i < fullText.Length; i++)
            {
                currentText.Append(fullText[i]);
                textMeshPro.text = currentText.ToString();

                yield return new WaitForSeconds(typingSpeed);
            }

            deletionCoroutine = StartCoroutine(DeleteText());
        }
    }

    /// <summary>
    /// Called when the text needs to be deleted by the TypeText() coroutine, this coroutine waits delayBeforeDeletion seconds before starting to delete the text at the deletionSpeed.
    /// If there's another phrase to be said, it also calls the TypeText() coroutine to type it, else it sets voiceIsDone to true.
    /// </summary>
    /// <returns></returns>
    private IEnumerator DeleteText()
    {
        yield return new WaitForSeconds(delayBeforeDeletion);

        for (int i = currentText.Length - 1; i >= 0; i--)
        {
            currentText.Remove(i, 1);
            textMeshPro.text = currentText.ToString();

            yield return new WaitForSeconds(deletionSpeed);
        }

        textIndex++;

        if (textIndex < texts.Length)
        {
            typingCoroutine = StartCoroutine(TypeText());
        }
        else
        {
            voiceIsDone = true;
            if (lightCalculator.won == true)
            {
                yield return new WaitForSeconds(0.5f);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }

    /// <summary>
    /// Called when the object is destroyed, this method stops the typing and deletion coroutines.
    /// </summary>
    private void OnDestroy()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        if (deletionCoroutine != null)
            StopCoroutine(deletionCoroutine);
    }

    /// <summary>
    /// Called when the object is disabled, this method stops the typing and deletion coroutines.
    /// </summary>
    private void OnDisable()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        if (deletionCoroutine != null)
            StopCoroutine(deletionCoroutine);
    }

}
