using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour {

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player Ship"))
        {
            Destroy(collision.gameObject);
            GameManager.instance.Win();
        }
    }
}
