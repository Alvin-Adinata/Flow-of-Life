using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject tutorialPanel;

    public static bool isRestarted = false;

    void Start()
    {

        if (!isRestarted)
        {
            gameOverPanel.SetActive(false);
            tutorialPanel.SetActive(false);

            Time.timeScale = 0f;
            ShowTutorial();
        }
        else
        {
            // jika baru restart, langsung main tanpa tutorial
            tutorialPanel.SetActive(false);
            gameOverPanel.SetActive(false);
            Time.timeScale = 1f;

            isRestarted = false; // reset lagi setelah dipakai
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

    public void RestartGame()
    {
        Time.timeScale = 1f;
        isRestarted = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
