using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{

    public float speed = -10f;
    //private Rigidbody rigidbody;

    private void Start()
    {
        //this.rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        //rigidbody.MovePosition(new Vector3(speed * Time.deltaTime, 0, 0));
        
    }
}
