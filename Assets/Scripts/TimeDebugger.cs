using UnityEngine;

public class TimeDebugger : MonoBehaviour
{
    void Update()
    {
        if (Time.timeScale != 1)
        {
            Debug.LogWarning("⚠ TimeScale berubah! Nilai sekarang = " + Time.timeScale);
        }
    }
}
