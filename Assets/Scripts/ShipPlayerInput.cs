using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPlayerInput : InputController {


	// Update is called once per frame
	void Update () {
        if(m_Controller.state != Ship.ShipState.Operational) return;

        m_Controller.Direction(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized);
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 2000f))
        {/*
            if (Input.GetButton("Fire1") && m_Weapon.rateOfFireTimer >= (1 / m_Weapon.fireRate))
            {

                Debug.Log("Can Shoot");
                //m_Weapon.Fire(hit.point - transform.position);
            }
            */

            if (Input.GetButtonDown("Jump"))
            {
                m_Controller.DodgeRoll((hit.point - transform.position).normalized);
            }
        }
    }
}
