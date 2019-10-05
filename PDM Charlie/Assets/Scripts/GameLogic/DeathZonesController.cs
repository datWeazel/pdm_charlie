using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZonesController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Character")
        {
            other.GetComponentInParent<PlayerController>().HitDeathZone();
        }
    }
}
