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
[RequireComponent(typeof(NavMeshAgent))]
//[RequireComponent(typeof(Attack))]

public class UnitController : MonoBehaviour
{

    private readonly int _attackHash=Animator.StringToHash("ATTACK");
    private readonly int _moveHash = Animator.StringToHash("WALK");
    private readonly int _deadHash = Animator.StringToHash("DEAD");
    private readonly int _idleHash = Animator.StringToHash("IDLE");
    private readonly int _attackTrigger = Animator.StringToHash("ATTACKTRIGGER");


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

    public MeeleAttack _meeleAttack;
    public ArangeAttack _arangeAttack;
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
                    //_anim.CrossFade(_attackHash, 0.1f,-1,0);
                    break;
            }

        }

    }

    private void Awake()
    {

        _anim = GetComponent<Animator>();
        _unit = GetComponent<Unit>();
    }

    private void Start()
    {
        if (_unit._attackType == Define.AttackType.Both)
        {
            _arangeAttack = Util.GetOrAddComponent<ArangeAttack>(gameObject);
            _meeleAttack = Util.GetOrAddComponent<MeeleAttack>(gameObject);
        }
        if(_unit._attackType==Define.AttackType.Meele&& _meeleAttack==null)
        {
            _meeleAttack = Util.GetOrAddComponent<MeeleAttack>(gameObject);
        }
        if (_unit._attackType==Define.AttackType.Arange&& _arangeAttack == null)
        {
            _arangeAttack = Util.GetOrAddComponent<ArangeAttack>(gameObject);
        }
        if (_unit._movementType == Define.MovemnetType.Aerial)
        {
            GetComponent<NavMeshAgent>().baseOffset = 2;
        }
        CapsuleCollider cp = GetComponent<CapsuleCollider>();
        cp.isTrigger = true;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints=RigidbodyConstraints.FreezePositionX| RigidbodyConstraints.FreezePositionZ;
        rb.constraints |= RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
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
        NavMeshAgent nav = gameObject.GetOrAddComponent<NavMeshAgent>();
        nav.isStopped = false;
        FindTarget();
        if(_target!=null)
        {
            float distance = new Vector2(_target.transform.position.x - transform.position.x, _target.transform.position.z - transform.position.z).magnitude;
            if(distance<_unit._attackRange)
            {
                nav.SetDestination(transform.position);
                State = Define.State.Attack;
                return;
            }
            else
            {
                Debug.Log("사정거리 밖");
                nav.SetDestination(_target.transform.position);
               
            }
        }
        else
        {
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
                NavMeshAgent nav = gameObject.GetOrAddComponent<NavMeshAgent>();
                nav.isStopped = true;
                transform.LookAt(_target.transform);
     
               if(_isDelay==false)
                {
                    SplitAnimAttack();
                    //_anim.Play(_attackHash);


              
                    _anim.SetTrigger(_attackTrigger);
                   // StartCoroutine((CoAttack(_unit._attackTiming)));
                    StartCoroutine(CoAttackDelay(_unit._attackDelay));
                    Debug.Log("공격입력");
                    //애니메이션 이벤트에서 공격실행
       
                }
            }
            else
            {
                _target = null;
                
                State = Define.State.Moving;
            }
        }
      
    }
    public void SplitAnimAttack()
    {
        if (_unit._targetMovementType == Define.MovemnetType.Both && _unit._isSplitAttackAnim)
        {
            Unit tempUnit = _target.GetComponent<Unit>();
            if (tempUnit != null)
            {
                if (tempUnit._movementType == Define.MovemnetType.Ground)
                {
                    _anim.SetFloat("IsAir", 0);
                }
                else
                {
                    _anim.SetFloat("IsAir", 1);
                }
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
                _meeleAttack.DoAttack(_target);
                break;
            case Define.AttackType.Arange:
                _arangeAttack.DoAttack(_target);
                break;
            case Define.AttackType.Both:
                float distance = new Vector2(_target.transform.position.x - transform.position.x, _target.transform.position.z - transform.position.z).magnitude;
                if(distance<=2.5f)
                {
                    _meeleAttack.DoAttack(_target);
                }
                else
                {
                    _arangeAttack.DoAttack(_target);
                }
                break;
        }


    }
    IEnumerator CoAttackDelay(float delay)
    {
        _isDelay = true;
        yield return new WaitForSeconds(delay);
        _isDelay = false;
    }
    IEnumerator CoAttack(float timing = 0.2f)
    {
        yield return new WaitForSeconds(0.1f);
        yield return new WaitForSeconds(timing);
        AttackToTarget();
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

    public bool CheckCanAttackType(Unit target)
    {

        if(_unit._targetMovementType!=target._movementType&&_unit._targetMovementType != Define.MovemnetType.Both) return false;
        if(_unit._targetType!=target._unityType&&_unit._targetType!=Define.TargetType.Both) return false;
        return true;
    }

    public void TakeDamage(int damage,Define.UnitElementalType type)
    {
        if(damage<0)
        {
            damage = 1;
        }

        Debug.Log("HitDamage");
        int trueDamage = Mathf.RoundToInt(damage);
    }
    public void TakeDamage(int damage, UnitController fromUnit)
    {
        if (damage < 0)
        {
            damage = 1;
        }
        if(_target==null)
        {
            if(CheckCanAttackType(fromUnit._unit))
            {
                _target = fromUnit.gameObject;
            }
        }
        float eleDamage = ElementalCalculate.GetMultiplier(fromUnit._unit._unitElementalType, _unit._unitElementalType);
        Debug.Log("HitDamage");
        int trueDamage = Mathf.RoundToInt(damage*eleDamage);
        if(trueDamage<0)
        {
            trueDamage = 1;
        }
    }

}
