using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject MainMenu = null;
    public GameObject CharacterSelect = null;
    public GameObject StageSelect = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
