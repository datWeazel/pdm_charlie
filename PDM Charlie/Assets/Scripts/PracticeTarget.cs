using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class PracticeTarget : MonoBehaviour
{
    public float shakeDuration = 1.0f;
    public float shakeStrength = 2.5f;

    public float moveDuration = 1.0f;

    public int hitCount = 0;

    private List<Vector3> possiblePositions = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] targetSpawns = GameObject.FindGameObjectsWithTag("TargetSpawns");
        foreach(GameObject spawn in targetSpawns)
        {
            possiblePositions.Add(spawn.transform.position);
        }

        this.transform.position = possiblePositions.ElementAt(Random.Range(0, possiblePositions.Count - 1));
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.DOShakeRotation(shakeDuration, shakeStrength);
    }

    public void MoveTo(Vector3 newPosition)
    {
        this.transform.DOMove(newPosition, moveDuration);
    }

    public void OnHit()
    {
        hitCount++;

        Vector3 newPosition = this.transform.position;
        while (newPosition == this.transform.position)
        {
            newPosition = possiblePositions.ElementAt(Random.Range(0, possiblePositions.Count - 1));
        }

        MoveTo(newPosition);
    }
}
