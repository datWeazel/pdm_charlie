using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Junkbot
{
    public class GameCharacterController : CharacterControllerBase
    {
        private Animator animator;
        public GameObject lightAttackHitBox;

        // Start is called before the first frame update
        void Start()
        {
            this.animator = GetComponent<Animator>();
        }

        void Update()
        {
            base.Update();
            Debug.Log($"Update: {this.isAttacking}");
            if (this.animator != null)
            {
                // Set animator variables
                this.animator.SetBool("moving", this.isMoving);
                this.animator.SetBool("jumping", this.isJumping);
                this.animator.SetBool("attacking", this.isAttacking);
            }

            // Reset animator help variables for next frame
            this.isJumping = !this.isGrounded;
            this.isMoving = false;
        }
        //@TODO: outdated
        public override void LightAttack()
        {
            this.lightAttackHitBox.SetActive(true);

            if (!this.lightAttackHitBox.GetComponentInChildren<AttackHitboxControllerBase>().isExpanding)
            {
                this.lightAttackHitBox.GetComponentInChildren<AttackHitboxControllerBase>().StartAttackHitbox();
                this.isAttacking = true;
            }
        }
    }

}
