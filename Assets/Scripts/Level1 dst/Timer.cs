using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;

    [Header("Timer Settings")]
    [SerializeField] float startTime;   // waktu awal timer

    float remainingTime; // waktu berjalan (runtime)

    void Start()
    {
        // reset timer setiap kali scene dimulai / restart
        remainingTime = startTime;
    }

    void Update()
    {
        // Kurangi waktu
        remainingTime -= Time.deltaTime;

        // Jika waktu <= 10 detik, ubah warna menjadi merah
        if (remainingTime <= 10f)
        {
            timerText.color = Color.red;
        }

        // Jika habis → Game Over
        if (remainingTime <= 0f)
        {
            timerText.text = "0:00.00";

            // panggil GameOver dari GameManager
            Object.FindFirstObjectByType<GameManager>()?.GameOver();
            return;
        }

        // Update display
        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);
        int milliseconds = Mathf.FloorToInt((remainingTime * 1000f) % 1000f / 10f);  // dua digit ms

        // Format: M:SS.ms (dua digit ms)
        timerText.text = string.Format("{0:0}:{1:00}.{2:00}", minutes, seconds, milliseconds);
    }
}
