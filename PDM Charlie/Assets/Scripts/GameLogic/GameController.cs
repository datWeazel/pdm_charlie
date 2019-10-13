using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject UI = null;
    public List<PlayerController> players;
    public List<GameObject> characterPrefabs;
    public string gameState = "";

    public MatchRules rules = null;
    public string stageName = "";

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        gameState = "main_menu";
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void AddPlayer(PlayerController player)
    {
        if (!players.Contains(player))
        {
            players.Add(player);
        }
    }

    public void RemovePlayer(PlayerController player)
    {
        if (players.Contains(player))
        {
            players.Remove(player);
        }

        CameraLogic camLogic = Camera.main.GetComponent<CameraLogic>();
        if (camLogic != null) camLogic.RemovePlayerFromCam(player.characterController.transform);

        if (this.rules.teamSize == 1)
        {
            if (players.Count == 1)
            {
                EndMatch(players);
            }
        }
    }

    public void EndMatch(List<PlayerController> winners)
    {
        string endScreenText = "";
        foreach(PlayerController p in winners)
        {
            endScreenText += $"P{p.Id} ";
        }

        endScreenText += "WINS!";
        MatchHUDController matchHUDController = GameObject.Find("MATCH_HUD").GetComponent<MatchHUDController>();
        matchHUDController.UpdateEndScreenText(endScreenText);
        matchHUDController.SetEndScreenVisible(true);
        this.gameState = "match_end";
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void UpdateSelectedStage(string stage)
    {
        this.stageName = stage;
        GameObject.Find("SelectedStage").GetComponent<TextMeshProUGUI>().text = $"{this.stageName}";
    }

    public void RaiseStockCount()
    {
        this.rules.stocks++;
        if (this.rules.stocks > 999) this.rules.stocks = 999;
        GameObject.Find("StocksCount").GetComponent<TextMeshProUGUI>().text = $"{this.rules.stocks}";
    }

    public void LowerStockCount()
    {
        this.rules.stocks--;
        if (this.rules.stocks < 1) this.rules.stocks = 1;
        GameObject.Find("StocksCount").GetComponent<TextMeshProUGUI>().text = $"{this.rules.stocks}";
    }

    public void UpdateMatchRules(MatchRules rules)
    {
        this.rules = rules;
    }

    public void PrepareMatch()
    {
        if (this.stageName == "") return;

        gameState = "match_prepare";
        SceneManager.LoadScene(this.stageName);

        
        StartCoroutine(StartMatch(3));
    }

    IEnumerator StartMatch(int waitSeconds)
    {
        yield return new WaitForSeconds(waitSeconds);
        gameState = "match_active";

        foreach (PlayerController player in players)
        {
            GameObject character = characterPrefabs.FirstOrDefault(p => p.name == player.character);
            if (character != null)
            {
                GameObject c = player.CreateCharacter(character, GetPlayerStageSpawn(player.Id));
                player.stocks = this.rules.stocks;
                player.matchHUD = GetPlayerMatchInfoController(player.Id);
                player.matchHUD.ActivateParent();
                player.matchHUD.UpdatePlayerName($"P{player.Id}");
                player.matchHUD.UpdatePlayerStockCount(player.stocks);
                Camera.main.GetComponent<CameraLogic>()?.AddPlayerToCam(c.transform);
            }
        }

        Debug.Log("Match started!");
    }

    public void SetGameState(string state)
    {
        gameState = state;
    }

    public string GetGameState()
    {
        return this.gameState;
    }

    public PlayerMatchInfoController GetPlayerMatchInfoController(int id)
    {
        GameObject[] matchHUDs = GameObject.FindGameObjectsWithTag("PlayerMatchHUD");
        Debug.Log($"matchHUDs Count: {matchHUDs.Length}");
        foreach(GameObject matchHUD in matchHUDs)
        {
            if (matchHUD.name == $"PlayerMatchHUD_{id}")
            {
                //return matchHUD.GetComponent<PlayerMatchInfoController>();
                return matchHUD.GetComponentInChildren<PlayerMatchInfoController>(true);
            }
        }

        return null;
    }

    public Vector3 GetPlayerStageSpawn(int id)
    {
        GameObject[] spawns = GameObject.FindGameObjectsWithTag("StageSpawn");
        foreach (GameObject spawn in spawns)
        {
            if (spawn.name == $"Spawn_{id}")
            {
                //return matchHUD.GetComponent<PlayerMatchInfoController>();
                return spawn.transform.position;
            }
        }

        return new Vector3();
    }

    public bool DoesEveryPlayerHaveCharacter()
    {
        foreach (PlayerController player in players)
        {
            if (player.character == "") return false;
        }

        return true;
    }
}
