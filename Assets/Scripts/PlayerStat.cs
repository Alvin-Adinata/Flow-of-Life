using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public static int Money;
    [SerializeField] public int startMoney = 500;

    void Start()
    {
        Money = startMoney;
    }
 }
