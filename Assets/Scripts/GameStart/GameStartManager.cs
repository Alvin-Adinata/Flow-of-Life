using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStartManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject HomeScreen;
    [SerializeField] private GameObject LevelPanel;

    [Header("Buttons")]
    [SerializeField] private Button StartGameButton;
    [SerializeField] private Button ToLevel0Button;
    [SerializeField] private Button ToLevel1Button;

    private void Start()
    {
        // Awal game: Home aktif, LevelPanel nonaktif
        HomeScreen.SetActive(true);
        LevelPanel.SetActive(false);

        // Assign fungsi tombol
        StartGameButton.onClick.AddListener(OpenLevelPanel);
        ToLevel0Button.onClick.AddListener(() => LoadLevel("Level0"));
        ToLevel1Button.onClick.AddListener(() => LoadLevel("Level1"));
    }

    private void OpenLevelPanel()
    {
        HomeScreen.SetActive(false);
        LevelPanel.SetActive(true);
    }

    private void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
