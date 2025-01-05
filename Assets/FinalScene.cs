using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalScene : MonoBehaviour
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
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            StartCoroutine(FadeToBlackAndLoadScene(fadeDuration, 0));
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private IEnumerator FadeToBlackAndLoadScene(float duration, int sceneIndex)
    {
        fadeCanvasGroup.blocksRaycasts = true;

        float currentTime = 0;
        // Fade to black
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
        fadeCanvasGroup.blocksRaycasts = true;

        float currentTime = duration;
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(0, 1, currentTime / duration);
            yield return null;
        }

        fadeCanvasGroup.blocksRaycasts = false;
    }
}
