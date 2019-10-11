using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitboxController : MonoBehaviour
{
    public float startRadius = 4.0f;
    public float endRadius = 6.0f;
    public float expansionSpeed = 1.0f;
    public float strength = 150.0f;
    public float hitStunDuration = 0.35f;

    public GameObject parent;
    public GameObject character;

    public bool isExpanding = false;

    private SphereCollider collider;
    private List<PlayerController> hitPlayers = new List<PlayerController>();

    private void Start()
    {
        collider = GetComponent<SphereCollider>();
    }


    // Update is called once per frame
    void Update()
    {
        if (this.isExpanding)
        {
            // If collider is set to expand, raise the collider radius until it hit's the maximum radius
            this.collider.radius += this.expansionSpeed * Time.deltaTime;
            if(this.collider.radius >= this.endRadius)
            {
                EndAttackHitbox();
            }
        }
    }

    /// <summary>
    /// Starts the expansion of the attack hitbox
    /// </summary>
    public void StartAttackHitbox()
    {
        this.isExpanding = true;
    }

    /// <summary>
    /// Ends the attack hitbox, clears hit players and deactivates the attack effect gameObject
    /// </summary>
    public void EndAttackHitbox()
    {
        this.collider.radius = this.startRadius;
        this.hitPlayers.Clear();
        this.isExpanding = false;
        this.parent.SetActive(false);
    }

    private void OnTriggerEnter(Collider entity)
    {
        // Check if entity that entered the hitbox collider is a Character and is not the player that attacked
        if(entity.transform.tag == "Character" && entity.gameObject != this.character)
        {
            PlayerController player = entity.transform.GetComponentInParent<PlayerController>();
            if (!this.hitPlayers.Contains(player))
            {
                this.hitPlayers.Add(player);

                Vector3 direction = entity.transform.position - this.character.transform.position;
                player.characterController.AddForce((direction * this.strength));
                player.characterController.SetHitStun(this.hitStunDuration);
            }
        }
    }
}
