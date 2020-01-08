using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerBase : MonoBehaviour
{
    public Rigidbody currentRigidbody;

    
    public float groundSpeed = 2.0f;
    public float aerialSpeed = 1.0f;
    public float jumpHeight = 10.0f;
    public float jumpForce = 1.0f;
    public float downForce = 0.1f;

    public float timeUntilButtonHold = 0.1f;
    public float lightAttackButtonHeld = 0.0f;
    public float heavyAttackButtonHeld = 0.0f;
    public float jumpButtonHeld = 0.0f;

    private bool isLightAttackButtonPressed = false;
    private bool isHeavyAttackButtonPressed = false;
    private bool isJumpButtonPressed = false;

    public bool isGrounded = false;
    public bool isMoving = false;
    public bool isJumping = false;
    public bool isAttacking = false;

    public Vector2 movementVector = new Vector2();
    private int floorCollisions = 0;
    public float hitStun = 0.0f;
    public bool movementStopped;

    // Start is called before the first frame update
    void Start()
    {
        this.currentRigidbody = GetComponent<Rigidbody>();
    }

    public void Update()
    {

    }

    public void FixedUpdate()
    {
        if(this.hitStun > 0.0f){
            this.hitStun -= Time.deltaTime;
            if(this.hitStun < 0.0f) this.hitStun = 0.0f;
        }

        //Debug.Log("FIXED UPDATE");
        if (this.currentRigidbody == null) this.currentRigidbody = GetComponent<Rigidbody>();

        // Add a constant down force to the character if he is not grounded
        if (!this.isGrounded) this.currentRigidbody.AddForce(-Vector3.up * this.downForce);

        if (this.movementVector.y > -0.5 && this.currentRigidbody.velocity.y <= 0 && this.gameObject.layer != 0)
        {
            this.gameObject.layer = 0;
            OnLosingPassThroughPlatform();
        }

        if (this.isMoving && this.hitStun == 0)
        {
            // Pass trough platforms when pressing down
            if (this.movementVector.y <= -0.5 && this.gameObject.layer != 8)
            {
                this.gameObject.layer = 8;
                OnBecomingPassThroughPlatform();
            }
            // Rotate the character in movement direction
            if (this.movementVector.x < 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (this.movementVector.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }

            float moveSpeed = (this.isGrounded) ? this.groundSpeed : this.aerialSpeed;
            if (!movementStopped)
            {
                this.currentRigidbody.velocity = new Vector3(this.movementVector.x * moveSpeed, this.currentRigidbody.velocity.y, 0);
            }

        }

        //check if some button is held long enough to trigger ButtonHeld
        if (isLightAttackButtonPressed)
        {
            lightAttackButtonHeld += Time.deltaTime;
            if (lightAttackButtonHeld > timeUntilButtonHold) LightAttackHold();
        }
        else
        {
            lightAttackButtonHeld = 0;
        }

        if (isHeavyAttackButtonPressed)
        {
            heavyAttackButtonHeld += Time.deltaTime;
            if (heavyAttackButtonHeld > timeUntilButtonHold) HeavyAttackHold();
        }
        else
        {
            heavyAttackButtonHeld = 0;
        }

        if (isJumpButtonPressed)
        {
            jumpButtonHeld += Time.deltaTime;
            if (jumpButtonHeld > timeUntilButtonHold) JumpHold();
        }
        else
        {
            jumpButtonHeld = 0;
        }
    }

    #region HitBox

    public Collider CreateHitBoxOnSelf()
    {
        
        Collider col = Instantiate(this.GetComponent<Collider>());
        //col.isTrigger

        return col;
    }

    #endregion

    #region Movement Functions
    /*
     * These methods deal with the button presses, timing for triggering a button as held.
     * They call abstract movement functions that should be overwritten by inheriting classes, while
     * these methods themselves should not be overwritten by inheriting classes, see region Abstract Functions
     */
    public void LightAttackButtonPressed()
    {
        isLightAttackButtonPressed = true;
        if (this.hitStun > 0) return;
        LightAttack();   
    }

    public void HeavyAttackButtonPressed()
    {
        isHeavyAttackButtonPressed = true;
        if (this.hitStun > 0) return;
        HeavyAttack();
    }

    public void JumpButtonPressed()
    {
        isJumpButtonPressed = true;
        if (this.hitStun > 0) return;
        Jump();
    }

    public void LightAttackButtonReleased()
    {
        isLightAttackButtonPressed = false;
        LightAttackRelease();
    }

    public void HeavyAttackButtonReleased()
    {
        isHeavyAttackButtonPressed = false;
        HeavyAttackRelease();
    }

    public void JumpButtonReleased()
    {
        isJumpButtonPressed = false;
        JumpRelease();
    }
    #endregion

    #region Abstract Functions
    /*
     * These are mostly method stubs that are meant to be overwritten by inheriting methods
     */

    //Functions that trigger once on button press
    public virtual void LightAttack()
    {

    }

    public virtual void HeavyAttack()
    {

    }

    public virtual void Jump()
    {
        if (this.isGrounded)
        {
            if (!this.isMoving)
            {
                Debug.Log("Jump without stick moved");
                this.currentRigidbody.velocity = new Vector3(0, this.jumpHeight, 0);
            }
            else
            {
                Debug.Log($"Jump with stick moved. Stick: {this.movementVector.x}");
                this.currentRigidbody.AddForce(new Vector3(this.jumpForce * this.movementVector.x, this.jumpHeight, 0));
            }
            this.isJumping = true;
            this.gameObject.layer = 8; // Set layer to "PassThroughPlatforms"
            OnBecomingPassThroughPlatform();
        }
    }

    // The hold functions are called every frame the button is held
    public virtual void LightAttackHold()
    {

    }

    public virtual void HeavyAttackHold()
    {
        
    }

    public virtual void JumpHold()
    {

    }


    //Functioins trigger once on button release
    public virtual void LightAttackRelease()
    {

    }

    public virtual void HeavyAttackRelease()
    {

    }

    public virtual void JumpRelease()
    {

    }

    //Other movement functions
    public void Move(Vector2 movementVector)
    {
        //Debug.Log($"MOVING: {movementVector.ToString()}");
        if (this.hitStun > 0) return;
        if (movementVector == new Vector2()) return;
        this.isMoving = true;
        this.movementVector = movementVector;
    }
    #endregion

    #region Random Functions
    public void AddForce(Vector3 direction, ForceMode forceMode = ForceMode.Force)
    {
        this.currentRigidbody.AddForce(direction, forceMode);
    }

    public void SetHitStun(float duration)
    {
        this.hitStun = duration;
    }

    public void SetPosition(Vector3 position)
    {
        this.transform.position = position;
        this.currentRigidbody.velocity = new Vector3();
    }

    public void SetRotation(Quaternion rotation)
    {
        this.transform.rotation = rotation;
        this.currentRigidbody.velocity = new Vector3();
    }

    public Rigidbody GetRigidBody()
    {
        return currentRigidbody;
    }
    #endregion

    #region Callbacks
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Floor" || collision.transform.tag == "Character")
        {
            floorCollisions++;
            this.isGrounded = true;
            OnGrounded();
        }
    }
    //This stub is meant to be implemented by inheriting classes
    public virtual void OnGrounded()
    {
        
    }

    public virtual void OnBecomingPassThroughPlatform()
    {
        //Debug.Log("Became pass through");
    }

    public virtual void OnLosingPassThroughPlatform()
    {
        //Debug.Log("No longer pass through");
    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Floor" || collision.transform.tag == "Character")
        {
            floorCollisions--;
            if (floorCollisions == 0)
            {
                this.isGrounded = false;
                OnAirborne();
            }
        }
    }

    //Called when character besomes ungrounded. This stub is meant to be implemented by inheriting classes
    public virtual void OnAirborne()
    {
        
    }

    //This is called by PlayerController after respawning char. This stub is meant to be implemented by inheriting classes 
    public virtual void OnCharacterDying() 
    {
        
    }
    #endregion
}
