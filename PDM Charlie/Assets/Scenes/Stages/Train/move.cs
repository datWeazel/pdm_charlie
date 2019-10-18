using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{

    public float speed = -10f;
    public float resetDistance = 9000f;

    private Vector3 startPos;
    //private Rigidbody rigidbody;

    private void Start()
    {
        //this.rigidbody = GetComponent<Rigidbody>();
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        //rigidbody.MovePosition(new Vector3(speed * Time.deltaTime, 0, 0));
        
        if(Vector3.Distance(startPos, transform.position) > resetDistance)
        {
            transform.position = startPos;
        }
    }
}
