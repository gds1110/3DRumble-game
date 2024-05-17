using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Analytics.Internal;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.GridLayoutGroup;


[AddComponentMenu("My Unit/Unit Controller")]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(NavMeshAgent))]
//[RequireComponent(typeof(Attack))]

public class UnitController : Controller, IConquerAble
{

    private readonly int _attackHash = Animator.StringToHash("ATTACK");
    private readonly int _moveHash = Animator.StringToHash("WALK");
    private readonly int _deadHash = Animator.StringToHash("DEAD");
    private readonly int _idleHash = Animator.StringToHash("IDLE");
    private readonly int _attackTrigger = Animator.StringToHash("ATTACKTRIGGER");

    public bool isChange = false;

    [SerializeField] private Define.State _state = Define.State.Idle;
    Animator _anim;

    public float _channelingTime;

    [SerializeField] float _scanRange = 10;
    public Transform _barrel;

    public bool _isPlaced = false;
    [SerializeField]
    public Transform _destPos;

    public Define.State State {
        get { return _state; }
        set { _state = value;
            switch (_state)
            {
                case Define.State.Die:
                    _anim.CrossFade(_deadHash, 0.1f);
                    if (_otherConquerAble != null)
                    {
                        StopConquer(this);
                    }
                    break;
                case Define.State.Moving:
                    _anim.CrossFade(_moveHash, 0.1f);
                    if (_otherConquerAble != null)
                    {
                        StopConquer(this);
                    }
                    break;
                case Define.State.Idle:
                    _anim.CrossFade(_idleHash, 0.1f);
                    if (_otherConquerAble != null)
                    {
                        StopConquer(this);
                    }
                    break;
                case Define.State.Attack:
                    //_anim.CrossFade(_attackHash, 0.1f,-1,0);
                    if (_otherConquerAble != null)
                    {
                        StopConquer(this);
                    }
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
        if (_unit._attackType == Define.AttackType.Meele && _meeleAttack == null)
        {
            _meeleAttack = Util.GetOrAddComponent<MeeleAttack>(gameObject);
        }
        if (_unit._attackType == Define.AttackType.Arange && _arangeAttack == null)
        {
            _arangeAttack = Util.GetOrAddComponent<ArangeAttack>(gameObject);
        }
        if (_unit._movementType == Define.MovemnetType.Aerial)
        {
            GetComponent<NavMeshAgent>().baseOffset = 2;
        }
        _unit.DeadAction.AddListener(OnDead);

        CapsuleCollider cp = GetComponent<CapsuleCollider>();
        cp.isTrigger = true;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        rb.constraints |= RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        NavMeshAgent nav = GetComponent<NavMeshAgent>();
        if (nav)
        {
            nav.angularSpeed = 720.0f;
            nav.acceleration = 100.0f;
        }

    }
    public void OnDead()
    {
        State = Define.State.Die;
        _otherConquerAble = null;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        NavMeshAgent nav = GetComponent<NavMeshAgent>();
        nav.enabled = false;
        _isPlaced = false;
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

            case Define.State.Channeling:
                UpdateChanneling();
                break;
        }
    }
    void UpdateChanneling()
    {
        Conquering(this);
    }
    void UpdateDie()
    {

    }
    void UpdateMoving()
    {
        NavMeshAgent nav = gameObject.GetOrAddComponent<NavMeshAgent>();
        nav.isStopped = false;
        FindTarget();

        if (_target != null)
        {
            float distance = new Vector2(_target.transform.position.x - transform.position.x, _target.transform.position.z - transform.position.z).magnitude;
            if (distance < _unit._attackRange)
            {
                nav.SetDestination(transform.position);
                State = Define.State.Attack;
                return;
            }
            else
            {
                if (nav.isStopped == true)
                {
                    nav.SetDestination(_target.transform.position);
                    nav.speed = _unit._speed;
                }
                else
                {
                    nav.isStopped = false;
                    nav.SetDestination(_target.transform.position);

                }

            }
        }
        else
        {
           if(_destPos)
            {
             
                nav.SetDestination(_destPos.position);
            }

        }

    }

    void UpdateIdle()
        {
            FindTarget();

            if (_target)
            {
                State = Define.State.Moving;
            }
            else
            {
                NavMeshAgent nav = gameObject.GetOrAddComponent<NavMeshAgent>();
                nav.SetDestination(_destPos.position);
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
                    //transform.LookAt(_target.transform);

                    if (_isDelay == false)
                    {
                        SplitAnimAttack();
                        //_anim.Play(_attackHash);

                        if(_target)
                        transform.LookAt(_target.transform);

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

                State = Define.State.Idle;
                }
        }
            else
        {
            _target = null;
            State = Define.State.Idle;
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
        private void OnTriggerEnter(Collider other)
        {
            if (State == Define.State.Attack) return;

            if (other.GetComponent<InteractZone>() != null)
            {
                if (this._unit._movementType == Define.MovemnetType.Aerial) return;
                if (other.GetComponent<InteractZone>()._isConquering) return;
                if (other.GetComponentInParent<IConquerAble>() != null && other.GetComponentInParent<Controller>()._owner != _owner)
                {
                    other.GetComponentInParent<IConquerAble>().StartConquer(this);
                    StartConquer(other.GetComponentInParent<Controller>());
                }
            }
        }

        public void AttackToTarget()
        {
            if (_target == null)
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
                    if (distance <= 2.5f)
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

        IEnumerator CoAttack(float timing = 0.2f)
        {
            yield return new WaitForSeconds(0.1f);
            yield return new WaitForSeconds(timing);
            AttackToTarget();
        }
    public void MustFindTarget()
    {
        if (State == Define.State.Attack) return;
        float closestDistance = Mathf.Infinity;
        HashSet<Controller> tempH = Managers.Game.allUnits;
        GameObject tempTarget = null;
        foreach (Controller controller in tempH)
        {
            if (controller == this) continue;
            if (!CheckCanAttackType(controller.GetComponent<BaseCombat>())) continue;
            if (_owner == controller._owner) continue;
            if (gameObject.tag == controller.tag) continue;
            if (controller._owner == Define.WorldObject.None || controller._owner == Define.WorldObject.Unknown) continue;
            float sqrDistance = (transform.position - controller.transform.position).sqrMagnitude;
            if (sqrDistance < closestDistance)
            {
                tempTarget = controller.gameObject;
                closestDistance = sqrDistance;
            }

        }

       
        _target = tempTarget;
        State = Define.State.Moving;

    }
    public override void FindTarget()
    {
        base.FindTarget();
        if (State == Define.State.Attack) return;
        float closestDistance = Mathf.Infinity;
        HashSet<Controller> tempH = Managers.Game.allUnits;
        GameObject tempTarget = null;
        foreach (Controller controller in tempH)
        {
            if (controller == this) continue;
            if (!CheckCanAttackType(controller.GetComponent<BaseCombat>())) continue;
            if (_owner == controller._owner) continue;
            if (gameObject.tag == controller.tag) continue;
            if (controller._owner == Define.WorldObject.None || controller._owner == Define.WorldObject.Unknown) continue;
            float sqrDistance = (transform.position - controller.transform.position).sqrMagnitude;
            if (sqrDistance < closestDistance)
            {
                tempTarget = controller.gameObject;
                closestDistance = sqrDistance;
            }

        }
        if (tempTarget != null)
        {
            float sqrDistance = (transform.position - tempTarget.transform.position).sqrMagnitude;
            if (sqrDistance < _scanRange * _scanRange)
                _target = tempTarget;
            State = Define.State.Moving;
        }

    } 

    IEnumerator CoConquer(float timing =5f)
    {
        float time = 0.0f;
        while(time<1.0f)
        {
            time += Time.deltaTime / timing;

            yield return null;
        }
    }

    public void StartConquer(Controller fromUnit)
    {
        State = Define.State.Channeling;
        _otherConquerAble = fromUnit;
       
    }

    public void EndConquer(Controller fromUnit)
    {
        _otherConquerAble = null;
        State = Define.State.Moving;
        NavMeshAgent nav = gameObject.GetOrAddComponent<NavMeshAgent>();
        nav.isStopped = false;
    }

    public void Conquering(Controller fromUnit)
    {
        if (_otherConquerAble != null)
        {
            transform.LookAt(_otherConquerAble.transform);
            NavMeshAgent nav = gameObject.GetOrAddComponent<NavMeshAgent>();
            nav.isStopped = true;

            _otherConquerAble.GetComponent<IConquerAble>().Conquering(fromUnit);



        }
    }

    public void StopConquer(Controller fromUnit)
    {

        _otherConquerAble = null;
        State = Define.State.Moving; 
        


    }

}
