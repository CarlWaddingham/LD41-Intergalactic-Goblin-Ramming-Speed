using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour {

    public GameObject target;
    public float speed = 5f;
    public float rotationSpeed = 5f;
    public float gravity;
    public Collider feetCollider;
    public List<GameObject> equipment;
    public bool isSitting;

    public delegate void Death();
    public event Death OnDeath;
    public enum GoblinState
    {
        Null,
        Idle,
        Chase,
        RunAway,
        Dead,
    }
    public GoblinState state;
    public GoblinState defaultAlertState = GoblinState.Chase;
    private Animator m_Animator;
    private Rigidbody m_Rigidbody;
    private Vector3 direction;
    public int scoreValue;

    public void ChangeState(GoblinState newState)
    {
        state = newState;

        switch (state)
        {
            case GoblinState.Idle:
                m_Rigidbody.drag = 5;
                m_Rigidbody.angularDrag = 10;
                m_Animator.SetBool("Sitting", isSitting);
                break;
            case GoblinState.RunAway:
            case GoblinState.Chase:
                isSitting = false;
                m_Animator.SetBool("Sitting", isSitting);
                m_Rigidbody.drag = 5;
                m_Rigidbody.angularDrag = 10;
                break;
            case GoblinState.Dead:
                isSitting = false;
                m_Animator.SetBool("Sitting", isSitting);
                foreach (Goblin sub in subscribedGOs)
                {
                    sub.OnDeath -= FriendDied;
                }
                if(OnDeath != null)
                    OnDeath();

                GameManager.instance.AddScore(scoreValue);
                feetCollider.enabled =false;
                m_Rigidbody.drag = 0;
                m_Rigidbody.angularDrag = 0.5f;
                break;
        }
    }


    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        foreach (GameObject equip in equipment)
        {
            int i = Random.Range(0, 2);
            if(i == 1)
            {
                equip.SetActive(false);
            }
        }
        ChangeState(GoblinState.Idle);
    }

    public void Direction(Vector3 value)
    {
        direction = value;
    }

    private void Update()
    {
        if (target)
        {
            direction = -(transform.position - target.transform.position).normalized;
        }

        if (state == GoblinState.Chase || state == GoblinState.RunAway)
        {
            m_Animator.SetFloat("Speed", m_Rigidbody.velocity.magnitude);
        }
        else {
            m_Animator.SetFloat("Speed", 0);
        }
    }

    void FixedUpdate()
    {
        if (!target) return;
        if (state != GoblinState.Dead)
        {
            if (direction != Vector3.zero)
            {
                direction.y = 0;
                if (state == GoblinState.RunAway)
                    direction = -direction;
                Quaternion rotation = Quaternion.LookRotation(direction);
                rotation *= Quaternion.Euler(0, 0, Mathf.Clamp(Vector3.SignedAngle(direction, transform.forward, transform.up) * 0.5f, -90, 90));

                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
                transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

                if(state == GoblinState.Chase || state == GoblinState.RunAway)
                    m_Rigidbody.AddForce(direction.normalized * speed, ForceMode.VelocityChange);
            }
        }


        m_Rigidbody.AddForce(Vector3.down * gravity, ForceMode.VelocityChange);
    }
    public float Knockback = 20f;
    public AudioClip hit;
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 16)
        {
            SoundManager.instance.PlaySound(hit, transform.position, true, gameObject);
            if (collision.transform.tag == "Player" || collision.transform.tag == "Enemy" || collision.transform.tag == "Object")
            {
                m_Rigidbody.velocity = Vector3.zero;
                m_Rigidbody.AddForce((transform.position - collision.transform.position).normalized + Vector3.up * Knockback,ForceMode.VelocityChange);

                if (state != GoblinState.Dead)
                {
                    CameraShake.instance.Shake(.05f);
                    ChangeState(GoblinState.Dead);
                    foreach (GameObject equip in equipment)
                    {
                        Rigidbody temp = equip.AddComponent<Rigidbody>();
                        if (temp)
                        {
                            temp.mass = 0.01f;
                            equip.transform.SetParent(null);
                        }
                    }
                }
            }
        }
    }

    List<Goblin> subscribedGOs = new List<Goblin>();

    public void OnTriggerEnter(Collider other)
    {
        if (state == GoblinState.Dead) return;
        if (other.gameObject.layer == LayerMask.NameToLayer("Player Ship"))
        {
            target = other.gameObject;
            if(state == GoblinState.Idle)
            {
                ChangeState(defaultAlertState);
            }
        } else if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (other.transform.name.Contains("Goblin"))
            {
                Goblin goblin = other.transform.GetComponent<Goblin>();

                if (!subscribedGOs.Contains(goblin))
                {
                    goblin.OnDeath += FriendDied;
                    subscribedGOs.Add(goblin);
                }
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (state != GoblinState.Idle || state != GoblinState.Chase) return;
        if (other.gameObject.layer == LayerMask.NameToLayer("Player Ship"))
        {
            target = null;
        }
    }

    void FriendDied()
    {
        ChangeState(GoblinState.Chase);
    }

}
