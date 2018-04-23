using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour {

    public AudioClip ding;
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.transform.tag == "Player")
        {
            SoundManager.instance.PlaySound(ding, transform.position, true);
            Destroy(gameObject);
        }
    }
}
