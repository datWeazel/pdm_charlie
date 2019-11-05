using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuController : MonoBehaviour
{
    public GameObject mainMenu = null;
    public GameObject characterSelect = null;
    public GameObject stageSelect = null;

    private bool moveCamera = false;
    private Vector3 cameraEndPosition = new Vector3();
    private Quaternion cameraEndRotation;

    private void FixedUpdate()
    {
        if (moveCamera)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, cameraEndPosition, Time.deltaTime);
            Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, cameraEndRotation, Time.deltaTime);
            if (Camera.main.transform.position == cameraEndPosition && Camera.main.transform.rotation == cameraEndRotation) moveCamera = false;
        }
    }

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

    public void MoveMainMenuCameraToPosition(Vector3 newPosition, Quaternion newRotation)
    {
        cameraEndPosition = newPosition;
        cameraEndRotation = newRotation;
        moveCamera = true;
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
