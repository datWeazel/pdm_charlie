using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleComponent : MonoBehaviour
{
    public bool isColliding = false;
    private Geist.GameCharacterController owner;

    public void init(Geist.GameCharacterController gcc)
    {
        owner = gcc;
    }

    public void OnCollisionEnter(Collision col)
    {
        //Debug.Log("col bottle");
        if (!isColliding) return;
        Collider entity = col.collider;
        if (entity.transform.tag == "Character")
        {
            PlayerController player = col.gameObject.GetComponentInParent<PlayerController>();
            if (!owner.hitPlayersLight.Contains(player))
            {
                owner.hitPlayersLight.Add(player);
                if (player = null) return;
                player.percentage += owner.lightPercentageDamage;
                player.UpdatePlayerPercentage();

                //Debug.Log("damage bottle");
                Vector3 direction = entity.transform.position - this.transform.position;
                player.characterController.AddForce((direction * (((player.percentage + 1.0f) / 100.0f) * owner.lightAttackStrength)));
                player.characterController.SetHitStun(owner.lightAttackHitStunDuration);
            }

        }
    }
}
