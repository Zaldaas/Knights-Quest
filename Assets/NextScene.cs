using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 1.0f;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(FadeFromBlack(fadeDuration));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            StartCoroutine(FadeToBlackAndLoadScene(fadeDuration, 2));
        }
    }

    private IEnumerator FadeToBlackAndLoadScene(float duration, int sceneIndex)
    {
        fadeCanvasGroup.blocksRaycasts = true;

        float currentTime = 0;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(0, 1, currentTime / duration);
            yield return null;
        }

        SceneManager.LoadSceneAsync(sceneIndex);
    }

    private IEnumerator FadeFromBlack(float duration)
    {
        fadeCanvasGroup.blocksRaycasts = false;

        float currentTime = 0;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(1, 0, currentTime / duration);
            yield return null;
        }
    }
}
