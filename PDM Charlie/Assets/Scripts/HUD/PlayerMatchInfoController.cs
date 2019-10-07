using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMatchInfoController : MonoBehaviour
{
    public TextMeshProUGUI PlayerName;
    public TextMeshProUGUI PlayerPercentage;
    public TextMeshProUGUI PlayerStocks;

    public void UpdatePlayerName(string newName)
    {
        PlayerName.text = newName;   
    }

    public void UpdatePlayerPercentage(int newPercentage)
    {
        PlayerPercentage.text = $"{newPercentage}%";
    }

    public void UpdatePlayerStockCount(int newStockCount)
    {
        PlayerStocks.text = $"{newStockCount}";
    }

    public void ActivateParent()
    {
        this.gameObject.SetActive(true);
    }
}
