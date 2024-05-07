using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{

    public GameObject _bullet;
    public GameObject _hitEffect;
    public int _damage = 1;
    //public Transform _Barrel;
    public UnitController _owner;
    public void Awake()
    {
        _owner = GetComponent<UnitController>();
    }


    public virtual void DoAttack(GameObject target) { }


}

