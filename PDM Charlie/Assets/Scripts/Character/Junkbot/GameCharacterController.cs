using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCharacterController : MonoBehaviour
{
    public float speed = 0.1f;
    public float jumpHeight = 10.0f;
    private Animator animator;
    private Rigidbody rigidbody;

    private bool moving = false;
    private bool jumping = false;
    private bool attacking = false;
    private Vector2 movementVector;

    private float distToGround;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        distToGround = GetComponent<BoxCollider>().bounds.extents.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            Vector3 newVelocity = new Vector3(movementVector.x*speed, rigidbody.velocity.y, 0);
            rigidbody.velocity = newVelocity;
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
            Vector3 newVelocity = new Vector3(0, jumpHeight, 0);
            this.transform.GetComponent<Rigidbody>().velocity = newVelocity;
            jumping = true;
        }
    }

    public void Move(Vector2 movementVector)
    {
        if (movementVector == new Vector2()) return;
        moving = true;
        this.movementVector = movementVector;
    }

    public bool IsGrounded(){
       return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
     }
}
