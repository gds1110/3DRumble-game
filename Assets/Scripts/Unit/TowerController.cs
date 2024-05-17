using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class TowerController : Controller,IConquerAble
{

    [SerializeField] private Define.State _state = Define.State.Idle;
    public float Arrange;
    public Define.State State
    {
        get { return _state; }
        set
        {
            _state = value;
            switch (_state)
            {
                case Define.State.Die:
                    break;
                case Define.State.Moving:
                case Define.State.Idle:

                    break;
                case Define.State.Attack:
                    break;
            }

        }

    }
    public bool _isAttackTower = false;
    public UnityEvent<Define.WorldObject> ConquerEvent;

    private void Awake()
    {
        _unit = GetComponent<Tower>();
        _interactZone = GetComponentInChildren<InteractZone>();

    }

    // Start is called before the first frame update
    void Start()
    {

        if (_isAttackTower == true)
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
        }
        _unit.DeadAction.AddListener(OnDead);

        if (gameObject.tag=="Enemy")
        {
            _owner = Define.WorldObject.Monster;
        }
       else if(gameObject.tag=="Player")
        {
            _owner = Define.WorldObject.Player;
        }
       else
        {
            _owner = Define.WorldObject.Unknown;
        }
        Arrange = _unit._attackRange;

        Managers.Game.allUnits.Add(this);
    }
    public void OnDead()
    {
        State = Define.State.Die;
        _otherConquerAble = null;

    }
    // Update is called once per frame
    void Update()
    {

        if(_owner==Define.WorldObject.Unknown) return;
        switch (_state)
        {
            case Define.State.Die:
                UpdateDie();
                break;
            case Define.State.Moving:
  
            case Define.State.Idle:
                UpdateIdle();
                break;
            case Define.State.Attack:
                UpdateAttack();
                break;
        }
    }

    private void UpdateDie()
    {
      
    }

    private void UpdateAttack()
    {
        if (_target != null)
        {

            float distance = new Vector2(_target.transform.position.x - transform.position.x, _target.transform.position.z - transform.position.z).magnitude;
            if (distance < _unit._attackRange)
            {
                if (_isDelay == false)
                {
                    StartCoroutine(CoAttackDelay(_unit._attackDelay));
                    AttackToTarget();
                }

            }

        }
        else
        {
            State = Define.State.Idle;
        }
    }

    private void UpdateIdle()
    {
        if (gameObject.tag == "None") return;
        if (!_isAttackTower) return;
        FindTarget();
        if (_target != null)
        {
            float distance = new Vector2(_target.transform.position.x - transform.position.x, _target.transform.position.z - transform.position.z).magnitude;
            if (distance < _unit._attackRange)
            {
                State = Define.State.Attack;
                return;
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
            //if (!CheckCanAttackType(controller.GetComponent<BaseCombat>())) continue;
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
            if (sqrDistance < _unit._attackRange* _unit._attackRange)
                _target = tempTarget;
          
        }
    }
    public void StartConquer(Controller fromUnit)
    {

        _interactZone.StartConquer(fromUnit);
    }

    public void EndConquer(Controller fromUnit)
    {
        _owner = _interactZone._owner;
        ConquerEvent?.Invoke(fromUnit._owner);
        Debug.Log("EndConquer!!");
    }

    public void Conquering(Controller fromUnit)
    {
        _interactZone.Conquering(fromUnit);
    }

    public void StopConquer(Controller fromUnit)
    {
       
    }
}
