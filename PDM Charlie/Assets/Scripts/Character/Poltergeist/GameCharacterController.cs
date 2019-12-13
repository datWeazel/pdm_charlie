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
        
        public GameObject big;
        public GameObject small;
        public GameObject bottle;

        private SpringJoint thisJoint;
        private SpringJoint bigJoint;
        private SpringJoint smallJoint;


        //Store settings from Prefab parts
        public float jointsMaxDistance;

        //Attack
        private List<PlayerController> hitPlayers = new List<PlayerController>();


        //Variables for Heavy attack
        public float chargeUpTime = 0.0f;
        public float minChargeUpTime = 0.5f;
        public float maxChargeUpTime = 2.5f;
        public float chargeFactor = 5.0f;
        public float heavyPercentageDamage = 0.3f;
        public bool heavyAttackColliderActive = false;
        public float heavyAttackStrength = 250.0f;
        public float heavyAttackHitStunDuration = 0.35f;

        // Start is called before the first frame update
        void Start()
        {
            //Instantiate
            big = Instantiate(big, this.transform.position, new Quaternion());
            small = Instantiate(small, this.transform.position, new Quaternion());
            bottle = Instantiate(bottle, this.transform.position, new Quaternion());
            thisJoint = this.GetComponent<SpringJoint>();
            bigJoint = big.GetComponent<SpringJoint>();
            smallJoint = small.GetComponent<SpringJoint>();

            //Attach GameObjects to another (this - big - small - bottle)
            thisJoint.connectedBody = big.GetComponent<Rigidbody>();
            bigJoint.connectedBody = small.GetComponent<Rigidbody>();
            smallJoint.connectedBody = bottle.GetComponent<Rigidbody>();

            //Make Parts not collide with another
            Physics.IgnoreCollision(this.GetComponent<Collider>(), bottle.GetComponent<Collider>());

            //Get SpringJoint default configuration
            joints[0] = thisJoint;
            joints[1] = bigJoint;
            joints[2] = smallJoint;
            particles[0] = this.GetComponentInChildren<ParticleSystem>();
            particles[1] = big.GetComponentInChildren<ParticleSystem>();
            particles[2] = small.GetComponentInChildren<ParticleSystem>();
        }

        new void Update()
        {
            base.Update();
        }

        new void FixedUpdate()
        {
            base.FixedUpdate();
            //Debug.Log($"Update: {this.isAttacking}");
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
            /*foreach (ParticleSystem particle in particles)
            {
                particle.Stop();
            }*/
            movementStopped = true;
            
            chargeUpTime = 0;
            //bottle.GetComponent<Rigidbody>().isKinematic = true;
        }

        public override void HeavyAttackHold()
        {
            chargeUpTime += Time.deltaTime;
            
            bottle.GetComponent<Rigidbody>().rotation = Quaternion.Euler(movementVector.x, movementVector.y, 0);
        }

        public override void HeavyAttackRelease()
        {
            movementStopped = false;

            //bottle.GetComponent<Rigidbody>().isKinematic = false;


            //Cancel attack when minChargeupTime not reached
            if (chargeUpTime < minChargeUpTime) return;

            //Ceil chargeUpTime
            if (chargeUpTime > maxChargeUpTime) chargeUpTime = maxChargeUpTime;

            this.currentRigidbody.AddForce(this.movementVector.x * chargeUpTime * chargeFactor, this.movementVector.y * chargeUpTime * chargeFactor, 0, ForceMode.Impulse);

            heavyAttackColliderActive=true;

            /*foreach (ParticleSystem particle in particles)
            {
                particle.Play();
            }

            //createHitBoxOnSelf();

            Debug.Log("chargeUp " + chargeUpTime);
            Debug.Log("mvVec.x " + this.movementVector.x);
            Debug.Log("mvVec.y " + this.movementVector.y);*/

            StartCoroutine(Wait(0.4f));

            
        }

        IEnumerator Wait(float t)
        {
            yield return new WaitForSecondsRealtime(t);

            heavyAttackColliderActive = false;
            hitPlayers.Clear();
        }

        private new void OnCollisionEnter(Collision col)
        {
            base.OnCollisionEnter(col);
            Debug.Log("Geist Col detect");
            Collider entity = col.collider;
            if (heavyAttackColliderActive)
            {
                Debug.Log("Geist Col detect heavy active");
                // Check if entity that entered the hitbox collider is a Character and is not the player that attacked
                if (entity.transform.tag == "Character")
                {
                    Debug.Log("Geist Col detect char col");
                    //@todo: player is null
                    PlayerController player = col.gameObject.transform.GetComponentInParent<PlayerController>();
                    Debug.Log(player);
                    if (!this.hitPlayers.Contains(player))
                    {
                        this.hitPlayers.Add(player);
                        Debug.Log("Geist Col detect players: " + hitPlayers);
                        player.percentage += heavyPercentageDamage;

                        Vector3 direction = entity.transform.position - this.transform.position;
                        player.characterController.AddForce((direction * (((player.percentage + 1.0f) / 100.0f) * heavyAttackStrength)));
                        player.characterController.SetHitStun(heavyAttackHitStunDuration);
                    }

                }
            }
        }

        #endregion

        #region Callbacks

        public override void OnBecomingPassThroughPlatform()
        {
            bottle.gameObject.layer = 8;
        }

        public override void OnLosingPassThroughPlatform()
        {
            bottle.gameObject.layer = 0;
        }

        public override void OnCharacterDying()
        {
            bottle.transform.position = this.transform.position;
        }

        #endregion
    }
}