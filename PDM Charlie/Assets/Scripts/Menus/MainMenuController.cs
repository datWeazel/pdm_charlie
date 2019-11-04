using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    public void SetGameState(string newGameState)
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().SetGameState(newGameState);
    }

    public void ExitGame()
    {
        Debug.Log("QuitGame (User action)");
        Application.Quit();
    }

    public void OnPlayerJoined(PlayerInput player)
    {
        characterSelect.GetComponent<CharacterSelectionController>().OnPlayerJoined(player);
    }

    public void OnPlayerLeft(PlayerInput player)
    {
        characterSelect.GetComponent<CharacterSelectionController>().OnPlayerLeft(player);
    }
}
