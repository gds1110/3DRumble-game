using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Unit Data",menuName ="Scriptable Object/UnitData",order =int.MaxValue)]
[Serializable]
public class UnitData : ScriptableObject
{
    [Header("Common")]
    public string _name;
    public string _displayName;
    public GameObject FriendlyUnit; // 快龋利牢
    public GameObject HostileUnit; // 利措利牢
    public Sprite UnitPortrait;
    public int cost;
    [Header("Unit")]
    public Define.AttackType _attackType = Define.AttackType.Meele;
    public Define.MovemnetType _movementType = Define.MovemnetType.Ground;
    public Define.MovemnetType _targetMovementType = Define.MovemnetType.Ground;
    public Define.TargetType _targetType = Define.TargetType.Both;
    public Define.TargetType _UnitType = Define.TargetType.Unit;
    public Define.UnitElementalType _elementalType = Define.UnitElementalType.Normal;
    [Header("UnitVariable")]
    public float _attackRatio=1f; //time between attack
    public float _attackRange = 1f;
    public float _speed = 5f;
    public float _scanRange = 10;
    public int _damage = 2;
    public int _life = 10;
    public int _defense = 10;
    public bool _isSplitAttackAnim = false;
    public float _attackTiming = 0.2f;


}
