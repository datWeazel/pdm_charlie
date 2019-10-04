using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject UI = null;
    public List<PlayerController> players;
    public string GameState = "";
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
