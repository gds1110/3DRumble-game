using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.GridLayoutGroup;


[AddComponentMenu("My Unit/Unit Controller")]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Attack))]

public class UnitController : MonoBehaviour
{

    private readonly int _attackHash=Animator.StringToHash("ATTACK");
    private readonly int _moveHash = Animator.StringToHash("WALK");
    private readonly int _deadHash = Animator.StringToHash("DEAD");
    private readonly int _idleHash = Animator.StringToHash("IDLE");

    [SerializeField] private Define.State _state= Define.State.Idle;
    Animator _anim;

    public Unit _unit;
    [SerializeField]
    GameObject _target;

    [SerializeField] float _scanRange = 10;
    public Transform _barrel;

    public bool _isPlaced = false;
    public bool _isDelay = false;
    [SerializeField]
    public Transform _destPos;
    public Define.State State { 
        get { return _state; }
        set { _state = value; 
            switch (_state)
            {
                case Define.State.Die:
                    _anim.CrossFade(_deadHash, 0.1f);
                    break;
                case Define.State.Moving:
                    _anim.CrossFade(_moveHash, 0.1f);
                    break;
                case Define.State.Idle:
                    _anim.CrossFade(_idleHash, 0.1f);
                    break;
                case Define.State.Attack:
                    _anim.CrossFade(_attackHash, 0.1f,-1,0);
                    break;
            }

        }

    }

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _unit = GetComponent<Unit>();


       // Managers.UI.MakeWorldSpaceUI<UI_HpBar>(transform);
    }
    private void Update()
    {
        if (_isPlaced == false)
        {
            return;
        }
        switch (_state)
        {
            case Define.State.Die:
                UpdateDie();
                break;
            case Define.State.Moving:
                UpdateMoving();
                break;
            case Define.State.Idle:
                UpdateIdle();
                break;
            case Define.State.Attack:
                UpdateAttack();
                break;
        }
    }

    void UpdateDie()
    {

    }
    void UpdateMoving()
    {
        FindTarget();
        if(_target!=null)
        {
            float distance = new Vector2(_target.transform.position.x - transform.position.x, _target.transform.position.z - transform.position.z).magnitude;
            if(distance<_unit._attackRange)
            {
                NavMeshAgent nav = gameObject.GetOrAddComponent<NavMeshAgent>();
                nav.SetDestination(transform.position);
                State = Define.State.Attack;
                return;
            }
            else
            {
                Debug.Log("사정거리 밖");
                NavMeshAgent nav = gameObject.GetOrAddComponent<NavMeshAgent>();
                nav.SetDestination(_target.transform.position);
               
            }
        }
        else
        {
            NavMeshAgent nav = gameObject.GetOrAddComponent<NavMeshAgent>();
            nav.SetDestination(_destPos.position);
            nav.speed = _unit._speed;
        }
    }
    void UpdateIdle()
    {
        if(_destPos!=null)
        {
            State = Define.State.Moving;
        }
    }
    void UpdateAttack()
    {
        if (_target != null)
        {

            float distance = new Vector2(_target.transform.position.x - transform.position.x, _target.transform.position.z - transform.position.z).magnitude;
            if (distance < _unit._attackRange)
            {
                transform.LookAt(_target.transform);
               // StartCoroutine(CoAttack());
               if(_isDelay==false)
                {
                    _anim.Play(_attackHash);
                }
            }
            else
            {
                State = Define.State.Moving;
            }
        }
      
    }

    private void OnDrawGizmos()
    {

        if (_unit != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, _unit._scanRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(this.transform.position, _unit._attackRange);
        }

    }

    public void AttackToTarget()
    {
        if(_target==null)
        {
            return;
        }
        switch (_unit._attackType)
        {
            case Define.AttackType.Meele:
                UnitController unitController = _target.GetComponent<UnitController>();
                if (unitController != null)
                {
                    //Poolable poolable = Managers.Pool.PopAutoPush(_hitEffect, transform);
                    //poolable.transform.position = _target.transform.position;
                    unitController.TakeDamage(_unit._damage, _unit._unitElementalType);
                }
                break;
            case Define.AttackType.Arange:
                //Projectile projectile = Instantiate(_unit._bullet,_barrel);
                Projectile projectile = Managers.Pool.Pop(_unit._bullet.gameObject,_barrel).GetComponent<Projectile>();

                if (projectile)
                {
                    projectile.transform.position = _barrel.transform.position;
                    projectile.SetProjectileInfo(_target.transform, _unit._damage, _unit);
                }
                break;
        }

        StartCoroutine(AttackDelay(_unit._attackDelay));
    }
    IEnumerator AttackDelay(float delay)
    {
        _isDelay = true;
        yield return new WaitForSeconds(delay);
        _isDelay = false;
    }
    IEnumerator CoAttack()
    {

        switch (_unit._attackType)
        {
            case Define.AttackType.Meele:
                yield return new WaitForSeconds(0.5f);
                break;
            case Define.AttackType.Arange:
                yield return new WaitForSeconds(0.2f);
                Projectile projectile = Instantiate(_unit._bullet, this.transform, true);
                projectile.SetProjectileInfo(_target.transform, _unit._damage, _unit);
                break;
        }
    }

    void FindTarget()
    {
        if(_target!=null)
        {
            return;
        }
        Collider[] cols = Physics.OverlapSphere(transform.position, _unit._scanRange, 1 << 3);
        if(cols.Length > 0)
        {
            for(int i=0;i<cols.Length; i++)
            {
                if (cols[i].tag!=gameObject.tag)
                {
                    Unit tempTarget = cols[i].gameObject.GetComponent<Unit>();
                    if (tempTarget != null && CheckCanAttackType(tempTarget))
                        _target = tempTarget.gameObject;
                        
                    break;

                }
            }
        }
    }

    bool CheckCanAttackType(Unit target)
    {

        if(_unit._targetMovementType!=target._movementType&&_unit._targetMovementType != Define.MovemnetType.Both) return false;
        if(_unit._targetType!=target._unityType&&_unit._targetType!=Define.TargetType.Both) return false;
        return true;
    }

    public void TakeDamage(int damage,Define.UnitElementalType type)
    {
        Debug.Log("HitDamage");
        int trueDamage = Mathf.RoundToInt(damage);
    }
}
