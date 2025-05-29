using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    private int coinAmount = 0;
    public int CoinAmount { get => this.coinAmount; set => this.coinAmount = value; }
    void Start()
    {
        EventManager.OP_EventManager.Subscribe<int>("IncreaseCoinAmount", IncreaseCoinAmount);
        EventManager.OP_EventManager.Subscribe<int>("SpendCoin", SpendCoin);
    }
    public void IncreaseCoinAmount(int amount)
    {
        this.coinAmount += amount;
        EventManager.OP_EventManager.TriggerEvent("LoadCoinText");
    }
    public void SpendCoin(int amount)
    {
        this.coinAmount -= amount;
        EventManager.OP_EventManager.TriggerEvent("LoadCoinText");
    }
    private void OnDestroy()
    {
        EventManager.OP_EventManager.Unsubscribe<int>("IncreaseCoinAmount", IncreaseCoinAmount);
        EventManager.OP_EventManager.Unsubscribe<int>("SpendCoin", SpendCoin);
    }
}
