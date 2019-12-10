using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Junkbot
{
    public class GameCharacterController : CharacterControllerBase
    {
        private Animator animator;
        public GameObject lightAttackHitBox;
	
	public AudioSource audioOutput;
	public AudioClip idleSound;
	public AudioClip moveSound;
	public AudioClip jumpSound;
	public AudioClip gotHitSound;

        // Start is called before the first frame update
        void Start()
        {
            this.animator = GetComponent<Animator>();
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
		
		if(this.isJumping){
			//Play Jump Sound Once
		}
		else{
			if(this.isMoving){
				// Start Moving Sound Loop
			}
			else{
				if(this.isAttacking){
				}
				else{
					// Start Idle Sound Loop
				}
			}
		}
            }

            // Reset animator help variables for next frame
            this.isJumping = !this.isGrounded;
            this.isMoving = false;
        }

        //@TODO: outdated
        public override void LightAttack()
        {
            if (this.lightAttackHitBox.activeInHierarchy) return;

            Debug.Log("=============");
            Debug.Log("ATTACK!!!");
            Debug.Log("=============");
            this.lightAttackHitBox.SetActive(true);

            if (!this.lightAttackHitBox.GetComponentInChildren<AttackHitboxControllerBase>().isExpanding)
            {
                this.lightAttackHitBox.GetComponentInChildren<AttackHitboxControllerBase>().StartAttackHitbox();
                this.isAttacking = true;
            }
        }
    }

}
