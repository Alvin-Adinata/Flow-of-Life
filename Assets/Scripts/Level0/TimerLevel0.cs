using UnityEngine;
using TMPro;

public class TimerLevel0 : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;

    [Header("Timer Settings")]
    [SerializeField] float startTime;

    float remainingTime;

    void Start()
    {
        remainingTime = startTime;
    }

    void Update()
    {
        // ⭐ Jika belum boleh jalan → stop
        if (!GameLevel0Manager.allowTimer)
            return;

        remainingTime -= Time.deltaTime;

        if (remainingTime <= 10f)
        {
            timerText.color = Color.red;
        }

        if (remainingTime <= 0f)
        {
            timerText.text = "0:00.00";
            remainingTime = 0;

            Object.FindFirstObjectByType<GameLevel0Manager>()?.GameOver();
            return;
        }

        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);
        int milliseconds = Mathf.FloorToInt((remainingTime * 1000f) % 1000f / 10f);

        timerText.text = string.Format("{0:0}:{1:00}.{2:00}", minutes, seconds, milliseconds);
    }
}
