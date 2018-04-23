using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {

    public enum ShipState
    {
        Null,
        Operational,
        Action,
        Damaged,
        Destroyed
    }

    public float speed = 5f;
    public float rotationSpeed = 5f;
    public float gravity;
    public float dodgeRollStrength = 200f;
    public ShipState state;
    public float recoveryRate = 1f;
    public GameObject ship;

    private Rigidbody m_Rigidbody;
    private Vector3 direction;

    public void Direction(Vector3 value)
    {
        direction = value;
    }

    public void ChangeState(ShipState newState)
    {
        state = newState;

        switch (state)
        {
            case ShipState.Operational:
                m_Rigidbody.drag = 5;
                m_Rigidbody.angularDrag = 10;
                break;
            case ShipState.Damaged:
                m_Rigidbody.drag = 1;
                m_Rigidbody.angularDrag = 1;
                m_Rigidbody.ResetCenterOfMass();
                Invoke("Recover", recoveryRate);
                break;
            case ShipState.Destroyed:
                m_Rigidbody.drag = 0;
                m_Rigidbody.angularDrag = 0;
                m_Rigidbody.ResetCenterOfMass();
                break;
            case ShipState.Action:
                m_Rigidbody.drag = 2f;
                m_Rigidbody.angularDrag = 0f;
                m_Rigidbody.ResetCenterOfMass();
                Invoke("Recover", 1f);
                break;
        }
    }

    public void DodgeRoll(Vector3 dir)
    {
        ChangeState(ShipState.Action);
        m_Rigidbody.velocity = Vector3.zero;
        dir.y = 0f;
        m_Rigidbody.AddForce(dir.normalized * dodgeRollStrength + Vector3.up * dodgeRollStrength * 0.35f, ForceMode.VelocityChange);
    }

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        ship.SetActive(false);
        Invoke("Init", 1f);
    }
    void Init()
    {
        ChangeState(ShipState.Action);
        m_Rigidbody.isKinematic = false;
        ship.SetActive(true);
        
        m_Rigidbody.AddForce(transform.forward * 40f + Vector3.up * 40f, ForceMode.VelocityChange);
    }

    void Recover()
    {
        ChangeState(ShipState.Operational);
        m_Rigidbody.centerOfMass = Vector3.down * 4f;
    }

    public AudioClip hit;
    private void OnCollisionEnter(Collision collision)
    {
        if (state == ShipState.Operational)
        {
            if (collision.transform.tag == "Environment")
            {
                SoundManager.instance.PlaySound(hit, transform.position, true, gameObject);
                if (collision.relativeVelocity.magnitude > 30)
                {
                    m_Rigidbody.AddForce(((transform.position - collision.transform.position).normalized + (Vector3.up* 0.5f)) * 25f , ForceMode.VelocityChange);
                    ChangeState(ShipState.Damaged);
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (state == ShipState.Operational)
        {
            if (direction != Vector3.zero)
            {

                Quaternion rotation = Quaternion.LookRotation(direction);
                rotation *= Quaternion.Euler(0, 0, Mathf.Clamp(Vector3.SignedAngle(direction, transform.forward, transform.up) * 0.5f, -90, 90));

                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
                transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

                m_Rigidbody.AddForce(direction * speed, ForceMode.VelocityChange);
            }
        }
        else
        {
            m_Rigidbody.AddTorque(Vector3.Cross(-m_Rigidbody.velocity, Vector3.up) * 5000f, ForceMode.VelocityChange);
        }


        m_Rigidbody.AddForce(Vector3.down * gravity, ForceMode.VelocityChange);
    }
}
