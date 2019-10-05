using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        if (this.rules.team_size == 1)
        {
            if (players.Count == 1)
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
    }

    public void UpdateMatchRules(MatchRules rules, string stage)
    {
        this.rules = rules;
        this.stageName = stage;
    }

    public void PrepareMatch()
    {
        GameState = "match_prepare";
        SceneManager.LoadScene(this.stageName);
        foreach(PlayerController player in players)
        {
            GameObject character = characterPrefabs.FirstOrDefault(p => p.name == player.Character);
            if(character != null)
            {
                GameObject c = player.CreateCharacter(character, new Vector3(-7.37f, 1.8f, -0.12f));
                player.Stocks = this.rules.stocks;
            }
        }
        StartCoroutine(StartMatch(3));
    }

    IEnumerator StartMatch(int waitSeconds)
    {
        yield return new WaitForSeconds(waitSeconds);
        GameState = "match_active";
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
}
