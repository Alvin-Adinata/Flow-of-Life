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
    // Tambahkan tombol level 2 jika ingin dikunci juga
    // [SerializeField] private Button ToLevel2Button; 

    private void Start()
    {
        // Awal game: Home aktif, LevelPanel nonaktif
        HomeScreen.SetActive(true);
        LevelPanel.SetActive(false);

        // --- TAMBAHAN BARU: SISTEM PENGUNCIAN LEVEL ---
        CheckLevelStatus();
        // ---------------------------------------------

        // Assign fungsi tombol
        StartGameButton.onClick.AddListener(OpenLevelPanel);
        ToLevel0Button.onClick.AddListener(() => LoadLevel("Level0"));
        ToLevel1Button.onClick.AddListener(() => LoadLevel("Level1"));
    }

    private void CheckLevelStatus()
    {
        // Cek Level 1
        // Defaultnya 0 (Terkunci), kecuali sudah diset jadi 1 di Level 0
        int isLevel1Unlocked = PlayerPrefs.GetInt("Level1Unlocked", 0);

        if (isLevel1Unlocked == 1)
        {
            // Jika sudah terbuka
            ToLevel1Button.interactable = true; 
        }
        else
        {
            // Jika belum terbuka (Terkunci)
            ToLevel1Button.interactable = false; 
            
            // OPSIONAL: Jika ingin menambahkan gambar gembok manual
            // ToLevel1Button.transform.Find("IconGembok").gameObject.SetActive(true);
        }
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
    
    // FUNGSI CHEAT (Untuk mengetes reset level saat develop)
    // Panggil fungsi ini pakai tombol rahasia atau saat Start untuk reset
    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Progress Direset!");
    }
}