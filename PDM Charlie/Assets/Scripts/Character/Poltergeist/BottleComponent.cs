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
        Debug.Log("col bottle");
        if (!isColliding) return;
        Collider entity = col.collider;
        Debug.Log(entity);
        if (entity.transform.tag == "Character")
        {
            Debug.Log("is char");
            PlayerController player = entity.GetComponentInParent<PlayerController>();
            if (!owner.hitPlayersLight.Contains(player))
            {
                owner.hitPlayersLight.Add(player);
                player.percentage += owner.lightPercentageDamage;
                player.UpdatePlayerPercentage();

                //Debug.Log("damage bottle");
                Vector3 direction = entity.transform.position - this.transform.position;
                player.GetComponentInChildren<Rigidbody>().AddForce((direction * (((player.percentage + 1.0f) / 100.0f) * owner.lightAttackStrength)), ForceMode.Impulse);
                player.characterController.SetHitStun(owner.lightAttackHitStunDuration);
            }
        }
        else if (entity.transform.tag == "PracticeTarget")
        {
            entity.transform.GetComponent<PracticeTarget>().OnHit();
            this.transform.GetComponentInParent<PlayerController>().matchHUD.UpdatePlayerPercentage($"{entity.transform.GetComponent<PracticeTarget>().hitCount}");
        }
    }
}
