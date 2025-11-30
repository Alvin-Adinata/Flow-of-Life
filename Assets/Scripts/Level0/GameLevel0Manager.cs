using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameLevel0Manager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject tutorialPanel;
    [SerializeField] GameObject winPanel;

    [Header("Menu Panel")]
    [SerializeField] GameObject menuPanel;

    [Header("Cutscene")]
    [SerializeField] private GameObject cutsceneRawImage;
    [SerializeField] private VideoPlayer videoPlayer;

    [Header("Skip Cutscene")]
    [SerializeField] private GameObject skipCutscenePanel;   
    [SerializeField] private Button skipCutsceneButton;      

    [Header("Audio Clips")]
    [SerializeField] private AudioClip bgMusic;
    [SerializeField] private AudioClip winMusic;
    [SerializeField] private AudioClip loseMusic;

    private AudioSource audioSource;

    public static bool isRestarted = false;
    public bool isGameActive = true;
    public static GameLevel0Manager instance;

    public static bool allowTimer = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SetupAudio();

        allowTimer = false;

        if (menuPanel != null)
            menuPanel.SetActive(false);

        if (skipCutscenePanel != null)
            skipCutscenePanel.SetActive(false);

        PlayCutscene();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isGameActive) return;
            if (cutsceneRawImage.activeSelf) return;

            if (!menuPanel.activeSelf)
                OpenMenu();
            else
                ContinueGame();
        }
    }

    // ============================================================
    //                         CUTSCENE
    // ============================================================
    private void PlayCutscene()
    {
        gameOverPanel.SetActive(false);
        tutorialPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);

        audioSource.Stop();
        Time.timeScale = 1f;

        cutsceneRawImage.SetActive(true);

        // ★ Tampilkan tombol Skip selama cutscene
        if (skipCutscenePanel != null)
            skipCutscenePanel.SetActive(true);

        if (skipCutsceneButton != null)
            skipCutsceneButton.onClick.AddListener(SkipCutscene);

        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.Play();
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        SkipCutscene(); // otomatis skip saat video selesai
    }

    public void SkipCutscene()
    {
        videoPlayer.Stop();

        cutsceneRawImage.SetActive(false);
        if (skipCutscenePanel != null)
            skipCutscenePanel.SetActive(false);

        StartGameAfterCutscene();
    }

    private void StartGameAfterCutscene()
    {
        if (!isRestarted)
        {
            tutorialPanel.SetActive(true);

            PlayBackgroundMusic();
            Time.timeScale = 0f;

            allowTimer = false;
        }
        else
        {
            tutorialPanel.SetActive(false);

            PlayBackgroundMusic();
            Time.timeScale = 1f;

            allowTimer = true;
            isRestarted = false;
        }
    }

    // ============================================================
    //                       MENU SYSTEM
    // ============================================================
    public void OpenMenu()
    {
        if (!isGameActive) return;
        menuPanel.SetActive(true);

        Time.timeScale = 0f;
        allowTimer = false;
    }

    public void ContinueGame()
    {
        menuPanel.SetActive(false);

        Time.timeScale = 1f;
        allowTimer = true;
    }

    // ============================================================
    //                       AUDIO SYSTEM
    // ============================================================
    private void SetupAudio()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.loop = true;
        audioSource.playOnAwake = false;

        PlayBackgroundMusic();
    }

    private void PlayBackgroundMusic()
    {
        if (bgMusic == null) return;

        audioSource.Stop();
        audioSource.clip = bgMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    private void PlayWinMusic()
    {
        if (winMusic == null) return;

        audioSource.Stop();
        audioSource.clip = winMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    private void PlayLoseMusic()
    {
        if (loseMusic == null) return;

        audioSource.Stop();
        audioSource.clip = loseMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    // ============================================================
    //                         TUTORIAL
    // ============================================================
    public void StartGameFromTutorial()
    {
        tutorialPanel.SetActive(false);

        Time.timeScale = 1f;
        allowTimer = true;
    }

    // ============================================================
    //                        GAME OVER
    // ============================================================
    public void GameOver()
    {
        if (!isGameActive) return;

        isGameActive = false;
        allowTimer = false;

        menuPanel.SetActive(false);
        gameOverPanel.SetActive(true);

        PlayLoseMusic();
        Time.timeScale = 0f;
    }

    // ============================================================
    //                          WIN
    // ============================================================
    public void LevelCompleted()
    {
        if (!isGameActive) return;

        isGameActive = false;
        allowTimer = false;

        PlayerPrefs.SetInt("Level1Unlocked", 1);
        PlayerPrefs.Save();

        if (winPanel != null) winPanel.SetActive(true);

        PlayWinMusic();
        Time.timeScale = 0f;
    }

    // ============================================================
    //                         RESTART
    // ============================================================
    public void RestartGame()
    {
        Time.timeScale = 1f;
        isRestarted = true;

        audioSource.Stop();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // ============================================================
    //                           HOME
    // ============================================================
    public void ToHome()
    {
        Time.timeScale = 1f;
        audioSource.Stop();
        SceneManager.LoadScene("GameStart");
    }

    // ============================================================
    //                        NEXT LEVEL
    // ============================================================
    public void ToNextLevel()
    {
        Time.timeScale = 1f;
        audioSource.Stop();
        SceneManager.LoadScene("Level1");
    }
}
