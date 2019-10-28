using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Geist
{
    public class GameCharacterController : CharacterControllerBase
    {
        private Animator animator;

        private SpringJoint[] joints;
        

        // Start is called before the first frame update
        void Start()
        {
            joints = GetComponentsInChildren<SpringJoint>();
        }

        void Update()
        {
            base.Update();
            Debug.Log($"Update: {this.isAttacking}");
            if (this.animator != null)
            {
                // Set animator variables
                /*this.animator.SetBool("moving", this.isMoving);
                this.animator.SetBool("jumping", this.isJumping);
                this.animator.SetBool("attacking", this.isAttacking);*/
            }

            // Reset animator help variables for next frame
            this.isJumping = !this.isGrounded;
            this.isMoving = false;
        }

        public new void HeavyAttack()
        {
            
        }

        public override void HeavyAttackHold()
        {
            Debug.Log("heavy hold "+heavyAttackButtonHeld);
        }
    }
}