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
    NavMeshAgent _nav;
    public bool isChange = false;

    [SerializeField] private Define.State _state = Define.State.Idle;
    Animator _anim;

    public float _channelingTime;

    [SerializeField] float _scanRange = 10;
    public Transform _barrel;

    public bool _isPlaced = false;
    [SerializeField]
    public Transform _destPos;

    public Define.State State
    {
        get { return _state; }
        set
        {
            _state = value;
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
                    _nav.isStopped = false;
                    _anim.CrossFade(_moveHash, 0.1f);
                    if (_otherConquerAble != null)
                    {
                        StopConquer(this);
                    }
                    break;
                case Define.State.Idle:
                    _nav.isStopped = false;
                    _anim.CrossFade(_moveHash, 0.1f);
                    if (_otherConquerAble != null)
                    {
                        StopConquer(this);
                    }
                    break;
                case Define.State.Attack:

                    if (_otherConquerAble != null)
                    {
                        StopConquer(this);
                    }
                    break;
                case Define.State.Channeling:
                    break;

            }

        }

    }


    private void Awake()
    {

        _anim = GetComponent<Animator>();
        _unit = GetComponent<Unit>();
        _nav = GetComponent<NavMeshAgent>();
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

  
            _nav.angularSpeed = 720.0f;
            _nav.acceleration = 100.0f;
        

    }
    public void OnDead()
    {
        State = Define.State.Die;
        _otherConquerAble = null;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        _nav.speed=0;
        _nav.angularSpeed = 0;
        _isPlaced = false;
    }
    public void OnPlace()
    {
        _nav.enabled = true;
        _nav.isStopped = false;
        if(_destPos)
           _nav.SetDestination(_destPos.position);
        State = Define.State.Moving;
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
        if(_nav)
            _nav.isStopped = true;

        Conquering(this);
    }
    void UpdateDie()
    {

    }
    void UpdateMoving()
    {
        if(_nav.isStopped==true)
        {
            _nav.isStopped = false;
        }

        if(_target|| CheckInScanArange())
        {
          
                _nav.SetDestination(_target.transform.position);
            if(CheckInArange())
                State = Define.State.Attack;
            
        }
        else if(!_target)
        {
            _nav.SetDestination(_destPos.transform.position);
        }
        else
        {
            MustFindTarget();
        }

    }

    void UpdateIdle()
    {
        if(CheckInScanArange())
        {
            State = Define.State.Moving;
        }
        else
        {
            State = Define.State.Moving;
        }
    }

    void UpdateAttack()
    {
        if (_target != null)
        {
            if (CheckInArange())
            {
                Vector3 direction = _target.transform.position - transform.position;
                Quaternion targetRot = Quaternion.LookRotation(direction);
                gameObject.transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10 * Time.deltaTime);
                _nav.isStopped = true;

                if (_isDelay == false)
                {
                    _nav.SetDestination(transform.position);
                    SplitAnimAttack();

                    _isDelay = true;

                    StartCoroutine(CoAttackDelay(_unit._attackDelay));
                    if (_target!=null)
                    {
                        Debug.Log($"Attack Trriger : {_target.name}");
                        _anim.SetTrigger(_attackTrigger);
                    }
                }
            }
            else
            {
             
                _nav.SetDestination(_target.transform.position);
                State = Define.State.Moving;
            }
        }
        else
        {
            State = Define.State.Moving;
            if (_destPos)
            {
                _nav.SetDestination(_destPos.position);
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
    //private void OnTriggerEnter(Collider other)

    //{

    //    if (State == Define.State.Attack) return;



    //    if (other.GetComponent<InteractZone>() != null)

    //    {
    //        if (this._unit._movementType == Define.MovemnetType.Aerial) return;

    //        if (other.GetComponent<InteractZone>()._isConquering) return;

    //        if (other.GetComponentInParent<IConquerAble>() != null && other.GetComponentInParent<Controller>()._owner != _owner)
    //        {

    //            other.GetComponentInParent<IConquerAble>().StartConquer(this);
    //            StartConquer(other.GetComponentInParent<Controller>());
    //        }
    //    }

    //}

  

    public void AttackToTarget()
    {
       // _anim.ResetTrigger("_attackTrigger");

        if (_target == null)

        {
            _anim.StopPlayback();
            State = Define.State.Moving;
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
        if (_target!=null) return;
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
   

   public bool CheckInArange()
    {
        return (transform.position-_target.transform.position).sqrMagnitude<=_unit._attackRange*_unit._attackRange;
    } 
    public bool CheckInScanArange(Transform target)
    {
        return (transform.position- target.transform.position).sqrMagnitude<=_unit._scanRange*_unit._scanRange;
    }

    public bool CheckInScanArange()
    {
        bool retBool = false;
        float closestDistance = Mathf.Infinity;
        HashSet<Controller> tempH = Managers.Game.allUnits;
        GameObject tempTarget = null;
        foreach (Controller controller in tempH)
        {
            if (!controller) continue;
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
        if (tempTarget)
        {
            if (CheckInScanArange(tempTarget.transform))
            {
                retBool = true;
                _target = tempTarget;
            }
        }
        return retBool;
    }


    public override void FindTarget()
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
    }

    IEnumerator CoConquer(float timing = 5f)
    {
        float time = 0.0f;
        while (time < 1.0f)
        {
            time += Time.deltaTime / timing;

            yield return null;
        }
    }

    public void StartConquer(Controller fromUnit)
    {
        State = Define.State.Channeling;
        _otherConquerAble = fromUnit;
        transform.LookAt(_otherConquerAble.transform);
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
            _otherConquerAble.GetComponent<IConquerAble>().Conquering(fromUnit);
        }
    }

    public void StopConquer(Controller fromUnit)
    {
        _otherConquerAble.GetComponent<IConquerAble>().StopConquer(this);
        _otherConquerAble = null;
        //State = Define.State.Moving;
    }

}
