using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor.SearchService;
#endif

public class FinalBossDefeat : MonoBehaviour
{
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject quitButton;
    [SerializeField] private GameObject playAgainText;
    [SerializeField] private AudioSource backgroundMusicSource;
    [SerializeField] private AudioSource effectsSource;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip gameOverClip;
    [SerializeField] private AudioClip forwardPressClip;
    [SerializeField] private AudioClip backPressClip;
    [SerializeField] private Text timerText;
    private float countdown = 60.0f;
    public static bool IsGameOver = false;
    // Start is called before the first frame update
    void Start()
    {
        IsGameOver = false;
        Time.timeScale = 1;
        if (playButton != null) playButton.SetActive(false);
        if (quitButton != null) quitButton.SetActive(false);
        if (playAgainText != null) playAgainText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerHealth.playerDefeated && !FinalBossVictory.isImmune)
        {
            IsGameOver = true;
            if (deathSound != null)
            {
                effectsSource.PlayOneShot(deathSound);
            }
            GameOver();
        }

        if (countdown > 0)
        {
            countdown -= Time.deltaTime;
            timerText.text = "Defeat him before time runs out! " + Mathf.Ceil(countdown).ToString();
        }
        else
        {
            IsGameOver = true;
            GameOver();
        }
    }

    private void GameOver()
    {
        if (backgroundMusicSource != null)
        {
            backgroundMusicSource.Stop();
        }

        if (gameOverClip != null)
        {
            backgroundMusicSource.clip = gameOverClip;
            backgroundMusicSource.loop = false;
            backgroundMusicSource.Play();
        }

        Time.timeScale = 0;
        ShowButtonsAndText();
    }

    void ShowButtonsAndText()
    {
        playButton.SetActive(true);
        quitButton.SetActive(true);
        playAgainText.SetActive(true);
    }

    public void PlayAgain()
    {
        if (forwardPressClip != null)
        {
            backgroundMusicSource.PlayOneShot(forwardPressClip);
        }
        StartCoroutine(LoadSceneAfterSound(.5f, 5));
    }

    IEnumerator LoadSceneAfterSound(float delay, int sceneIndex)
    {
        IsGameOver = false;
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(sceneIndex);
    }

    public void QuitGame()
    {
        if (backPressClip != null)
        {
            backgroundMusicSource.PlayOneShot(backPressClip);
        }
        IsGameOver = false;
        SceneManager.LoadSceneAsync(0);
    }
}