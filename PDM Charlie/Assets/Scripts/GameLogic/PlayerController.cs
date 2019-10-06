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
    public int Stocks = 1;
    public Vector3 Spawnpoint = new Vector3();

    public GameObject Selector = null;
    public float SelectorSpeed = 1.0f;
    private GameObject GameController = null;
    private GameController GameControllerScript = null;

    public string Character = "";
    public GameCharacterController CharacterController = null;

    private Vector2 moveVector = new Vector2();

    // Start is called before the first frame update
    void Start()
    {
        InputUser.PerformPairingWithDevice(Gamepad.current);
        DontDestroyOnLoad(this);
        GameController = GameObject.Find("GameController");
        GameControllerScript = GameController.GetComponent<GameController>();
        Selector.GetComponentInChildren<TextMeshProUGUI>().text = $"P {this.Id}";
    }

    // Update is called once per frame
    void Update()
    {
        if (GameControllerScript.GetGameState() == "character_select" || GameControllerScript.GetGameState() == "stage_select" || GameControllerScript.GetGameState() == "match_end")
        {
            if (!Selector.activeSelf) Selector.SetActive(true);

            if (moveVector != new Vector2())
            {
                Selector.transform.position += new Vector3((moveVector.x * SelectorSpeed), (moveVector.y * SelectorSpeed), 0);
            }
        }
        else
        {
            if (Selector.activeSelf) Selector.SetActive(false);

            if (GameControllerScript.GetGameState() == "match_active")
            {
                if (moveVector != new Vector2())
                {
                    this.CharacterController?.Move(new Vector2(moveVector.x, moveVector.y));
                }
            }
        }
    }

    public void HitDeathZone()
    {
        this.Stocks--;

        if(this.Stocks == 0)
        {
            GameControllerScript.RemovePlayer(this);
            GameObject.Destroy(this.gameObject);
            return;
        }

        this.CharacterController.SetPosition(Spawnpoint);
    }

    public GameObject CreateCharacter(GameObject characterPrefab, Vector3 position)
    {
        Debug.Log($"Creating character {characterPrefab.name} at position {position}");
        GameObject character = Instantiate(characterPrefab, this.transform);
        character.transform.position = position;
        this.CharacterController = character.GetComponent<GameCharacterController>();
        this.Spawnpoint = position;

        return character;
    }

    public void OnMove(InputValue value)
    {
        moveVector = value.Get<Vector2>();
    }

    public void OnPoint(InputValue value)
    {
        Vector2 analogVector = value.Get<Vector2>();
        Selector.transform.position = analogVector;
    }

    public void OnClick()
    {
        Debug.Log("OnClick");
        if (GameControllerScript.GetGameState() == "character_select" || GameControllerScript.GetGameState() == "stage_select" || GameControllerScript.GetGameState() == "match_end")
        {
            Select();
        }
    }

    public void OnJump(InputValue value)
    {
        if(GameControllerScript.GetGameState() == "character_select" || GameControllerScript.GetGameState() == "stage_select" || GameControllerScript.GetGameState() == "match_end")
        {
            Select();
        }
        else if (GameControllerScript.GetGameState() == "match_active")
        {
            this.CharacterController?.Jump();
        }
    }

    public void Select()
    {
        PointerEventData cursor = new PointerEventData(EventSystem.current);
        cursor.position = Selector.transform.position;
        List<RaycastResult> objectsHit = new List<RaycastResult>();
        EventSystem.current.RaycastAll(cursor, objectsHit);

        foreach (RaycastResult rr in objectsHit)
        {
            Transform CharacterNameTag = rr.gameObject.transform.Find("CharacterNameTag");
            if (CharacterNameTag != null)
            {
                Character = CharacterNameTag.GetComponent<TextMeshProUGUI>().text;
                GameObject.Find("CharacterSelect").GetComponent<CharacterSelectionController>().UpdateSelectedCharacter(this.transform.GetComponent<PlayerInput>(), Character);
                Debug.Log($"Player selected character {CharacterNameTag.GetComponent<TextMeshProUGUI>().text} ");

                return;
            }

            Transform StageNameTag = rr.gameObject.transform.Find("CharacterNameTag");
            if(StageNameTag != null)
            {
                GameControllerScript.stageName = StageNameTag.GetComponent<TextMeshProUGUI>().text;
                GameObject.Find("SelectedStage").GetComponent<TextMeshProUGUI>().text = $"{GameControllerScript.stageName}";
                return;
            }

            Button btn = rr.gameObject.GetComponent<Button>();
            if(btn != null)
            {
                if (rr.gameObject.name == "btn_stage_select" && (GameControllerScript.players.Count < 2 || !GameControllerScript.DoesEveryPlayerHaveCharacter())) return;
                if (rr.gameObject.name == "btn_match_start" && GameControllerScript.stageName == "") return;
                btn.onClick.Invoke();
                return;
            }
        }
    }

    public void OnLightAttack(InputValue value)
    {
        Debug.Log("LightAttack");
        if (GameControllerScript.GetGameState() == "match_active")
        {
            this.CharacterController?.Attack(false);
        }
    }

    public void OnHeavyAttack(InputValue value)
    {
        if (GameControllerScript.GetGameState() == "match_active")
        {
            this.CharacterController?.Attack(true);
        }
    }

    public void OnStart(InputValue value)
    {
    }

    public void OnSelect(InputValue value)
    {
    }

    bool rectOverlaps(RectTransform rectTrans1, RectTransform rectTrans2)
    {
        Rect rect1 = new Rect(rectTrans1.localPosition.x, rectTrans1.localPosition.y, rectTrans1.rect.width, rectTrans1.rect.height);
        Rect rect2 = new Rect(rectTrans2.localPosition.x, rectTrans2.localPosition.y, rectTrans2.rect.width, rectTrans2.rect.height);

        return rect1.Overlaps(rect2);
    }
}
