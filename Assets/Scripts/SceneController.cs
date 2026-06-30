using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneController : MonoBehaviour
{
    [Header("Scene Settings")]
    public string nextSceneName;
    public string mainMenuSceneName = "mainmenu";

    [Header("Timer Options")]
    public bool useTimer = false;
    public float delayInSeconds = 5f;

    [Header("Skip Settings")]
    public bool canSkip = true;
    public GameObject skipButton;
    public float skipButtonDelay = 2f; 

    [Header("Level Info")]
    public int levelIndex = 0;

    [Header("Tutorial Settings")]
    public GameObject tutorialPanel;
    private bool isTutorialOpen = false;

    // --- TAMBAHAN BARU: SETTINGS OPTIONS ---
    [Header("Settings Options")]
    public GameObject settingsPanel;
    private bool isSettingsOpen = false;

    private Coroutine timerCoroutine;

    void Awake()
    {
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false);
        }

        // --- TAMBAHAN BARU: Pastikan panel settings mati di awal ---
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }

        if (skipButton != null)
        {
            skipButton.SetActive(false);
            if (canSkip)
            {
                StartCoroutine(ShowSkipButtonAfterDelay());
            }
        }

        if (useTimer)
        {
            timerCoroutine = StartCoroutine(WaitAndChangeScene());
        }
    }

    IEnumerator ShowSkipButtonAfterDelay()
    {
        yield return new WaitForSeconds(skipButtonDelay);
        if (skipButton != null)
        {
            skipButton.SetActive(true);
        }
    }

    public void SkipScene()
    {
        if (canSkip)
        {
            if (timerCoroutine != null)
            {
                StopCoroutine(timerCoroutine);
            }
            LoadNextScene();
        }
    }

    public void PlayButtonSound()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.buttons);
        }
    }

    public void OpenTutorial()
    {
        PlayButtonSound();
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(true);
            Time.timeScale = 0f;
            isTutorialOpen = true;
        }
    }

    public void CloseTutorial()
    {
        PlayButtonSound();
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false);
            // Hanya normalkan waktu jika panel settings juga sedang tidak terbuka
            if (!isSettingsOpen)
            {
                Time.timeScale = 1f;
            }
            isTutorialOpen = false;
        }
    }

    // --- TAMBAHAN BARU: METHOD UNTUK SETTINGS PANEL ---

    public void OpenSettings()
    {
        PlayButtonSound();
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
            Time.timeScale = 0f; // Menjeda seluruh pergerakan/scene (pause)
            isSettingsOpen = true;
        }
    }

    public void CloseSettings()
    {
        PlayButtonSound();
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
            // Hanya normalkan waktu jika panel tutorial juga sedang tidak terbuka
            if (!isTutorialOpen)
            {
                Time.timeScale = 1f; // Melanjutkan scene (resume)
            }
            isSettingsOpen = false;
        }
    }

    // --- UPDATED NAVIGATION METHODS TO USE TRANSITION INSTANCE ---

    public void RestartScene()
    {
        PlayButtonSound();
        Time.timeScale = 1f;
        string currentScene = SceneManager.GetActiveScene().name;

        if (SceneTransitionManager.Instance != null)
            SceneTransitionManager.Instance.NextLevel(currentScene);
        else
            SceneManager.LoadScene(currentScene);
    }

    public void BackToMainMenu()
    {
        PlayButtonSound();
        Time.timeScale = 1f;
        if (!string.IsNullOrEmpty(mainMenuSceneName))
        {
            if (SceneTransitionManager.Instance != null)
                SceneTransitionManager.Instance.NextLevel(mainMenuSceneName);
            else
                SceneManager.LoadScene(mainMenuSceneName);
        }
    }

    public void LoadNextScene()
    {
        PlayButtonSound();
        Time.timeScale = 1f;

        if (levelIndex > 0)
        {
            PlayerPrefs.SetInt("levelDipilih", levelIndex);
            Debug.Log("Set levelDipilih: " + levelIndex);
        }

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            if (SceneTransitionManager.Instance != null)
                SceneTransitionManager.Instance.NextLevel(nextSceneName);
            else
                SceneManager.LoadScene(nextSceneName);
        }
    }

    public void QuitGame()
    {
        PlayButtonSound();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    IEnumerator WaitAndChangeScene()
    {
        yield return new WaitForSeconds(delayInSeconds);
        LoadNextScene();
    }
}