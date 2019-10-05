using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    public int Id = -1;
    public GameObject Selector = null;
    public float SelectorSpeed = 1.0f;
    private GameObject GameController = null;
    private GameController GameControllerScript = null;
    public string Character = "";
    public GameCharacterController CharacterController = null;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        GameController = GameObject.Find("GameController");
        GameControllerScript = GameController.GetComponent<GameController>();
        Selector.GetComponentInChildren<TextMeshProUGUI>().text = "Player " + Id;
    }

    // Update is called once per frame
    void Update()
    {
        var gamepad = Gamepad.current;

        if (GameControllerScript.GetGameState() == "character_select")
        {
            if (!Selector.activeSelf) Selector.SetActive(true);

            Selector.transform.position += new Vector3((gamepad.leftStick.x.ReadValue() * SelectorSpeed), (gamepad.leftStick.y.ReadValue() * SelectorSpeed), 0);
        }
        else
        {
            if (Selector.activeSelf) Selector.SetActive(false);
            if (GameControllerScript.GetGameState() == "match_active")
            {
                this.CharacterController?.Move(new Vector2(gamepad.leftStick.x.ReadValue(), gamepad.leftStick.y.ReadValue()));
            }
        }
    }

    public GameObject CreateCharacter(GameObject characterPrefab, Vector3 position)
    {
        Debug.Log($"Creating character {characterPrefab.name} at position {position}");
        GameObject character = Instantiate(characterPrefab, this.transform);
        character.transform.position = position;
        this.CharacterController = character.GetComponent<GameCharacterController>();

        return character;
    }

    public void OnMove(InputValue value)
    {
        //Vector2 analogVector = value.Get<Vector2>();
        //Debug.Log($"Player move! ({analogVector.ToString()})");
    }

    public void OnJump(InputValue value)
    {
        if(GameControllerScript.GetGameState() == "character_select")
        {
            PointerEventData cursor = new PointerEventData(EventSystem.current);
            cursor.position = Selector.transform.position;
            List<RaycastResult> objectsHit = new List<RaycastResult>();
            EventSystem.current.RaycastAll(cursor, objectsHit);

            foreach(RaycastResult rr in objectsHit)
            {
                Transform NameTag = rr.gameObject.transform.Find("NameTag");
                if (NameTag != null)
                {
                    Character = NameTag.GetComponent<TextMeshProUGUI>().text;
                    GameObject.Find("CharacterSelect").GetComponent<CharacterSelectionController>().UpdateSelectedCharacter(this.transform.GetComponent<PlayerInput>(), Character);
                    Debug.Log($"Player selected character {NameTag.GetComponent<TextMeshProUGUI>().text} ");
                }
            }
        }
        else if (GameControllerScript.GetGameState() == "match_active")
        {
            this.CharacterController?.Jump();
        }
    }

    public void OnLightAttack(InputValue value)
    {
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
