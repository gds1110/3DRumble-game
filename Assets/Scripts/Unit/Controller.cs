using System.Collections;
using System.Collections.Generic;
using Unity.Loading;
using UnityEngine;


public class Controller : MonoBehaviour
{
    [SerializeField]
    public BaseCombat _unit;
    [SerializeField]
    public GameObject _target;

    public MeeleAttack _meeleAttack;
    public ArangeAttack _arangeAttack;
    public bool _isDelay = false; 
    public Define.WorldObject _owner;
    public Controller _otherConquerAble;
    public InteractZone _interactZone;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CheckCanAttackType(BaseCombat target)
    {
        if(target._isUnbreakable) return false;
        if(_unit._unityType==Define.TargetType.Building) { return true; }
        if (_unit._targetMovementType != target._movementType && _unit._targetMovementType != Define.MovemnetType.Both) return false;
        if (_unit._targetType != target._unityType && _unit._targetType != Define.TargetType.Both) return false;
        return true;
    }
    public void OnDeath()
    {

    }
    public virtual void FindTarget()
    {

        

        //Collider[] cols = Physics.OverlapSphere(transform.position, _unit._scanRange, (1 << 3|1<<8));
        //if (cols.Length > 0)
        //{
        //    for (int i = 0; i < cols.Length; i++)
        //    {
        //        if (cols[i].tag != gameObject.tag)
        //        {
        //            BaseCombat tempTarget = cols[i].gameObject.GetComponent<BaseCombat>();
        //            Controller controller = tempTarget.GetComponent<Controller>();
        //            if (controller._owner == Define.WorldObject.Unknown) continue;
        //            if(tempTarget._isUnbreakable) { continue; }
        //            if (tempTarget != null && CheckCanAttackType(tempTarget))
        //                _target = tempTarget.gameObject;

        //            break;

        //        }
        //    }
        //}

   
    }
    public IEnumerator CoAttackDelay(float delay)
    {
        _isDelay = true;
        yield return new WaitForSeconds(delay);
        _isDelay = false;
    }
}
