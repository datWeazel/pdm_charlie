using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerBase : MonoBehaviour
{
    private Rigidbody currentRigidbody;

    public GameObject lightAttackHitBox;
    public float groundSpeed = 2.0f;
    public float aerialSpeed = 1.0f;
    public float jumpHeight = 10.0f;
    public float jumpForce = 1.0f;
    public float downForce = 0.1f;

    public bool isGrounded = false;
    public bool isMoving = false;
    public bool isJumping = false;
    public bool isAttacking = false;

    private Vector2 movementVector = new Vector2();
    private int floorCollisions = 0;
    private float hitStun = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        this.currentRigidbody = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        if (this.currentRigidbody.velocity.y <= 0 && this.gameObject.layer != 0)
        {
            this.gameObject.layer = 0;
        }

        if (this.isMoving && this.hitStun == 0)
        {
            // Pass trough platforms when pressing down
            if (this.movementVector.y <= -0.5 && this.gameObject.layer != 8) this.gameObject.layer = 8;

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
            this.currentRigidbody.velocity = new Vector3(this.movementVector.x * moveSpeed, this.currentRigidbody.velocity.y, 0);
        }
    }

    public void FixedUpdate()
    {
        Debug.Log("FIXED UPDATE");
        if (this.currentRigidbody == null) this.currentRigidbody = GetComponent<Rigidbody>();

        // Add a constant down force to the character if he is not grounded
        if (!this.isGrounded) this.currentRigidbody.AddForce(-Vector3.up * this.downForce);
    }

    #region Movement Functions
    public void Attack(bool heavy)
    {
        if (this.hitStun > 0) return;
        if (!heavy)
        {
            this.lightAttackHitBox.SetActive(true);

            if (!this.lightAttackHitBox.GetComponentInChildren<AttackHitboxControllerBase>().isExpanding)
            {
                this.lightAttackHitBox.GetComponentInChildren<AttackHitboxControllerBase>().StartAttackHitbox();
                this.isAttacking = true;
            }
        }
    }

    public void Jump()
    {
        if (this.hitStun > 0) return;
        if (this.isGrounded)
        {
            if (!this.isMoving)
            {
                this.currentRigidbody.velocity = new Vector3(0, this.jumpHeight, 0);
            }
            else
            {
                this.currentRigidbody.AddForce(new Vector3(this.jumpForce * this.movementVector.x, this.jumpHeight, 0));
            }
            this.isJumping = true;
            this.gameObject.layer = 8; // Set layer to "PassThroughPlatforms"
        }
    }

    public void Move(Vector2 movementVector)
    {
        Debug.Log($"MOVING: {movementVector.ToString()}");
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
    #endregion

    #region Callbacks
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Floor" || collision.transform.tag == "Character")
        {
            floorCollisions++;
            this.isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Floor" || collision.transform.tag == "Character")
        {
            floorCollisions--;
            if (floorCollisions == 0) this.isGrounded = false;
        }
    }
    #endregion
}
