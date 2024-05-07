using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Unit Data",menuName ="Scriptable Object/UnitData",order =int.MaxValue)]
public class UnitData : ScriptableObject
{
    [Header("Common")]
    public string Name;
    public GameObject FriendlyUnit; // 우호적인
    public GameObject HostileUnit; // 적대적인
    public Sprite UnitPortrait;
    public int cost;
    [Header("Unit")]
    public Define.AttackType _attackType = Define.AttackType.Meele;
    public Define.MovemnetType _movementType = Define.MovemnetType.Ground;
    public Define.MovemnetType _targetMovementType = Define.MovemnetType.Ground;
    public Define.TargetType _targetType = Define.TargetType.Both;
    public Define.TargetType _UnitType = Define.TargetType.Unit;
    public Define.UnitElementalType _elementalType = Define.UnitElementalType.Normal;
    public float _attackRatio=1f; //time between attack
    public float _attackRange = 1f;
    public float _speed = 5f;
    public float _scanRange = 10;
    public int _damage = 2;
    public int _life = 10;
    public int _defense = 10;
}
