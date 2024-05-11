using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public UnitData unitData;
    public Define.AttackType _attackType;
    public Define.TargetType _targetType;
    public Define.TargetType _unityType;
    public Define.MovemnetType _movementType;
    public Define.MovemnetType _targetMovementType;
    public Define.UnitElementalType _unitElementalType;
    public string _name;
    public string DisplayName;

    public float _attackDelay; //time between attack
    public float _attackRange;
    public int _damage;
    public float _speed;
    public float _life;
    public float _defense;
    public float _scanRange;
    public bool _isSplitAttackAnim = false;
    public float _attackTiming;
    public float _currentLife;

    public Define.WorldObject _worldObject;

    public UnitController _controller;

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
        _controller = GetComponent<UnitController>();
    }
    private void Awake()
    {
        SetData();
        
    }



}
