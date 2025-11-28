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

    [Header("Background Music")]
    [SerializeField] private AudioClip bgMusic;
    private AudioSource audioSource;

    private void Start()
    {
        SetupMusic();

        HomeScreen.SetActive(true);
        LevelPanel.SetActive(false);

        CheckLevelStatus();

        StartGameButton.onClick.AddListener(OpenLevelPanel);
        ToLevel0Button.onClick.AddListener(() => LoadLevel("Level0"));
        ToLevel1Button.onClick.AddListener(() => LoadLevel("Level1"));
    }

    private void SetupMusic()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.clip = bgMusic;
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        if (bgMusic != null)
            audioSource.Play();
    }

    private void CheckLevelStatus()
    {
        int isLevel1Unlocked = PlayerPrefs.GetInt("Level1Unlocked", 0);
        ToLevel1Button.interactable = isLevel1Unlocked == 1;
    }

    private void OpenLevelPanel()
    {
        HomeScreen.SetActive(false);
        LevelPanel.SetActive(true);

        if (audioSource != null)
        {
            audioSource.Stop();  
            audioSource.Play();  
        }
    }

    private void LoadLevel(string sceneName)
    {
        if (audioSource != null)
            audioSource.Stop();

        SceneManager.LoadScene(sceneName);
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Progress Direset!");
    }
}
