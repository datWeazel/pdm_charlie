using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MatchHUDController : MonoBehaviour
{
    public GameObject MatchTimer;
    public GameObject EndScreen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateEndScreenText(string text)
    {
        EndScreen.transform.Find("EndScreenText").GetComponent<TextMeshProUGUI>().text = text;
    }

    public void SetEndScreenVisible(bool visible)
    {
        EndScreen.SetActive(visible);
    }
}
