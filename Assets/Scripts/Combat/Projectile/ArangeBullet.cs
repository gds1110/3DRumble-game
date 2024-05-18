using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class ArangeBullet : Projectile
{
     [SerializeField]
    protected float _speed = 5f;
     [SerializeField]
    protected float _rotateSpeed =20f;
    public float _targetThreshold = 1.0f;
    [SerializeField]
    bool _isFollow = true;
    private void Start()
    {
       
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        SphereCollider sp = GetComponent<SphereCollider>();
        sp.isTrigger = true;
         _speed = 10f;

    }

public override void SetProjectileInfo(Transform target, int damage, Controller byUnit, Attack byAttack, bool isFollow = true)
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
    public void Update()
    {
        if (!_owner) { Managers.Pool.Push(GetComponent<Poolable>()); return; }
        if (_target == null)
        {
            Managers.Pool.Push(GetComponent<Poolable>());
        }
        if (_isFollow == false)
            return;

        if (_isShoot == false)
            return;
        //Vector3 dir = (_target.position - transform.position).normalized;
        //Vector3 rotationAmount = Vector3.Cross(transform.forward, dir);
        //_rb.angularVelocity = rotationAmount * _rotateSpeed;
        //_rb.velocity = transform.forward * _speed;
     
        if (_target != null)
        {
            transform.position = Vector3.Lerp(transform.position, _target.position, _speed * Time.deltaTime);

            float distance = Vector3.Distance(transform.position, _target.position);
            if(distance<=_targetThreshold)
            {
                OnTarget();  
            }
        }


            // transform.rotation = Quaternion.LookRotation(_target.position - transform.position);
    }

    public void FixedUpdate()
    {
      
    }
    void OnTarget()
    {
        OnEffect();
        _target.GetComponent<IDamageAble>().damagedDelegate.Invoke(_damage, _owner);
        PushOrDestroy();
    }

    void OnEffect()
    {
        if (_hitEffect)
        {
            Poolable poolable = Managers.Pool.PopAutoPush(_hitEffect);
            poolable.transform.position = transform.position;
            //poolable.transform.position = _target.position + _target.transform.up * 1;
           
        }
    }
    void PushOrDestroy()
    {
        _isShoot = false;
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

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.transform == _target)
    //    {
    //        if (_hitEffect)
    //        {
    //            Poolable poolable = Managers.Pool.PopAutoPush(_hitEffect);
    //            poolable.transform.position = _target.position+_target.transform.up*1;
    //        }
    //        IDamageAble damageAble = _target.GetComponent<IDamageAble>();
    //        if (damageAble != null)
    //            damageAble.TakeDamage(_damage, _owner);
    //        if (_isSplash == true)
    //        {
    //            SplashDamage(other);
    //        }
    //        Poolable thisPool = GetComponent<Poolable>();
    //        if (thisPool != null)
    //        {
    //            Managers.Pool.Push(thisPool);
    //        }
    //        else
    //        {
    //            Destroy(gameObject);
    //        }
           
    //    }
    //}

    public override void Clear()
    {
        base.Clear();
    }
}
