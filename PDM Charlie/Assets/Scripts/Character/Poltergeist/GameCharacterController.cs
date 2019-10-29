using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Geist
{
    public class GameCharacterController : CharacterControllerBase
    {
        //Parts from Prefab
        private Animator animator;
        private SpringJoint[] joints;
        private ParticleSystem[] particles;
        private Rigidbody bottle;

        //Store settings from Prefab parts
        public float jointsMinDistance;
        public float jointsSpring;

        //Variables for Heavy attack
        public float chargeUpTime = 0.0f;
        public float minChargeUpTime = 0.5f;
        public float maxChargeUpTime = 2.5f;
        public float chargeFactor;

        // Start is called before the first frame update
        void Start()
        {
            joints = GetComponentsInChildren<SpringJoint>();
            jointsMinDistance = joints[0].minDistance;
            jointsSpring = joints[0].spring;
            chargeFactor = 5.0f;
            particles = GetComponentsInChildren<ParticleSystem>();
            bottle = GameObject.Find("Bottle").GetComponent<Rigidbody>();
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

        #region Heavy Attack
        public override void HeavyAttack()
        {
            foreach(SpringJoint joint in joints)
            {
                joint.minDistance = 0.0f;
                joint.spring = 100.0f;
            }
            foreach (ParticleSystem particle in particles)
            {
                var main = particle.main;
                main.maxParticles = main.maxParticles / 10;
            }
            movementStopped = true;
            bottle.isKinematic = true;
            chargeUpTime = 0;
        }

        public override void HeavyAttackHold()
        {
            chargeUpTime += Time.deltaTime;

            bottle.rotation = Quaternion.Euler(movementVector.x, movementVector.y, 0);
        }

        public override void HeavyAttackRelease()
        {
            foreach (SpringJoint joint in joints)
            {
                joint.minDistance = jointsMinDistance;
                joint.spring = jointsSpring;
            }
            foreach (ParticleSystem particle in particles)
            {
                var main = particle.main;
                main.maxParticles = main.maxParticles * 10;
            }

            movementStopped = false;
            bottle.isKinematic = true;

            //Cancel attack when minChargeupTime not reached
            if (chargeUpTime < minChargeUpTime) return;

            //Ceil chargeUpTime
            if (chargeUpTime > maxChargeUpTime) chargeUpTime = maxChargeUpTime;

            this.currentRigidbody.AddForce(this.movementVector.x * chargeUpTime * chargeFactor, this.movementVector.y * chargeUpTime * chargeFactor, 0, ForceMode.Impulse);

            //createHitBoxOnSelf();

            Debug.Log("chargeUp " + chargeUpTime);
            Debug.Log("mvVec.x " + this.movementVector.x);
            Debug.Log("mvVec.y " + this.movementVector.y);
        }

        #endregion
    }
}