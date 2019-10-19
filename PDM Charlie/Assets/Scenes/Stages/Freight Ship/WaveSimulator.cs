using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSimulator : MonoBehaviour
{
    public float maxAngle = 2f;
    public float speed = 1.5f;
    public bool counterClockwise = true;
    //public float minClamp = 0.2f;
    //public float maxClamp = 1f;

    private float slowDownFactor;

    // Update is called once per frame
    void Update()
    {

        //Prüfe, ob maxAngle im Postiven oder negativen (360-maxAngle) erreícht wird, dann Drehrichtung ändern
        if(transform.rotation.eulerAngles.z > maxAngle && transform.rotation.eulerAngles.z < 360 - maxAngle)
        {
            counterClockwise = !counterClockwise;
        }
        
       /* if(transform.rotation.eulerAngles.z > 180)
        {
            slowDownFactor = 360 - transform.rotation.eulerAngles.z;
        } else
        {
            slowDownFactor = transform.rotation.eulerAngles.z;
        }
        slowDownFactor = slowDownFactor / maxAngle;
        slowDownFactor = 1 - slowDownFactor;
        slowDownFactor = Mathf.Clamp(slowDownFactor, minClamp, maxClamp);*/



        if (counterClockwise)
        {
            transform.Rotate(0, 0, speed * slowDownFactor * Time.deltaTime);
        }
        else
        {
            transform.Rotate(0, 0, -speed * Time.deltaTime);
        }
        
    }
}
