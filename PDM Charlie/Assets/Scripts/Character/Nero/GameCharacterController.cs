using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Nero
{
    public class GameCharacterController : CharacterControllerBase
    {
        public GameObject molotow;

        private Animator animator;

        private float heavyAttackCooldownRemaining = 0;
        private bool heavyAttackValid = true;
        public float heavyAttackCooldownTime = 6.0f;
        public float heavyAttackMolotowForce = 5.0f;
        // Start is called before the first frame update
        void Start()
        {
            
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

            //Cooldown
            heavyAttackCooldownRemaining -= Time.deltaTime;
        }

        #region Heavy Attack
        public override void HeavyAttack()
        {
            if (heavyAttackCooldownRemaining > 0) return;
            heavyAttackValid = true;            
        }

        public override void HeavyAttackHold()
        {
            

            
        }

        public override void HeavyAttackRelease()
        {
            //Don't execute if still in cooldown
            if (!heavyAttackValid) return;

            //Reset cooldown
            heavyAttackValid = false;
            heavyAttackCooldownRemaining = heavyAttackCooldownTime;

            //Instantiate a molotow and give it force
            molotow = Instantiate(molotow, this.transform.position, new Quaternion());
            
            molotow.GetComponent<Molotow>().ownerCollider = this.GetComponent<Collider>();          
            molotow.GetComponent<Rigidbody>().AddForce(this.movementVector.x * heavyAttackMolotowForce, this.movementVector.y * heavyAttackMolotowForce, 0, ForceMode.Impulse);
        }

        #endregion
    }
}