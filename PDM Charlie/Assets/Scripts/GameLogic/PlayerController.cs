using System;
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
    public (Vector3 position, Quaternion rotation) spawn = (new Vector3(), new Quaternion());

    public GameObject selector = null;
    public float selectorSpeed = 1.0f;
    private GameObject gameController = null;
    private GameController gameControllerScript = null;

    public string character = "";
    public CharacterControllerBase characterController = null;

    public PlayerMatchInfoController matchHUD = null;

    private Vector2 moveVector = new Vector2();

    private bool lightAttackHold = false;

    private Button hoveredButton = null;
    private Transform hoveredCharacterSelector = null;
    private Transform hoveredStageSelector = null;

    public LayerMask charSelectLayerMask;

    public int respawnDelayMS = 5000;
    public GameObject deathEffectPrefab;

    public bool isDead = false;
    private float respawnTimer = 0;
    private float respawnTimerWas = 0;

    private GameObject deathEffect;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);

        this.gameController = GameObject.Find("GameController");
        this.gameControllerScript = gameController.GetComponent<GameController>();

        this.selector.GetComponentInChildren<TextMeshProUGUI>().text = $"P {this.Id}";
    }

    private void FixedUpdate()
    {
        if (isDead && this.gameControllerScript.GetGameState() == "match_active")
        {
            respawnTimerWas = respawnTimer;
            matchHUD.UpdatePlayerPercentage($"{Math.Round(respawnTimer, 2)}");
            respawnTimer -= Time.deltaTime;
        }
    }


    /*
        if(Timo_Peters){
            lead_programmer = true;
            menu_design = true;
            audio_design = true;
        }
    */

    // Update is called once per frame
    void Update()
    {
        string gameState = this.gameControllerScript.GetGameState();

        if(gameState.Contains("menu_"))
        {
            if (gameState == "menu_main" & this.Id != 1)
            {
                SetSelectorState(false);
                return;
            }

            SetSelectorState(true);

            if (this.moveVector != new Vector2())
            {
                float cursorSpeed = this.selectorSpeed;
                if (lightAttackHold) cursorSpeed *= 2;
                this.selector.transform.position += new Vector3((this.moveVector.x * cursorSpeed), (this.moveVector.y * cursorSpeed), 0);
            }

            Button oldHoveredButton = hoveredButton;
            hoveredButton = null;
            hoveredCharacterSelector = null;
            hoveredStageSelector = null;

            PointerEventData cursor = new PointerEventData(EventSystem.current);
            cursor.position = this.selector.transform.position;

            List<RaycastResult> objectsHit = new List<RaycastResult>();
            EventSystem.current.RaycastAll(cursor, objectsHit);
            foreach (RaycastResult rr in objectsHit)
            {
                if (rr.gameObject.name.Contains("btn_"))
                {
                    hoveredButton = rr.gameObject.GetComponentInChildren<Button>();
                    if (oldHoveredButton != hoveredButton)
                    {
                        gameControllerScript.UI.GetComponent<MainMenuController>().HoverButton(hoveredButton);
                    }
                }

                Transform StageNameTag = rr.gameObject.transform.Find("StageNameTag");
                if (StageNameTag != null) hoveredStageSelector = StageNameTag;

                //Debug.Log($"hoveredCharacterSelector({(hoveredCharacterSelector != null)}) || hoveredStageSelector({(hoveredStageSelector != null)})");
            }

            if(hoveredButton == null)
            {
                if(oldHoveredButton != null)
                {
                    gameControllerScript.UI.GetComponent<MainMenuController>().UnHoverButton(oldHoveredButton);
                }
            }

            if (gameState == "menu_character_select")
            {
                RaycastHit[] hits;
                Ray ray = Camera.main.ScreenPointToRay(cursor.position);
                hits = Physics.RaycastAll(ray, 100.0f, charSelectLayerMask);

                foreach (RaycastHit rr in hits)
                {
                    if (rr.transform.gameObject.transform.tag == "Character") hoveredCharacterSelector = rr.transform.gameObject.transform;
                }
            }
        }
        else
        {
            SetSelectorState(false);

            if (this.matchHUD != null)
            {
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

    public void UpdatePlayerPercentage()
    {
        this.matchHUD.UpdatePlayerPercentage(this.percentage);
    }

    public void HitDeathZone()
    {
        this.stocks--;
        this.matchHUD?.UpdatePlayerStockCount(this.stocks);

        this.percentage = 0.0f;
        UpdatePlayerPercentage();

        deathEffect = Instantiate(deathEffectPrefab, this.characterController.transform.TransformPoint(new Vector3(0.0f, 0.0f, 0.5f)), Quaternion.identity); ;

        Camera.main.GetComponent<CameraLogic>()?.RemovePlayerFromCam(this.characterController.transform);

        if (this.stocks == 0)
        {
            GameObject.Destroy(this.characterController.gameObject);
            this.characterController = null;
            return;
        }

        StartCoroutine(RemoveDeathEffect());
        StartCoroutine(RespawnPlayer(respawnDelayMS));
    }

    IEnumerator RemoveDeathEffect()
    {
        yield return new WaitForSeconds(1.5f);
        GameObject.Destroy(deathEffect);
    }

    IEnumerator RespawnPlayer(int delayMS)
    {
        //Debug.Log("Respawn incoming. :)");
        this.isDead = true;
        this.respawnTimer = (respawnDelayMS/1000);

        yield return new WaitForSeconds(delayMS / 1000);

        Camera.main.GetComponent<CameraLogic>()?.AddPlayerToCam(this.characterController.transform);

        UpdatePlayerPercentage();
        this.isDead = false;
        this.characterController.SetPosition(this.spawn.position);
        this.characterController.SetRotation(this.spawn.rotation);
        this.characterController.OnCharacterDying();
    }

    public GameObject CreateCharacter(GameObject characterPrefab, (Vector3 position, Quaternion rotation) spawnLocation)
    {
        GameObject character = Instantiate(characterPrefab, this.transform);
        this.spawn = spawnLocation;

        character.transform.position = this.spawn.position;
        character.transform.rotation = this.spawn.rotation;

        this.characterController = character.GetComponent<CharacterControllerBase>();

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
        if (this.gameControllerScript == null) return;
        if(this.gameControllerScript.GetGameState().Contains("menu_"))
        {
            Select();
        }
    }

    public void OnJump(InputValue value)
    {
        if (this.gameControllerScript == null) return;
        if (this.gameControllerScript.GetGameState().Contains("menu_"))
        {
            Select();
        }
        else if (this.gameControllerScript.GetGameState() == "match_active")
        {
            this.characterController?.JumpButtonPressed();
        }
    }

    public void OnJumpRelease(InputValue value)
    {
        this.characterController?.JumpButtonReleased();
    }

    public void Select()
    {
        if(hoveredCharacterSelector != null)
        {
            this.character = hoveredCharacterSelector.name;
            GameObject.Find("CharacterSelect").GetComponent<CharacterSelectionController>().UpdateSelectedCharacter(this.transform.GetComponent<PlayerInput>(), this.character);
            return;
        }

        if(hoveredStageSelector != null)
        {
            this.gameControllerScript.stageName = hoveredStageSelector.GetComponent<TextMeshProUGUI>().text;
            GameObject.Find("SelectedStage").GetComponent<TextMeshProUGUI>().text = $"{this.gameControllerScript.stageName}";
            return;
        }

        if (hoveredButton != null)
        {

            if (hoveredButton.gameObject.name == "btn_stage_select") {
                if (!this.gameControllerScript.DoesEveryPlayerHaveCharacter()) return;


                if(this.gameControllerScript.players.Count < 2)
                {
                    this.gameControllerScript.rules.gameMode = 1;
                }
            }
            if (hoveredButton.gameObject.name == "btn_match_start" && this.gameControllerScript.stageName == "") return;

            gameControllerScript.UI.GetComponent<MainMenuController>().ClickButton(hoveredButton);
            return;
        }
    }

    public void OnLightAttack(InputValue value)
    {
        lightAttackHold = true;
        if (this.gameControllerScript?.GetGameState() == "match_active")
        {
            this.characterController?.LightAttackButtonPressed();
            //Debug.Log("light atk");
        }
    }

    public void OnLightAttackRelease(InputValue value)
    {
        lightAttackHold = false;
        if (this.gameControllerScript?.GetGameState() == "match_active")
        {
            this.characterController?.LightAttackButtonReleased();
            //Debug.Log("light atk release");
        }
    }

    public void OnHeavyAttack(InputValue value)
    {
        if (this.gameControllerScript.GetGameState() == "match_active")
        {
            this.characterController?.HeavyAttackButtonPressed();
        }
    }

    public void OnHeavyAttackRelease(InputValue value)
    {
        if (this.gameControllerScript.GetGameState() == "match_active")
        {
            this.characterController?.HeavyAttackButtonReleased();
        }
    }

    public void OnStart(InputValue value)
    {
        if(this.gameControllerScript?.gameState == "menu_match_end")
        {
            this.gameControllerScript.LoadScene("MainMenu_Room");
        }
    }

    public void OnSelect()
    {
        string gameState = this.gameControllerScript.gameState;
        if(gameState == "menu_main" || gameState == "menu_character_select")
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    public void SetSelectorState(bool isActive)
    {
        if (this.selector.activeSelf != isActive) this.selector.SetActive(isActive);
    }
}
