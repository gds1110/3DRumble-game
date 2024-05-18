using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]

public class Projectile : MonoBehaviour
{
    [SerializeField]
    protected Transform _target;
    protected int _damage;
    public Controller _owner;
    protected Define.UnitElementalType _type;
    protected Rigidbody _rb;
    public bool _isSplash;
    public int _splashNum;

    public bool _isShoot=false;

    public GameObject _hitEffect;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }


    public virtual void SetProjectileInfo(Transform target, int damage, Controller byUnit, Attack byAttack, bool isFollow = true) { }
   

   public virtual void Clear()
    {
        StopAllCoroutines();
        _target = null;
        _hitEffect = null;
    }

    public virtual void DamageToTarget(IDamageAble target,float damage,Controller fromUnit)
    {
        target.damagedDelegate.Invoke((int)damage, fromUnit); 
       
    }

    public virtual void SplashDamage(Collider other)
    {
        int splashNum = 0;

        Collider[] hitCols = Physics.OverlapSphere(gameObject.transform.position, 2.0f, 1 << 3);
        for (int i = 0; i < hitCols.Length; i++)
        {
            if (hitCols[i].gameObject == other.gameObject)
            {
                continue;
            }
            IDamageAble damageAble = hitCols[i].GetComponent<IDamageAble>();
          
            UnitController targetUnit = hitCols[i].GetComponent<UnitController>();
            if (targetUnit && _owner.CheckCanAttackType(targetUnit._unit))
            {
                if (damageAble != null)
                    damageAble.damagedDelegate.Invoke((int)(_damage * 0.5f), _owner);
            
                if (_hitEffect)
                {
                    Poolable poolable = Managers.Pool.PopAutoPush(_hitEffect, _owner.transform);
                    poolable.transform.position = hitCols[i].transform.position;
                }
                splashNum++;
                if (splashNum >= _splashNum)
                {
                    break;
                }
            }

        }

    }
}
