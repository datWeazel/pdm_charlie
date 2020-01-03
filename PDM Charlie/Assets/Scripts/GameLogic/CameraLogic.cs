using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class CameraLogic : MonoBehaviour {

	//private Transform transform;
	private Vector3 desiredPos;
	public List<Transform> players;
	public float camSpeed = 2.5f;
    public float distanceConst = 4.5f;
    public float distanceConstZ = 0.0f;

    private Camera cam;
    private float distanceH_param_a = 0.25f;
    private float distanceH_param_b = 5f;
    private float distanceW_param_a = 0.25f;
    private float distanceW_param_b = 5f;

    void Awake()
	{
		this.cam = GetComponent<Camera>();            
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
		float distanceZ = distanceConstZ;

		if (players.Count < 1) return;
		if(this.players.Count == 1)
		{
			distanceZ -= 4;
		}

		var hSort = this.players.OrderByDescending(p => p.position.y);
		var wSort = this.players.OrderByDescending(p => p.position.x);
		var mHeight = hSort.First().position.y - hSort.Last().position.y;
		var mWidth = wSort.First().position.x - wSort.Last().position.x;
		var distanceH = -(mHeight + distanceH_param_b) * distanceH_param_a / Mathf.Tan(this.cam.fieldOfView * distanceH_param_a * Mathf.Deg2Rad);
		var distanceW = -(mWidth / this.cam.aspect + distanceW_param_b) * distanceW_param_a / Mathf.Tan(this.cam.fieldOfView * distanceW_param_a * Mathf.Deg2Rad);
        float distance = (distanceH < distanceW) ? distanceH : distanceW;

        this.desiredPos = Vector3.zero;
        foreach (Transform player in this.players)
        {
            desiredPos += player.position;
        }

		if (distance > -10f) distance = -10f;
        this.desiredPos /= this.players.Count;
        this.desiredPos.z = distance + distanceZ;
        this.desiredPos.y += this.distanceConst;
	}

	void LateUpdate()
	{
		if (this.players.Count < 1) {
			return;
		}
        this.transform.position = Vector3.MoveTowards(transform.position, this.desiredPos, this.camSpeed);
	}

	public void AddPlayerToCam(Transform p){
        this.players.Add (p);
	}

	public void RemovePlayerFromCam(Transform p){
        this.players.Remove (p);
	}
}
