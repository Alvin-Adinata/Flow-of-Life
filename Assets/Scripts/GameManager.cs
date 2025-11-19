using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject tutorialPanel;

    void Start()
    {

        // pastikan panel disembunyikan di awal
        gameOverPanel.SetActive(false);
        tutorialPanel.SetActive(false);

        Time.timeScale = 0f; // jeda permainan di awal
        ShowTutorial();
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
        Time.timeScale = 1f;   // kembalikan normal
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
