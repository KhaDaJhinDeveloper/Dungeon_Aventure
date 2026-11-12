using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    private int coinAmount = 0;
    public int CoinAmount { get => this.coinAmount; set => this.coinAmount = value; }
    public void IncreaseCoinAmount(int amount)
    {
        this.coinAmount += amount;
        EventManager.Instance.TriggerEvent("LoadCoinText");
    }
    public void SpendCoin(int amount)
    {
        this.coinAmount -= amount;
        EventManager.Instance.TriggerEvent("LoadCoinText");
    }
}
