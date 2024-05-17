using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tower : BaseCombat, IDamageAble
{
    public UnitData unitData;

    public Define.WorldObject _worldObject;

    public TowerController _controller;

    public void SetData()
    {
        _name = unitData._name;
        _attackType = unitData._attackType;
        _targetType = unitData._targetType;
        _attackDelay = unitData._attackRatio;
        _attackRange = unitData._attackRange;
        _damage = unitData._damage;
        _speed = unitData._speed;
        _life = unitData._life;
        _defense = unitData._defense;
        _scanRange = unitData._scanRange;
        _unitElementalType = unitData._elementalType;
        _movementType = unitData._movementType;
        _targetMovementType = unitData._targetMovementType;
        _unityType = unitData._UnitType;
        _isSplitAttackAnim = unitData._isSplitAttackAnim;
        _attackTiming = unitData._attackTiming;
        _controller = GetComponent<TowerController>();
       // _isUnbreakable = unitData._isUnbreakable;
        
    }

    private void Start()
    {
        if (GetComponent<Collider>())
        {
            hpBar = Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform, "UI_HPBar");
            hpBar.Init();
        }
    }



    public void TakeDamage(int damage, Controller fromUnit)
    {
        if (damage < 0)
        {
            damage = 1;
        }
 
        float eleDamage = ElementalCalculate.GetMultiplier(fromUnit._unit._unitElementalType, _unitElementalType);
        Debug.Log("HitDamage");
        int trueDamage = Mathf.RoundToInt(damage * eleDamage);
        if (trueDamage < 0)
        {
            trueDamage = 1;
        }

        _currentLife -= trueDamage;
        if(hpBar)
        {
            float ratio = _currentLife / _life;
            hpBar.SetHpRatio(ratio);
        }
        if (_currentLife <= 0)
        {

            if (_currentLife <= 0)
            {
                DeadAction?.Invoke();
                Invoke("OnDeath", 1.0f);
            }
        }

    }

    private void Awake()
    {
        SetData();

    }
   
}
