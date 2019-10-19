using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeTransparent : MonoBehaviour
{

    public Material normal;
    public Material transparent;

    private int charsInside;

 

    // Update is called once per frame
    void Start()
    {
        GetComponent<Renderer>().material = normal;
        charsInside = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Character")
        {
            GetComponent<Renderer>().material = transparent;
            charsInside++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Character")
        {
            charsInside--;
            if (charsInside == 0)
            {
                GetComponent<Renderer>().material = normal;
            }
        }
    }
}
