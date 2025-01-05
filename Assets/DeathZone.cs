using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor.SearchService;
#endif

public class DeathZone : MonoBehaviour
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
    private Animator anim;
    public static bool IsGameOver = false;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        if (playButton != null) playButton.SetActive(false);
        if (quitButton != null) quitButton.SetActive(false);
        if (playAgainText != null) playAgainText.SetActive(false);

        anim = GameObject.Find("Player_Knight/model").GetComponent<Animator>();

        if (GameSession.Instance.checkpointPosition != Vector3.zero)
        {
            GameObject player = GameObject.Find("Player_Knight");
            player.transform.position = GameSession.Instance.checkpointPosition;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Die();
        }
    }

    void Die()
    {
        if(!IsGameOver && !CorruptorVictory.isImmune && !FinalBossVictory.victoryStarted)
        {
            IsGameOver = true;
            if (deathSound != null)
            {
                effectsSource.PlayOneShot(deathSound);
            }
            anim.SetTrigger("Die");
            StartCoroutine(DelayedGameOver());
        }
    }

    IEnumerator DelayedGameOver()
    {

        yield return new WaitForSecondsRealtime(.5f);

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
        StartCoroutine(LoadSceneAfterSound(.5f, 2));
    }

    public void PlayAgainL2()
    {
        if (forwardPressClip != null)
        {
            backgroundMusicSource.PlayOneShot(forwardPressClip);
        }
        StartCoroutine(LoadSceneAfterSound(.5f, 4));
    }

    public void PlayAgainL3()
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
        GameSession.Instance.ResetCheckpoints();
        SceneManager.LoadSceneAsync(0);
    }
}
