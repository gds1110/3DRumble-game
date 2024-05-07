using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ArangeAttack : Attack
{
    public Transform _Barrel;
    public void Start()
    {
        string projectileName = _owner._unit._name + "PR";
        string hitEffectName = _owner._unit._name + "HE";
        _bullet = Managers.Resource.Load<GameObject>("Prefabs/Projectile/"+projectileName);
        _hitEffect = Managers.Resource.Load<GameObject>("Prefabs/HitEffect/" + hitEffectName);
        if(_Barrel == null)
        {
            _Barrel = new GameObject("_Barrel").transform;
            _Barrel.position = transform.position + gameObject.transform.forward * 1.5f + (transform.up * 1.5f); ;
            _Barrel.SetParent(transform);
        }
    }
    public override void DoAttack(GameObject target)
    {
        ArangeBullet projectile = Managers.Pool.Pop(_bullet.gameObject, gameObject.transform).GetComponent<ArangeBullet>();

        if (projectile)
        {
            projectile.transform.position = _Barrel.position;
            projectile.SetProjectileInfo(target.transform,_damage, _owner,this);
        }

    }
}
