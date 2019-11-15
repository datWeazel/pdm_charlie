using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MatchHUDController : MonoBehaviour
{
    public GameObject matchTimer;
    public GameObject endScreen;

    public GameObject[] playerMatchHUDs;

    public GameObject[] countdownNumbers;
    public int currentCountdownStep = 0;

    public void StartCountdown()
    {
        countdownNumbers[currentCountdownStep].SetActive(true);
        StartCoroutine(NextCountdownStep());
    }

    IEnumerator NextCountdownStep()
    {
        float wait = 1.0f;
        if (currentCountdownStep == 3) wait = 0.5f;
        yield return new WaitForSeconds(wait);

        countdownNumbers[currentCountdownStep].SetActive(false);
        if (currentCountdownStep < 3)
        {
            currentCountdownStep++;
            countdownNumbers[currentCountdownStep].SetActive(true);
            if(currentCountdownStep == 2)
            {
                GameObject.Find("GameController").GetComponent<GameController>().StartMatch();
            }
            StartCoroutine(NextCountdownStep());
        }
        else
        {
            currentCountdownStep = 0;
        }
    }

    public void UpdateEndScreenText(string text)
    {
        this.endScreen.transform.Find("EndScreenText").GetComponent<TextMeshProUGUI>().text = text;
    }

    public void SetEndScreenVisible(bool visible)
    {
        this.endScreen.SetActive(visible);
    }

    public GameObject GetPlayerMatchHUD(int id)
    {
        return playerMatchHUDs[id-1];
    }
}
