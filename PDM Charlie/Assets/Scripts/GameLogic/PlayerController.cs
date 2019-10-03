using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMove(InputValue value)
    {
        Debug.Log($"Player move! ({value.Get<Vector2>().ToString()})");
    }

    public void OnJump(InputValue value)
    {
        Debug.Log("Player jump!");
    }

    public void OnLightAttack(InputValue value)
    {
        Debug.Log("Player lightAttack!");
    }

    public void OnHeavyAttack(InputValue value)
    {
        Debug.Log("Player heavyAttack!");
    }

    public void OnStart(InputValue value)
    {
        Debug.Log("Player start!");
    }

    public void OnSelect(InputValue value)
    {
        Debug.Log("Player select!");
    }
}
