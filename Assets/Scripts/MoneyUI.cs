using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class MoneyUI : MonoBehaviour
{

    [SerializeField] public TextMeshProUGUI moneyText;

    void Update()
    {
        moneyText.text = "$" + PlayerStat.Money.ToString();
    }
}
