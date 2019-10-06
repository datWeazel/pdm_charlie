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
    public string GameState = "";

    public MatchRules rules = null;
    public string stageName = "";

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        GameState = "main_menu";
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
        if (camLogic != null) camLogic.RemovePlayerFromCam(player.CharacterController.transform);

        if (this.rules.team_size == 1)
        {
            if (players.Count == 1)
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
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

        GameState = "match_prepare";
        SceneManager.LoadScene(this.stageName);

        
        StartCoroutine(StartMatch(3));
    }

    IEnumerator StartMatch(int waitSeconds)
    {
        yield return new WaitForSeconds(waitSeconds);
        GameState = "match_active";

        foreach (PlayerController player in players)
        {
            GameObject character = characterPrefabs.FirstOrDefault(p => p.name == player.Character);
            if (character != null)
            {
                GameObject c = player.CreateCharacter(character, new Vector3(-7.37f, 1.8f, -0.12f));
                player.Stocks = this.rules.stocks;
                player.MatchHUD = GetPlayerMatchInfoController(player.Id);
                player.MatchHUD.ActivateParent();
                player.MatchHUD.UpdatePlayerName($"P{player.Id}");
                player.MatchHUD.UpdatePlayerStockCount(player.Stocks);
            }
        }

        Debug.Log("Match started!");
    }

    public void SetGameState(string state)
    {
        GameState = state;
    }

    public string GetGameState()
    {
        return this.GameState;
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

    public bool DoesEveryPlayerHaveCharacter()
    {
        foreach (PlayerController player in players)
        {
            if (player.Character == "") return false;
        }

        return true;
    }
}
