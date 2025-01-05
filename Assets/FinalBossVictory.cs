using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using Unity.Collections;

public class FinalBossVictory : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundMusicSource;
    [SerializeField] private AudioClip gameVictoryClip;
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 1.0f;
    public static bool isImmune = false;
    public static bool victoryStarted = false;

    void Start()
    {
        isImmune = false;
        victoryStarted = false;
    }

    void Update()
    {
        if (BossHealth.bossDefeated && !victoryStarted)
        {
            victoryStarted = true;
            StartCoroutine(DelayedGameVictory());
        }
    }

    IEnumerator DelayedGameVictory()
    {

        Time.timeScale = 1;

        isImmune = true;
    
        if (backgroundMusicSource != null && gameVictoryClip != null)
        {
            backgroundMusicSource.Stop();
            backgroundMusicSource.clip = gameVictoryClip;
            backgroundMusicSource.loop = false;
            backgroundMusicSource.Play();
            yield return new WaitForSecondsRealtime(gameVictoryClip.length);
        }

        StartCoroutine(FadeToBlackAndLoadScene(fadeDuration, 6));
    }

    private IEnumerator FadeToBlackAndLoadScene(float duration, int sceneIndex)
    {
        fadeCanvasGroup.blocksRaycasts = true;

        float currentTime = 0f;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(0, 1, currentTime / duration);
            yield return null;
        }

        SceneManager.LoadSceneAsync(sceneIndex);
    }
}
