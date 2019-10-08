using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitboxController : MonoBehaviour
{
    public float startRadius = 4.0f;
    public float endRadius = 6.0f;
    public float expansionSpeed = 1.0f;
    public float strength = 150.0f;

    public GameObject parent;
    public GameObject character;

    public bool isExpanding = false;
    private List<PlayerController> hitPlayers = new List<PlayerController>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isExpanding)
        {
            GetComponent<SphereCollider>().radius += expansionSpeed * Time.deltaTime;
            if(GetComponent<SphereCollider>().radius >= endRadius)
            {
                GetComponent<SphereCollider>().radius = startRadius;
                hitPlayers.Clear();
                isExpanding = false;
                parent.SetActive(false);
            }
        }
    }

    public void StartHitbox()
    {
        isExpanding = true;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.transform.tag == "Character" && collision.gameObject != this.character)
        {
            PlayerController player = collision.transform.GetComponentInParent<PlayerController>();
            if (!hitPlayers.Contains(player))
            {
                Debug.Log($"Hit!");
                hitPlayers.Add(player);

                Vector3 direction = collision.transform.position - this.character.transform.position;
                Debug.Log($"Hit! (direction: {direction.ToString()}");
                player.CharacterController.AddForce((direction*strength));
                player.CharacterController.SetHitStun(0.35f);
            }
        }
    }
}
