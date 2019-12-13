using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horn : MonoBehaviour
{
    public float nextHorn = 3.0f;

    // Update is called once per frame
    void Update()
    {
        nextHorn -= Time.deltaTime;

        if (nextHorn < 0)
        {
            nextHorn = (Random.value * 50.0f) + 13.0f;
            GetComponentInParent<AudioSource>().Play();
        }
    }
}
