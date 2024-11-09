using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    public AudioSource MenuMusic;
    public AudioClip selectPressClip;
    public AudioClip forwardPressClip;
    public AudioClip backPressClip;
    public GameObject checkmarkLevel2;
    public GameObject checkmarkLevel3;
    public float fadeDuration = 1.0f;

    public CanvasGroup fadeCanvasGroup;
    public GameObject settingsPanel;
    public GameObject loadPanel;
    public GameObject infoPanel;
    public GameObject selectText1;
    public GameObject selectText2;
    public GameObject selectText3;
    public AudioMixer audioMixer;
    public Slider volumeSlider;
    public Sprite fullscreenSprite;
    public Sprite windowedSprite;
    public Image toggleImage;
    private int selectedLevel;

    void Start()
    {
        Time.timeScale = 1f;
        StartCoroutine(FadeFromBlack(fadeDuration));
        settingsPanel.SetActive(false);
        loadPanel.SetActive(false);
        infoPanel.SetActive(false);
        selectText1.SetActive(false);
        selectText2.SetActive(false);
        selectText3.SetActive(false);
        if (PlayerPrefs.HasKey("VolumePreference"))
        {
            float savedVolume = PlayerPrefs.GetFloat("VolumePreference");
            SetVolume(savedVolume);

            volumeSlider.value = savedVolume;
        }

        toggleImage.sprite = Screen.fullScreen ? fullscreenSprite : windowedSprite;

        UpdateLevelSelectionAvailability();
    }
    
    public void SelectLevel1()
    {
        if (selectPressClip != null)
        {
            MenuMusic.PlayOneShot(selectPressClip);
        }
        selectedLevel = 1;
        selectText1.SetActive(true);
        selectText2.SetActive(false);
        selectText3.SetActive(false);
    }

    private void UpdateLevelSelectionAvailability()
    {
        bool hasCompletedLevel1 = PlayerPrefs.GetInt("Level1Completed", 0) == 1;
        checkmarkLevel2.SetActive(hasCompletedLevel1);
        bool hasCompletedLevel2 = PlayerPrefs.GetInt("Level2Completed", 0) == 1;
        checkmarkLevel3.SetActive(hasCompletedLevel2);
    }

    public void SelectLevel2()
    {
        if (selectPressClip != null)
        {
            MenuMusic.PlayOneShot(selectPressClip);
        }
        if (PlayerPrefs.GetInt("Level1Completed", 0) == 1)
        {
            selectedLevel = 3;
            selectText1.SetActive(false);
            selectText2.SetActive(true);
            selectText3.SetActive(false);
        }
    }

    public void SelectLevel3()
    {
        if (selectPressClip != null)
        {
            MenuMusic.PlayOneShot(selectPressClip);
        }
        if (PlayerPrefs.GetInt("Level2Completed", 0) == 1)
        {
            selectedLevel = 5;
            selectText1.SetActive(false);
            selectText2.SetActive(false);
            selectText3.SetActive(true);
        }
    }

    public void UnlockLevel2()
    {
        checkmarkLevel2.SetActive(true);
        PlayerPrefs.SetInt("Level1Completed", 1);
        PlayerPrefs.Save();
        
    }

    public void UnlockLevel3()
    {
        checkmarkLevel3.SetActive(true);
        PlayerPrefs.SetInt("Level2Completed", 1);
        PlayerPrefs.Save();
        
    }

    public void LoadSelectedLevel()
    {
        if (forwardPressClip != null)
        {
            MenuMusic.PlayOneShot(forwardPressClip);
        }
        StartCoroutine(FadeOutMusicAndLoadScene(fadeDuration, selectedLevel));
    }

    public void QuitGame()
    {
        if (backPressClip != null)
        {
            MenuMusic.PlayOneShot(backPressClip);
        }
        Application.Quit();
    }

    public void ToggleSettings()
    {
        if (forwardPressClip != null && !settingsPanel.activeSelf)
        {
            MenuMusic.PlayOneShot(forwardPressClip);
        }
        else if(forwardPressClip != null && settingsPanel.activeSelf)
        {
            MenuMusic.PlayOneShot(backPressClip);
        }
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }

    public void ToggleLoad()
    {
        if (forwardPressClip != null && !loadPanel.activeSelf)
        {
            MenuMusic.PlayOneShot(forwardPressClip);
        }
        else if(forwardPressClip != null && loadPanel.activeSelf)
        {
            MenuMusic.PlayOneShot(backPressClip);
        }
        loadPanel.SetActive(!loadPanel.activeSelf);
        UpdateLevelSelectionAvailability();
    }

    public void ToggleInfo()
    {
        if (forwardPressClip != null && !infoPanel.activeSelf)
        {
            MenuMusic.PlayOneShot(forwardPressClip);
        }
        else if(forwardPressClip != null && infoPanel.activeSelf)
        {
            MenuMusic.PlayOneShot(backPressClip);
        }
        infoPanel.SetActive(!infoPanel.activeSelf);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
        PlayerPrefs.SetFloat("VolumePreference", volume);
        PlayerPrefs.Save();
    }

    public void SetFullscreen (bool isFullscreen)
    {
        if (selectPressClip != null)
        {
            MenuMusic.PlayOneShot(selectPressClip);
        }
        Screen.fullScreen = isFullscreen;
        toggleImage.sprite = isFullscreen ? fullscreenSprite : windowedSprite;
    }

    private IEnumerator FadeOutMusicAndLoadScene(float duration, int sceneIndex)
    {
        float currentTime = 0;
        float startVolume = MenuMusic.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            MenuMusic.volume = Mathf.Lerp(startVolume, 0, currentTime / duration);
            yield return null;
        }

        yield return StartCoroutine(FadeToBlack(duration));

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    private IEnumerator FadeToBlack(float duration)
    {
        fadeCanvasGroup.blocksRaycasts = true;

        float currentTime = 0;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(0, 1, currentTime / duration);
            yield return null;
        }
    }

    private IEnumerator FadeFromBlack(float duration)
    {
        float currentTime = 0;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(1, 0, currentTime / duration);
            yield return null;
        }

        fadeCanvasGroup.blocksRaycasts = false;
    }
}
