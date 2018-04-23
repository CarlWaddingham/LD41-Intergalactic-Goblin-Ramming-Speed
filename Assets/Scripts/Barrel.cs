using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{

    public GameObject brokenPrefab;
    public AudioClip clip;
    public int scoreValue;
    bool called = false;
    public void OnCollisionEnter(Collision collision)
    {
        if (called) return;
        if (collision.transform.tag == "Player" || collision.transform.tag == "Enemy" || collision.transform.tag == "Object" || collision.transform.tag == "Environment")
        {
            if (collision.relativeVelocity.magnitude > 10)
            {
                called = true;
                CameraShake.instance.Shake(.05f);
                GameManager.instance.AddScore(scoreValue);
                Debug.Log(SoundManager.instance.PlaySound(clip, transform.position, true));
                GameObject go = Instantiate(brokenPrefab, transform.position, transform.rotation);
                Debug.Log("No Message");
                go.transform.localScale = transform.localScale;
                Destroy(gameObject);
            }
        }
    }
}
