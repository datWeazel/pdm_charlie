using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionController : MonoBehaviour
{
    public GameObject GameController = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 1; i <= 4; i++){
            if (Input.GetButtonUp("Joy_Start_" + i))
            {
                GameController GameCtrl = GameController.GetComponent<GameController>();
                if(!GameCtrl.DoesPlayerExist(i))
                {
                    GameCtrl.AddPlayer(i);
                }
                else{
                    GameCtrl.RemovePlayer(i);
                }
            }  
        }
    }
}
