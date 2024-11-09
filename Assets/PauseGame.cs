using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    [SerializeField] private GameObject resumeButton;
    [SerializeField] private GameObject quitButton;
    [SerializeField] private GameObject gamePausedText;
    public AudioSource BackgroundMusic;
    public AudioClip forwardPressClip;
    public AudioClip backPressClip;
    public AudioClip pausePressClip;
    private bool isPaused = false;
    private Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        if (resumeButton != null) resumeButton.SetActive(false);
        if (quitButton != null) quitButton.SetActive(false);
        if (gamePausedText != null) gamePausedText.SetActive(false);

        anim = GameObject.Find("Player_Knight/model").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !DeathZone.IsGameOver)
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                GamePause();
            }
        }
    }

    public void GamePause()
    {
        if (pausePressClip != null)
        {
            BackgroundMusic.PlayOneShot(pausePressClip);
        }
        resumeButton.SetActive(true);
        quitButton.SetActive(true);
        gamePausedText.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        if (forwardPressClip != null)
        {
            BackgroundMusic.PlayOneShot(forwardPressClip);
        }
        resumeButton.SetActive(false);
        quitButton.SetActive(false);
        gamePausedText.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void QuitGame()
    {
        if (backPressClip != null)
        {
            BackgroundMusic.PlayOneShot(backPressClip);
        }
        GameSession.Instance.ResetCheckpoints();
        SceneManager.LoadSceneAsync(0);
    }
}
