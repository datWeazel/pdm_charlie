using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    public int Id = -1;
    public int stocks = 1;
    public float percentage = 0;
    public Vector3 spawnpoint = new Vector3();

    public GameObject selector = null;
    public float selectorSpeed = 1.0f;
    private GameObject gameController = null;
    private GameController gameControllerScript = null;

    public string character = "";
    public CharacterControllerBase characterController = null;

    public PlayerMatchInfoController matchHUD = null;

    private Vector2 moveVector = new Vector2();

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);

        this.gameController = GameObject.Find("GameController");
        this.gameControllerScript = gameController.GetComponent<GameController>();

        this.selector.GetComponentInChildren<TextMeshProUGUI>().text = $"P {this.Id}";
    }

    // Update is called once per frame
    void Update()
    {
        string gameState = this.gameControllerScript.GetGameState();
        Debug.Log($"State: {gameState} || Id: {this.Id}");

        if (gameState == "character_select" || gameState == "stage_select" || gameState == "match_end" || (gameState == "main_menu" && this.Id == 1))
        {
            if (!this.selector.activeSelf) this.selector.SetActive(true);

            if (this.moveVector != new Vector2())
            {
                this.selector.transform.position += new Vector3((this.moveVector.x * this.selectorSpeed), (this.moveVector.y * this.selectorSpeed), 0);
            }
        }
        else
        {
            if (this.selector.activeSelf) this.selector.SetActive(false);

            if(this.matchHUD != null)
            {
                this.matchHUD.UpdatePlayerPercentage(this.percentage);
            }


            if (gameState == "match_active")
            {
                if (this.moveVector != new Vector2())
                {
                    this.characterController?.Move(new Vector2(this.moveVector.x, this.moveVector.y));
                }
            }
        }
    }

    public void HitDeathZone()
    {
        this.stocks--;
        this.matchHUD?.UpdatePlayerStockCount(this.stocks);

        this.percentage = 0.0f;

        if(this.stocks == 0)
        {
            this.gameControllerScript.RemovePlayer(this.GetComponent<PlayerInput>());
            GameObject.Destroy(this.characterController.gameObject);
            return;
        }

        this.characterController.SetPosition(this.spawnpoint);
    }

    public GameObject CreateCharacter(GameObject characterPrefab, Vector3 position)
    {
        GameObject character = Instantiate(characterPrefab, this.transform);
        character.transform.position = position;

        this.characterController = character.GetComponent<CharacterControllerBase>();
        this.spawnpoint = position;

        return character;
    }

    public void OnMove(InputValue value)
    {
        this.moveVector = value.Get<Vector2>();
    }

    public void OnPoint(InputValue value)
    {
        this.selector.transform.position = value.Get<Vector2>();
    }

    public void OnClick()
    {
        string gameState = this.gameControllerScript.GetGameState();
        if (gameState == "main_menu" || gameState == "character_select" || gameState == "stage_select")
        {
            Select();
        }
    }

    public void OnJump(InputValue value)
    {
        string gameState = this.gameControllerScript.GetGameState();
        Debug.Log($"OnJump - gameState: {gameState}");
        if (gameState == "main_menu" || gameState == "character_select" || gameState == "stage_select")
        {
            Select();
        }
        else if (this.gameControllerScript.GetGameState() == "match_active")
        {
            this.characterController?.Jump();
        }
    }

    public void Select()
    {
        PointerEventData cursor = new PointerEventData(EventSystem.current);
        cursor.position = this.selector.transform.position;

        List<RaycastResult> objectsHit = new List<RaycastResult>();
        EventSystem.current.RaycastAll(cursor, objectsHit);

        foreach (RaycastResult rr in objectsHit)
        {
            Transform CharacterNameTag = rr.gameObject.transform.Find("CharacterNameTag");
            if (CharacterNameTag != null)
            {
                this.character = CharacterNameTag.GetComponent<TextMeshProUGUI>().text;
                GameObject.Find("CharacterSelect").GetComponent<CharacterSelectionController>().UpdateSelectedCharacter(this.transform.GetComponent<PlayerInput>(), this.character);
                Debug.Log($"Player selected character {CharacterNameTag.GetComponent<TextMeshProUGUI>().text} ");

                return;
            }

            Transform StageNameTag = rr.gameObject.transform.Find("StageNameTag");
            if(StageNameTag != null)
            {
                this.gameControllerScript.stageName = StageNameTag.GetComponent<TextMeshProUGUI>().text;
                GameObject.Find("SelectedStage").GetComponent<TextMeshProUGUI>().text = $"{this.gameControllerScript.stageName}";
                return;
            }

            Button btn = rr.gameObject.GetComponent<Button>();
            if(btn != null)
            {
                if (rr.gameObject.name == "btn_stage_select" && (this.gameControllerScript.players.Count < 2 || !this.gameControllerScript.DoesEveryPlayerHaveCharacter())) return;
                if (rr.gameObject.name == "btn_match_start" && this.gameControllerScript.stageName == "") return;
                btn.onClick.Invoke();
                return;
            }
        }
    }

    public void OnLightAttack(InputValue value)
    {
        if (this.gameControllerScript.GetGameState() == "match_active")
        {
            this.characterController?.Attack(false);
        }
    }

    public void OnHeavyAttack(InputValue value)
    {
        if (this.gameControllerScript.GetGameState() == "match_active")
        {
            this.characterController?.Attack(true);
        }
    }

    public void OnStart(InputValue value)
    {
        if(this.gameControllerScript.gameState == "match_end")
        {
            this.gameControllerScript.LoadScene("MainMenu");
        }
    }

    public void OnSelect()
    {
        string gameState = this.gameControllerScript.gameState;
        if(gameState == "main_menu" || gameState == "character_select")
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
