using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class FadeInOut : MonoBehaviour
{
    public AudioMixer audioMixer;
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 1.0f;

    // Use this for initialization
    void Start()
    {
        if (PlayerPrefs.HasKey("VolumePreference"))
        {
        float savedVolume = PlayerPrefs.GetFloat("VolumePreference");
        audioMixer.SetFloat("Volume", savedVolume);
        }
        StartCoroutine(FadeFromBlack(fadeDuration));
    }

    // Update is called once per frame
    void Update()
    {

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
