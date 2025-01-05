using System.Collections;
using UnityEngine;
using TMPro;

public class TipTypeWriter : MonoBehaviour
{
    [SerializeField] TMP_Text _tmpProText;
    [SerializeField] float delayBeforeStart = 0f;
    [SerializeField] float timeBtwChars = 0.1f;
    [SerializeField] string textPrefix = "";
    [SerializeField] bool repeatTyping = false;

    private string fullText;
    private Coroutine typingCoroutine;

    private void Awake()
    {
        _tmpProText = GetComponent<TMP_Text>();
        _tmpProText.text = "";
    }

    public void DisplayText(string text)
    {
        fullText = text;
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeWriterTMP());
    }

    IEnumerator TypeWriterTMP()
    {
        _tmpProText.text = textPrefix;
        yield return new WaitForSeconds(delayBeforeStart);

        foreach (char c in fullText)
        {
            _tmpProText.text += c;
            yield return new WaitForSeconds(timeBtwChars);
        }

        if (repeatTyping)
        {
            yield return new WaitForSeconds(2);
            DisplayText(fullText);
        }
    }
}
