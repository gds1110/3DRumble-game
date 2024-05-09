using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class ArangeBullet : Projectile
{
     [SerializeField]
    protected float _speed = 30f;
     [SerializeField]
    protected float _rotateSpeed =20f;

    [SerializeField]
    bool _isFollow = true;
    private void Start()
    {
       
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        SphereCollider sp = GetComponent<SphereCollider>();
        sp.isTrigger = true;
    }

    public override void SetProjectileInfo(Transform target, int damage, UnitController byUnit, Attack byAttack, bool isFollow = true)
    {
        _target = target;
        _damage = damage;
        _owner = byUnit;
        _type = byUnit._unit._unitElementalType;
        _isFollow = isFollow;
        _isShoot = true;
        _hitEffect = byAttack._hitEffect;
        transform.LookAt(target);
    }
    public void FixedUpdate()
    {
        if (_isFollow == false)
            return;
        if (_target == null)
            return;
        if (_isShoot == false)
            return;
        //Vector3 dir = (_target.position - transform.position).normalized;
        //Vector3 rotationAmount = Vector3.Cross(transform.forward, dir);
        //_rb.angularVelocity = rotationAmount * _rotateSpeed;
        //_rb.velocity = transform.forward * _speed;

        transform.position = Vector3.Lerp(transform.position, _target.position, _speed*Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform == _target)
        {
            if (_hitEffect)
            {
                Poolable poolable = Managers.Pool.PopAutoPush(_hitEffect, _owner.transform);
                poolable.transform.position = _target.position+_target.transform.up*1;
            }
            UnitController unit = _target.GetComponent<UnitController>();
            unit.TakeDamage(_damage, _owner);
            if (_isSplash == true)
            {
                SplashDamage(other);
            }
            Poolable thisPool = GetComponent<Poolable>();
            if (thisPool != null)
            {
                Managers.Pool.Push(thisPool);
            }
            else
            {
                Destroy(gameObject);
            }
           
        }
    }

    public override void Clear()
    {
        base.Clear();
    }
}
