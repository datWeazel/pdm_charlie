using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSelectionController : MonoBehaviour
{
    public GameObject GameController = null;
    public GameObject Playerlist = null;
    public List<GameObject> PlayerContainer;
    private Dictionary<PlayerInput, GameObject> ContainerToPlayer = new Dictionary<PlayerInput, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        MatchRules rules = new MatchRules();
        GameController.GetComponent<GameController>().UpdateMatchRules(rules, "MayanStage");
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnPlayerJoined(PlayerInput player)
    {
        if (!ContainerToPlayer.ContainsKey(player))
        {
            GameObject emptyContainer = GetEmptyPlayerContainer();
            if (emptyContainer != null)
            {
                ContainerToPlayer.Add(player, emptyContainer);
                player.GetComponentInParent<PlayerController>().Id = Convert.ToInt32(emptyContainer.name.Replace("PlayerContainer_", ""));
                emptyContainer.transform.Find("join_help").GetComponent<TextMeshProUGUI>().text = $"P{player.GetComponentInParent<PlayerController>().Id} Select character";
                GameController.GetComponent<GameController>().AddPlayer(player.GetComponentInParent<PlayerController>());
                Debug.Log("Player joined!");
            }
        }
    }

    public void UpdateSelectedCharacter(PlayerInput player, string character)
    {
        if (ContainerToPlayer.ContainsKey(player))
        {
            GameObject container = null;
            ContainerToPlayer.TryGetValue(player, out container);
            container.transform.Find("join_help").GetComponent<TextMeshProUGUI>().text = character;
        }
    }

    public GameObject GetEmptyPlayerContainer()
    {
        foreach(GameObject container in PlayerContainer)
        {
            if (!ContainerToPlayer.ContainsValue(container)) return container;
        }

        return null;
    }

    public void OnPlayerLeft(PlayerInput player)
    {
        if (ContainerToPlayer.ContainsKey(player))
        {
            GameObject container = null;
            ContainerToPlayer.TryGetValue(player, out container);
            container.transform.Find("join_help").GetComponent<TextMeshProUGUI>().text = "Press Start!";
            ContainerToPlayer.Remove(player);
            GameController.GetComponent<GameController>().RemovePlayer(player.GetComponentInParent<PlayerController>());
            Debug.Log("Player left!");
        }
    }
}
