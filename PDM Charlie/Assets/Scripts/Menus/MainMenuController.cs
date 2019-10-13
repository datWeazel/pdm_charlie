using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject mainMenu = null;
    public GameObject characterSelect = null;
    public GameObject stageSelect = null;


    public void ShowMenu(GameObject menu){
            menu?.SetActive(true);
    }

    public void HideMenu(GameObject menu){
            menu?.SetActive(false);
    }

    public void ExitGame()
    {
        Debug.Log("QuitGame (User action)");
        Application.Quit();
    }
}
