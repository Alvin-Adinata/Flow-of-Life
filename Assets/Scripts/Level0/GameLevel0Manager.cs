using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLevel0Manager : MonoBehaviour
{
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject tutorialPanel;

    // Panel Menang
    [SerializeField] GameObject winPanel;

    public static bool isRestarted = false;

    // Status agar game tidak mengecek win berkali-kali
    public bool isGameActive = true;

    public static GameLevel0Manager instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (!isRestarted)
        {
            gameOverPanel.SetActive(false);
            tutorialPanel.SetActive(false);

            // Matikan panel win di awal
            if (winPanel != null) winPanel.SetActive(false);

            Time.timeScale = 0f;
            ShowTutorial();
        }
        else
        {
            tutorialPanel.SetActive(false);
            gameOverPanel.SetActive(false);

            if (winPanel != null) winPanel.SetActive(false);

            Time.timeScale = 1f;
            isRestarted = false;
        }
    }

    public void ShowTutorial()
    {
        tutorialPanel.SetActive(true);
    }

    public void StartGameFromTutorial()
    {
        tutorialPanel.SetActive(false);
        Time.timeScale = 1f; // mulai permainan
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
    }

    // Level Completed / Menang
    public void LevelCompleted()
    {
        if (!isGameActive) return;

        Debug.Log("YOU WIN!");
        isGameActive = false;

        if (winPanel != null) winPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        isRestarted = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // ==========================
    //   TO HOME BUTTON
    // ==========================
    public void ToHome()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameStart");
    }

    // ==========================
    //   TO NEXT LEVEL BUTTON
    // ==========================
    public void ToNextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level1");
    }
}
