using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject tutorialPanel;
    
    // 1. TAMBAHKAN INI: Variabel untuk Panel Menang
    [SerializeField] GameObject winPanel; 

    public static bool isRestarted = false;
    
    // Status agar game tidak mengecek win berkali-kali
    public bool isGameActive = true; 

    void Start()
    {
        if (!isRestarted)
        {
            gameOverPanel.SetActive(false);
            tutorialPanel.SetActive(false);
            
            // Matikan panel win di awal
            if(winPanel != null) winPanel.SetActive(false); 

            Time.timeScale = 0f;
            ShowTutorial();
        }
        else
        {
            tutorialPanel.SetActive(false);
            gameOverPanel.SetActive(false);
            if(winPanel != null) winPanel.SetActive(false);

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

    // 2. TAMBAHKAN FUNGSI INI: Level Completed / Menang
    public void LevelCompleted()
    {
        // Cek dulu apakah game masih aktif (supaya tidak panggil 2x)
        if (!isGameActive) return;

        Debug.Log("YOU WIN!");
        isGameActive = false; // Stop status game
        
        // Nyalakan Panel Win
        if (winPanel != null) winPanel.SetActive(true);
        
        // Hentikan waktu (optional, kalau mau game freeze saat menang)
        Time.timeScale = 0f; 
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        isRestarted = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static GameManager instance;
    void Awake()
    {
        instance = this;
    }
}
