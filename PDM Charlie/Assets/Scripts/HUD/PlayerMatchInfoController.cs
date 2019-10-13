using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMatchInfoController : MonoBehaviour
{
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI playerPercentage;
    public TextMeshProUGUI playerStocks;

    public void UpdatePlayerName(string newName)
    {
        this.playerName.text = newName;   
    }

    public void UpdatePlayerPercentage(int newPercentage)
    {
        this.playerPercentage.text = $"{newPercentage}%";
    }

    public void UpdatePlayerStockCount(int newStockCount)
    {
        this.playerStocks.text = $"{newStockCount}";
    }

    public void ActivateParent()
    {
        this.gameObject.SetActive(true);
    }
}
