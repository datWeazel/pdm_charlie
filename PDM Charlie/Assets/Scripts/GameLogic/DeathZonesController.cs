using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZonesController : MonoBehaviour
{
    public void OnTriggerEnter(Collider entity)
    {
        if (entity.tag == "Character")
        {
            entity.GetComponentInParent<PlayerController>()?.HitDeathZone();
        }
    }
}
