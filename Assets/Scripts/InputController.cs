using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    public Ship m_Controller;
    public WeaponController m_Weapon;

    // Use this for initialization
    protected virtual void Start()
    {
        m_Controller = GetComponent<Ship>();
        m_Weapon = GetComponent<WeaponController>();
    }

}
