using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Junkbot
{
    public class GameCharacterController : CharacterControllerBase
    {
        private Animator animator;
        public GameObject lightAttackHitBox;

        private AudioSource audioSource;

        public AudioClip idleSound;
        public AudioClip moveSound;
        public AudioClip jumpSound;
        public AudioClip gotHitSound;

        public AudioClip[] deathSounds;

        // Start is called before the first frame update
        void Start()
        {
            this.animator = GetComponent<Animator>();
            this.audioSource = GetComponent<AudioSource>();
        }

        new void Update()
        {
            base.Update();
        }

        new void FixedUpdate()
        {
            base.FixedUpdate();

            Debug.Log($"Update: {this.isAttacking}");
            if (this.animator != null)
            {
                // Set animator variables
                this.animator.SetBool("moving", this.isMoving);
                this.animator.SetBool("jumping", this.isJumping);
                this.animator.SetBool("attacking", this.isAttacking);
            }

            if (this.isJumping)
            {
                //Play Jump Sound Once
                PlaySound(jumpSound, false);
            }
            else
            {
                if (this.isMoving)
                {
                    // Start Moving Sound Loop
                    PlaySound(moveSound, true);
                }
                else
                {
                    if (!this.isAttacking)
                    {
                        // Start Idle Sound Loop
                        PlaySound(idleSound, true);
                    }
                }
            }

            // Reset animator help variables for next frame
            this.isJumping = !this.isGrounded;
            this.isMoving = false;
        }

        public override void LightAttack()
        {
            if (this.lightAttackHitBox.activeInHierarchy) return;

            this.lightAttackHitBox.SetActive(true);

            if (!this.lightAttackHitBox.GetComponentInChildren<AttackHitboxControllerBase>().isExpanding)
            {
                this.lightAttackHitBox.GetComponentInChildren<AttackHitboxControllerBase>().StartAttackHitbox();
                this.isAttacking = true;
            }
        }

        public void PlaySound(AudioClip clip, bool loop)
        {
            if (audioSource.clip == clip && audioSource.loop == loop && audioSource.isPlaying) return; // Don't restart clip if it's still active and looping
            if (audioSource.clip != clip && !audioSource.loop && loop && audioSource.isPlaying) return; //Don't override currently playing clip if it's is set to play once

            audioSource.loop = loop;
            audioSource.clip = clip;
            audioSource.Play();
        }

        public void PlayHitSound()
        {
            PlaySound(gotHitSound, false);
        }

        public void PlayDeathSound()
        {

            PlaySound(deathSounds[Random.Range(0, deathSounds.Length-1)], false);
        }
    }

}
