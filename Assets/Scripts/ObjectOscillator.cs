using UnityEngine;
using System.Collections;

public class ObjectOscillator : MonoBehaviour
{

    public float speedMult = 1.0f;
    public float rangeMult = 1.0f;
    // Use this for initialization
    public GameObject bullet;
    public float shootInterval = 1.0f;
    float basex = 0.0f;
    float shootTimeAc = 0.0f;
    public float speed = 4000;
    // Update is called once per frame
    void Start()
    {
        basex = transform.position.x;
    }
    void Update()
    {
        Vector3 position = transform.position;
        float interval = Mathf.Sin(Time.time * (speedMult / rangeMult)) * rangeMult;
        bool shoot = false;
        if (Time.deltaTime + shootTimeAc > shootInterval)
        {
            shootTimeAc = 0.0f;
            shoot = true;
        }

        else
            shootTimeAc += Time.deltaTime;


        transform.Rotate((Vector3.up * speedMult) * Time.deltaTime);
        if (shoot)
        {
            Rigidbody bulletRB = Instantiate(bullet, transform.position, transform.rotation).GetComponent<Rigidbody>();

            bulletRB.AddRelativeForce(Vector3.forward * speed, ForceMode.Acceleration);

        }
    }
}