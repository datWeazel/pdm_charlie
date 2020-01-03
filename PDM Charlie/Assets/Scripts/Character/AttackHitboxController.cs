using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitboxControllerBase : MonoBehaviour
{
    public float startRadius = 4.0f;
    public float endRadius = 6.0f;
    public float expansionSpeed = 1.0f;
    public float strength = 150.0f;
    public float hitStunDuration = 0.35f;
    public float percentageDamage = 0.5f;

    public GameObject parent;
    public GameObject character;

    public bool isExpanding = false;

    private SphereCollider sphereCollider;
    private List<PlayerController> hitPlayers = new List<PlayerController>();

    public AudioSource audioSource;

    public AudioClip[] attackSounds;
    public AudioClip[] hitSounds;

    private void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (this.isExpanding)
        {
            // If collider is set to expand, raise the collider radius until it hit's the maximum radius
            this.sphereCollider.radius += this.expansionSpeed * Time.deltaTime;
            if (this.sphereCollider.radius >= this.endRadius)
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
        PlaySound(attackSounds[Random.Range(0, attackSounds.Length-1)], false);
    }

    /// <summary>
    /// Ends the attack hitbox, clears hit players and deactivates the attack effect gameObject
    /// </summary>
    public void EndAttackHitbox()
    {
        this.sphereCollider.radius = this.startRadius;
        this.hitPlayers.Clear();
        this.isExpanding = false;
        this.parent.SetActive(false);
        PlayerController player = this.parent.transform.GetComponentInParent<PlayerController>();
        player.characterController.isAttacking = false;

    }

    private void OnTriggerEnter(Collider entity)
    {
        Debug.Log($"EntityHitByAttack: (name: {entity.transform.name} | tag: {entity.transform.tag})");
        // Check if entity that entered the hitbox collider is a Character and is not the player that attacked
        if (entity.transform.tag == "Character" && entity.gameObject != this.character)
        {
            PlayerController player = entity.transform.GetComponentInParent<PlayerController>();
            if (!this.hitPlayers.Contains(player))
            {
                this.hitPlayers.Add(player);

                player.percentage += this.percentageDamage;
                player.UpdatePlayerPercentage();

                Vector3 direction = entity.transform.position - this.character.transform.position;
                player.characterController.AddForce((direction * (((player.percentage+1.0f)/100.0f) * this.strength)));
                player.characterController.SetHitStun(this.hitStunDuration);
            }
        }
        else if(entity.transform.tag == "PracticeTarget")
        {
            entity.transform.GetComponent<PracticeTarget>().OnHit();
            this.transform.GetComponentInParent<PlayerController>().matchHUD.UpdatePlayerPercentage($"{entity.transform.GetComponent<PracticeTarget>().hitCount}");
        }
    }

    public void PlaySound(AudioClip clip, bool loop)
    {
        if(audioSource == null || clip == null) return;
        
        if (audioSource.clip == clip && audioSource.loop == loop && audioSource.isPlaying) return; // Don't restart clip if it's still active and looping
        if (audioSource.clip != clip && !audioSource.loop && audioSource.isPlaying) return; //Don't override currently playing clip if it's is set to play once

        audioSource.loop = loop;
        audioSource.clip = clip;
        audioSource.Play();
    }
}
