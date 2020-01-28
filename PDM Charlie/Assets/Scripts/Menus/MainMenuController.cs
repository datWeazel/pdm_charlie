using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject startGameScreen = null;
    public GameObject mainMenu = null;
    public GameObject characterSelect = null;
    public GameObject stageSelect = null;

    public AudioClip buttonHoverSound;
    public AudioClip buttonClickSound;

    private bool moveCamera = false;
    private Vector3 cameraEndPosition = new Vector3();
    private Quaternion cameraEndRotation;

    public float camMoveSpeed = 2.0f;
    public float camRotateSpeed = 1.0f;

    private void FixedUpdate()
    {
        if (moveCamera)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, cameraEndPosition, Time.deltaTime);
            Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, cameraEndRotation, Time.deltaTime);
            if (Camera.main.transform.position == cameraEndPosition && Camera.main.transform.rotation == cameraEndRotation) moveCamera = false;
        }
    }

    public void ShowStartGameScreen()
    {
        startGameScreen.SetActive(true);
    }

    public void HideStartGameScreen()
    {
        startGameScreen.SetActive(false);
    }

    public void ShowMenu(GameObject menu){
        menu?.SetActive(true);
    }

    public void HideMenu(GameObject menu){
        menu?.SetActive(false);
    }

    public void HoverButton(Button button)
    {
        if (button.GetComponent<Animator>() != null)
        {
            button.GetComponent<Animator>().SetBool("hovered", true);
        }

        if (button.GetComponent<AudioSource>() != null)
        {
            button.GetComponent<AudioSource>().clip = this.buttonHoverSound;
            button.GetComponent<AudioSource>().Play();
        }
    }

    public void UnHoverButton(Button button)
    {
        if (button.GetComponent<Animator>() != null)
        {
            button.GetComponent<Animator>().SetBool("hovered", false);
        }
    }

    public void ClickButton(Button button)
    {
        button.onClick.Invoke();
        if (button.GetComponent<AudioSource>() != null)
        {
            button.GetComponent<AudioSource>().clip = this.buttonClickSound;
            button.GetComponent<AudioSource>().Play();
        }
    }

    public void SetGameState(string newGameState)
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().SetGameState(newGameState);
    }

    public void TryStartGame()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().PrepareMatch();
    }

    public void RaiseGameStocks()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().RaiseStockCount();
    }
    public void LowerGameStocks()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().LowerStockCount();
    }

    public void MoveMainMenuCameraToPosition(Vector3 newPosition, Quaternion newRotation)
    {
        //cameraEndPosition = newPosition;
        //cameraEndRotation = newRotation;
        //moveCamera = true;
        Camera.main.transform.DOMove(newPosition, camMoveSpeed);
        Camera.main.transform.DORotateQuaternion(newRotation, camRotateSpeed);
    }

    public void ExitGame()
    {
        //Debug.Log("QuitGame (User action)");
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
