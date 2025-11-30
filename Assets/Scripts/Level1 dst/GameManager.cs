using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject tutorialPanel;
    [SerializeField] GameObject winPanel;

    [Header("Menu Panel")]
    [SerializeField] GameObject menuPanel; 

    [Header("Audio Clips")]
    [SerializeField] private AudioClip bgMusic;
    [SerializeField] private AudioClip winMusic;
    [SerializeField] private AudioClip loseMusic;

    private AudioSource audioSource;

    public static bool isRestarted = false;
    public bool isGameActive = true;

    public static GameManager instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SetupAudio();

        if (!isRestarted)
        {
            gameOverPanel.SetActive(false);
            tutorialPanel.SetActive(false);
            if (winPanel != null) winPanel.SetActive(false);
            if (menuPanel != null) menuPanel.SetActive(false);

            Time.timeScale = 0f;
            ShowTutorial();
        }
        else
        {
            tutorialPanel.SetActive(false);
            gameOverPanel.SetActive(false);
            if (winPanel != null) winPanel.SetActive(false);
            if (menuPanel != null) menuPanel.SetActive(false);

            Time.timeScale = 1f;
            isRestarted = false;
        }
    }

    void Update()
    {
        // ================================
        //   ESC UNTUK BUKA/TUTUP MENU
        // ================================
        if (Input.GetKeyDown(KeyCode.Escape) && isGameActive)
        {
            // Jika panel belum aktif → buka
            if (!menuPanel.activeSelf)
                OpenMenu();
            else
                ContinueFromMenu();
        }
    }

    // =====================================
    //       AUDIO HANDLING
    // =====================================
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

    // =====================================
    //              TUTORIAL
    // =====================================
    public void ShowTutorial()
    {
        tutorialPanel.SetActive(true);
    }

    public void StartGameFromTutorial()
    {
        tutorialPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    // =====================================
    //              MENU SYSTEM
    // =====================================
    public void OpenMenu()
    {
        if (!isGameActive) return;

        menuPanel.SetActive(true);

        // pause game → timer otomatis berhenti
        Time.timeScale = 0f;
    }

    public void ContinueFromMenu()
    {
        menuPanel.SetActive(false);

        // lanjut game
        Time.timeScale = 1f;
    }

    // =====================================
    //            GAME OVER
    // =====================================
    public void GameOver()
    {
        if (!isGameActive) return;

        isGameActive = false;
        gameOverPanel.SetActive(true);

        PlayLoseMusic();

        Time.timeScale = 0f;
    }

    // =====================================
    //               WIN
    // =====================================
    public void LevelCompleted()
    {
        if (!isGameActive) return;

        Debug.Log("YOU WIN!");
        isGameActive = false;

        PlayerPrefs.SetInt("Level2Unlocked", 1);
        PlayerPrefs.Save();

        if (winPanel != null) winPanel.SetActive(true);

        PlayWinMusic();

        Time.timeScale = 0f;
    }

    // =====================================
    //              RESTART
    // =====================================
    public void RestartGame()
    {
        Time.timeScale = 1f;
        isRestarted = true;

        if (audioSource != null)
            audioSource.Stop();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // =====================================
    //              TO HOME
    // =====================================
    public void ToHome()
    {
        Time.timeScale = 1f;

        if (audioSource != null)
            audioSource.Stop();

        SceneManager.LoadScene("GameStart");
    }

    // =====================================
    //           TO NEXT LEVEL
    // =====================================
    public void ToNextLevel()
    {
        Time.timeScale = 1f;

        if (audioSource != null)
            audioSource.Stop();

        SceneManager.LoadScene("Level2");
    }
}
