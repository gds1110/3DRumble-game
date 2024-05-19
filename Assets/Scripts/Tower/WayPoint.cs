using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WayPoint : MonoBehaviour
{

    public Define.WorldObject _owner;
    // Start is called before the first frame update

    private void Awake()
    {
        //if (GetComponentInParent<Controller>()) _owner = GetComponentInParent<Controller>()._owner;
        //if (GetComponent<Controller>()) _owner = GetComponentInParent<Controller>()._owner;

    }
    void Start()
    {
        Managers.Game.wayPoints.Add(this);
        GetComponent<BoxCollider>().isTrigger = true;

        if(GetComponent<Tower>()!=null)
        {

        }

    }
    void OnDead()
    {
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<UnitController>() != null)
        {
            
            UnitController unitController = other.GetComponent<UnitController>();
            if (unitController._owner == _owner) return;
            //switch (unitController._owner)
            //{
            //    case Define.WorldObject.Unknown:
            //        break;
            //    case Define.WorldObject.Player:
            //        unitController._target = Managers.Game.EnemyAlter;
            //        break;
            //    case Define.WorldObject.Monster:
            //        unitController._target = Managers.Game.playerAlter;
            //        break;
            //    case Define.WorldObject.None:
            //        break;
            //}
             Managers.Game.MustGetTower(unitController);
             if (unitController._target == null) unitController.MustFindTarget();
        }


    }
    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.GetComponent<UnitController>() != null)
    //    {

    //        UnitController unitController = other.GetComponent<UnitController>();
    //        if (unitController._owner == _owner) return;
    //        if (unitController._target == null) unitController.MustFindTarget();
    //    }
    //}
}
