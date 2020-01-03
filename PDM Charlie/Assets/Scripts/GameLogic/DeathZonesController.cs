﻿using DG.Tweening;
using UnityEngine;


public class DeathZonesController : MonoBehaviour
{
    const float cameraShakeDuration = 0.5f;
    const float cameraShakeStrength = 2.0f;

    public void OnTriggerEnter(Collider entity)
    {
        if (entity.tag == "Character")
        {
            entity.GetComponentInParent<PlayerController>()?.HitDeathZone();
            Camera.main.transform.DOShakePosition(cameraShakeDuration, cameraShakeStrength);
        }
    }
}
