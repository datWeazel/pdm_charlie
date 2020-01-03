using DG.Tweening;
using UnityEngine;


public class DeathZonesController : MonoBehaviour
{
    const float cameraShakeDuration = 0.5f;
    const float cameraShakeStrength = 2.0f;

    AudioSource audioSource = null;

    private void OnStart()
    {
        audioSource = this.transform.GetComponent<AudioSource>();
    }

    public void OnTriggerEnter(Collider entity)
    {
        if (entity.tag == "Character")
        {
            entity.GetComponentInParent<PlayerController>()?.HitDeathZone();
            Camera.main.transform.DOShakePosition(cameraShakeDuration, cameraShakeStrength);

            // Try again to find death zone audio source
            if(audioSource == null) audioSource = this.transform.GetComponent<AudioSource>();

            if (audioSource != null)
            {
                audioSource.Play();
                Debug.Log("Playing death zone sound.");
            }
        }
    }
}
