using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Transform _target;
    int _damage;
    Unit _owner;
    Define.UnitElementalType _type;
    Rigidbody _rb;
    bool _isMeele=true;
    bool _isFollow = false;
    [SerializeField]
    float _speed=20f; 
    [SerializeField]
    float _rotateSpeed=5f;

    public bool _isShoot=false;

    public GameObject _hitEffect;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

   public void SetProjectileInfo(Transform target,int damage,Unit byUnit,bool isFollow=true)
    {
        _target= target;
        _damage= damage;
        _owner = byUnit;
        _type = byUnit._unitElementalType;
        _isFollow= isFollow;
        _isMeele = false;
        _isShoot = true;
    }
    public void FixedUpdate()
    {
        if (_isFollow == false)
            return;
        if(_target == null) 
            return;
        if (_isShoot == false)
            return;
        Vector3 dir = (_target.position - transform.position).normalized;
        Vector3 rotationAmount = Vector3.Cross(transform.forward, dir);
        _rb.angularVelocity = rotationAmount * _rotateSpeed;   
        _rb.velocity = transform.forward * _speed;   
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform == _target)
        {
            //Vector3 norm = Vector3.Normalize(gameObject.transform.position - other.transform.position);
            //Managers.Effect.PlayEffect(_hitEffect, other.transform.position, norm);

            Poolable poolable =  Managers.Pool.PopAutoPush(_hitEffect,_owner.transform);
            poolable.transform.position = _target.position;

            UnitController unit = _target.GetComponent<UnitController>();
            unit.TakeDamage(_damage, _owner._unitElementalType);

            if (_isMeele == false)
            {
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
    }

}
