using System;
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

    public void UpdatePlayerPercentage(float newPercentage)
    {
        this.playerPercentage.text = $"{(Math.Round(newPercentage, 1))}%";
    }

    public void UpdatePlayerPercentage(string newText)
    {
        this.playerPercentage.text = newText;
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
