using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class MeeleBullet : Projectile
{

    public BoxCollider _bulletColider;


    private void Awake()
    {
        _bulletColider = GetComponent<BoxCollider>();
        _bulletColider.enabled = false;
        _bulletColider.size = new Vector3(5, 8, 5);
        _bulletColider.center = new Vector3(0, 1, 1.5f);

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    public override void SetProjectileInfo(Transform target, int damage, Controller byUnit, Attack byAttack, bool isFollow = true)
    {
        _target = target;
        _damage = damage;
        _owner = byUnit;
        _type = byUnit._unit._unitElementalType;
        _isShoot = true;
        _hitEffect = byAttack._hitEffect;
        _bulletColider.enabled = true;

        //StartCoroutine(MeeleOnOf());
    }
    IEnumerator MeeleOnOf()
    {
        _bulletColider.enabled = true;
        yield return new WaitForSeconds(3f);
        _bulletColider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other==_owner.GetComponent<Collider>())
        {
            return;
        }

        if (other.gameObject.transform == _target)
        {
            if (_hitEffect)
            {
                Poolable poolable = Managers.Pool.PopAutoPush(_hitEffect, _owner.gameObject.transform);
                poolable.transform.position = transform.position + (transform.forward * 1.5f)+(transform.up*1.5f);
            }

            IDamageAble damageAble = _target.GetComponent<IDamageAble>();
            if (damageAble!=null)
                damageAble.TakeDamage(_damage, _owner);
            if (_isSplash == true)
            {
                SplashDamage(other);
            }
            _bulletColider.enabled = false;

        }
      
    }
   

    public override void Clear()
    {
        base.Clear();
    }
}
