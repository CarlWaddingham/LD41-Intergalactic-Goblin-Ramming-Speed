using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public ParticleSystem projectile, impact;
    public TrailRenderer trail;
    private Rigidbody m_Rigidbody;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        trail = GetComponent <TrailRenderer>();
    }

    public void Fire(float bulletSpeed, float bulletRange, bool player)
    {
        m_Rigidbody.velocity = Vector3.zero;
        m_Rigidbody.AddForce(transform.forward * bulletSpeed, ForceMode.VelocityChange);
        trail.Clear();
        gameObject.layer = LayerMask.NameToLayer("Player Projectile");
        Invoke("Collide", bulletRange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collide();
    }

    void Collide()
    {
        if (projectile)
        {
            projectile.Stop(true);
            impact.gameObject.SetActive(true);
            Invoke("Restart", 2f);
        }
        gameObject.SetActive(false);
    }

    private void Restart()
    {
        CancelInvoke();
        impact.Clear(true);
        impact.gameObject.SetActive(false);
        projectile.Play(true);
        gameObject.SetActive(false);
    }
}
