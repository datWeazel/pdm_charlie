using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableInvisibleTerrain : MonoBehaviour
{

    private Terrain[] terrains;
    private Collider col;

    // Start is called before the first frame update
    void Start()
    {
        terrains = GetComponentsInChildren<Terrain>();
        col = GetComponent<Collider>();
        foreach(Terrain ter in terrains)
        {
            ter.enabled = false;
        }
        //Debug.Log(terrains);
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other is TerrainCollider)
        {
            //Debug.Log("Is TerrainCol true");
            other.GetComponentInParent<Terrain>().enabled = true;
        } else
        {
            //Debug.Log("Is TerrainCol false");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (Terrain ter in terrains)
        {
            if (GameObject.ReferenceEquals(ter.transform.gameObject, other.transform.gameObject))
            {
                ter.enabled = false;
            }

        }
    }
}
