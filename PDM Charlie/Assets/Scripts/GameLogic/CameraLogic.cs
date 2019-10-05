using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class CameraLogic : MonoBehaviour {

	//private Transform transform;
	private Vector3 desiredPos;
	public List<Transform> players;
	public float camSpeed = 2.5f;
	private Camera cam;
    [SerializeField] public float distanceH_param_a = 0.25f;
    [SerializeField] public float distanceH_param_b = 5f;
    [SerializeField] public float distanceW_param_a = 0.25f;
    [SerializeField] public float distanceW_param_b = 5f;

    void Awake()
	{
		//transform = GetComponent<Transform>();
		cam = GetComponent<Camera>();            
	}

	private void Start()
    {
        foreach(GameObject character in GameObject.FindGameObjectsWithTag("Character"))
        {
            AddPlayerToCam(character.transform);
        }
    }

	void Update()
	{
		if (players.Count <= 1)//early out if no players have been found
			return;
		desiredPos = Vector3.zero;
		float distance = 0f;
		var hSort = players.OrderByDescending(p => p.position.y);
		var wSort = players.OrderByDescending(p => p.position.x);
		var mHeight = hSort.First().position.y - hSort.Last().position.y;
		var mWidth = wSort.First().position.x - wSort.Last().position.x;
		var distanceH = -(mHeight + distanceH_param_b) * distanceH_param_a / Mathf.Tan(cam.fieldOfView * distanceH_param_a * Mathf.Deg2Rad);
		var distanceW = -(mWidth / cam.aspect + distanceW_param_b) * distanceW_param_a / Mathf.Tan(cam.fieldOfView * distanceW_param_a * Mathf.Deg2Rad);
		distance = distanceH < distanceW ? distanceH : distanceW;

		for (int i = 0; i < players.Count; i++)
		{
			desiredPos += players[i].position;
		}
		if (distance > -10f) distance = -10f;
		desiredPos /= players.Count;
		desiredPos.z = distance;
		desiredPos.y += 4.5f;
	}

	void LateUpdate()
	{
		if (players.Count <= 1) {
			return;
		}
		transform.position = Vector3.MoveTowards(transform.position, desiredPos, camSpeed);
	}

	public void AddPlayerToCam(Transform p){
		players.Add (p);
	}

	public void RemovePlayerFromCam(Transform p){
		players.Remove (p);
	}
}
