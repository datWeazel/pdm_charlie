using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MatchHUDController : MonoBehaviour
{
    public GameObject matchTimer;
    public GameObject endScreen;


    public void UpdateEndScreenText(string text)
    {
        this.endScreen.transform.Find("EndScreenText").GetComponent<TextMeshProUGUI>().text = text;
    }

    public void SetEndScreenVisible(bool visible)
    {
        this.endScreen.SetActive(visible);
    }
}
