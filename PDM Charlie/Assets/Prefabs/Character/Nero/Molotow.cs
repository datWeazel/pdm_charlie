using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Molotow : MonoBehaviour
{

    public Collider ownerCollider;

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject == ownerCollider)
        {
            Physics.IgnoreCollision(collision.collider, ownerCollider);
        }



        Destroy(this);
    }
}
