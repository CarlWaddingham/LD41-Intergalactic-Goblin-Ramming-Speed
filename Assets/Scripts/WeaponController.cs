using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

    public float bulletSpeed = 7000;
    public float shotLength = 1f;
    public float fireRate = .2f;
    public float rateOfFireTimer;
    public Transform axis, fire;
    public bool isPlayer;

    private void Update()
    {
        rateOfFireTimer += Time.deltaTime;
    }

    public void Fire(Vector3 direction)
    {
        Quaternion rotation = Quaternion.LookRotation(direction);
        rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
        axis.rotation = rotation;

        var instanceBullet = ObjectPooler.SharedInstance.GetPooledObject("BasicShot");
        if (instanceBullet != null)
        {
            instanceBullet.transform.position = fire.position;
            instanceBullet.transform.rotation = fire.rotation;
            instanceBullet.gameObject.SetActive(true);
            instanceBullet.GetComponent<Bullet>().Fire(bulletSpeed, shotLength, isPlayer);

        }
        rateOfFireTimer = 0.0f;


    }

}
