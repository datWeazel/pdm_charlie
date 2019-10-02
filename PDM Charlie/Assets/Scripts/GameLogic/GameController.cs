using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject UI = null;
    public List<PlayerController> players;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public bool DoesPlayerExist(int ControllerId){
        return (players.FirstOrDefault(p => p.ControllerId == ControllerId) != null);
    }

    public void AddPlayer(int ControllerId){
        PlayerController player = players.FirstOrDefault(p => p.ControllerId == ControllerId);
        if(player != null) return;

        player = new PlayerController(){
            ControllerId = ControllerId
        };

        players.Add(player);
    }

    public void RemovePlayer(int ControllerId){
        PlayerController player = players.FirstOrDefault(p => p.ControllerId == ControllerId);
        if(player == null) return;

        players.Remove(player);
    }
}
