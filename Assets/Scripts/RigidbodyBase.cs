using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyBase : MonoBehaviour {
    public AudioClip hit;
    public Rigidbody m_Rigidbody;
    bool canDisable = false;
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (m_Rigidbody != null)
        {
            if (canDisable)
            {
                if (m_Rigidbody.velocity.magnitude < 0.5f)
                {
                    Invoke("Inactive", 1f);
                }
                else
                {
                    CancelInvoke();
                }
            }
        }
        else
        {
            m_Rigidbody = GetComponent<Rigidbody>();
        }
    }

    void Inactive()
    {
        Destroy(m_Rigidbody);
        Destroy(GetComponent<Collider>());
        Destroy(this);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 10)
        {
            canDisable = true;
            if (hit)
            {
                SoundManager.instance.PlaySound(hit, transform.position, true, gameObject);
            }
        }
    }
}
