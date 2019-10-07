using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCharacterController : MonoBehaviour
{
    public float speed = 2.0f;
    public float aerialSpeed = 1.0f;
    public float jumpHeight = 10.0f;
    public float jumpForce = 1.0f;
    public float downForce = 0.1f;
    private Animator animator;

    private bool moving = false;
    private bool jumping = false;
    private bool attacking = false;
    private Vector2 movementVector;

    private float distToGround;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        distToGround = GetComponent<BoxCollider>().bounds.extents.y;
    }

    private void FixedUpdate()
    {
        GetComponent<Rigidbody>().AddForce(-Vector3.up * downForce);
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody r = GetComponent<Rigidbody>();
        if (moving)
        {
            if (movementVector.x < 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if(movementVector.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            float move_speed = speed;
            if (!IsGrounded()) move_speed = aerialSpeed;
            Vector3 newVelocity = new Vector3(movementVector.x * move_speed, r.velocity.y, 0);
            r.velocity = newVelocity;
        }

        animator.SetBool("moving", moving);
        animator.SetBool("jumping", jumping);
        animator.SetBool("attacking", attacking);

        if (IsGrounded())
        {
            jumping = false;
        }

        moving = false;
        attacking = false;
    }

    public void Attack(bool heavy)
    {
        attacking = true;
    }

    public void Jump()
    {
        if (IsGrounded())
        {
            if (!this.moving)
            {
                Vector3 newVelocity = new Vector3(0, jumpHeight, 0);
                this.transform.GetComponent<Rigidbody>().velocity = newVelocity;
            }
            else
            {
                Rigidbody r = GetComponent<Rigidbody>();
                r.AddForce(new Vector3(jumpForce * movementVector.x, jumpHeight, 0));
            }
            jumping = true;
        }
    }

    public void Move(Vector2 movementVector)
    {
        if (movementVector == new Vector2()) return;
        moving = true;
        this.movementVector = movementVector;
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }

    public void SetPosition(Vector3 position)
    {
        this.transform.position = position;
        GetComponent<Rigidbody>().velocity = new Vector3();
    }
}
