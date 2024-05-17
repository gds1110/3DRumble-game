using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseCombat : MonoBehaviour
{
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
    public bool _isUnbreakable;
    public UI_HPBar hpBar;

    public UnityEvent DeadAction;

    public virtual void OnDeath()
    {
        Managers.Game.Despawn(this.gameObject);
    }
}
