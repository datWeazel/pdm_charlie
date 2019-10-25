using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjectsOnFloor : MonoBehaviour
{

    public float x = 10, y, z;

    public void OnCollisionStay(Collision collisionInfo)
    {
        Debug.Log("collision ground");
        Debug.Log(collisionInfo.collider);
        collisionInfo.collider.GetComponentInParent<Rigidbody>().AddForce(x, y, z, ForceMode.Force);       
    }
}
