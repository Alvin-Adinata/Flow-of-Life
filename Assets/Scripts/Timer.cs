using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime;

    void Update()
    {

        if (remainingTime <= 0)
        {
            timerText.text = "0:00";
            timerText.color = Color.red;
            return;
        }
        remainingTime -= Time.deltaTime;
        int minutes = Mathf.FloorToInt(remainingTime / 60F);
        int seconds = Mathf.FloorToInt(remainingTime - minutes * 60);
        timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }
}
