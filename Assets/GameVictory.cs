using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameVictory : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundMusicSource;
    [SerializeField] private AudioClip gameVictoryClip;
    [SerializeField] private float delayBeforeTransition = 1.0f;
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 1.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Win();
        }
    }

    void Win()
    {
        PlayerPrefs.SetInt("Level1Completed", 1);
        PlayerPrefs.Save();
        GameSession.Instance.ResetCheckpoints();
        
        StartCoroutine(DelayedGameVictory());
    }

    IEnumerator DelayedGameVictory()
    {
        if (backgroundMusicSource != null)
        {
            backgroundMusicSource.Stop();
            if (gameVictoryClip != null)
            {
                backgroundMusicSource.clip = gameVictoryClip;
                backgroundMusicSource.Play();
            }
        }

        yield return new WaitForSecondsRealtime(delayBeforeTransition);
        
        StartCoroutine(FadeToBlackAndLoadScene(fadeDuration, 3));
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
