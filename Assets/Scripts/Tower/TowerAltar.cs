using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WayPoint))]
public class TowerAltar : MonoBehaviour
{
    Define.WorldObject _owner;
    public UnitData _centryTowerOrigin;
    public int UnbreakableCount = 0;

    //public Action UnbreakableCountAction;

    private void Start()
    {
        _owner = GetComponent<Tower>()._worldObject;
        if(_owner==Define.WorldObject.Monster)
            Managers.Game.EnemyAlter = this.gameObject;
        else
            Managers.Game.playerAlter = this.gameObject;

        if (_centryTowerOrigin)
        {
            GameObject leftTower = Instantiate(_centryTowerOrigin.FriendlyUnit, transform.position + new Vector3(-10, 0, 0), transform.rotation,transform);
            GameObject RightTower = Instantiate(_centryTowerOrigin.FriendlyUnit, transform.position + new Vector3(10, 0, 0), transform.rotation,transform);
            leftTower.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
            RightTower.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
            leftTower.GetComponent<Tower>()._worldObject=_owner;
            leftTower.GetComponent<TowerController>()._owner=_owner;
            leftTower.tag = gameObject.tag;  
            RightTower.GetComponent<Tower>()._worldObject=_owner;
            RightTower.GetComponent<TowerController>()._owner=_owner;
            RightTower.tag = gameObject.tag;
            leftTower.GetComponent<TowerCentry>().SetMat(_owner);
            RightTower.GetComponent<TowerCentry>().SetMat(_owner);
            leftTower.GetComponent<TowerCentry>().DeadAction.AddListener(CountUnbreakable);
            RightTower.GetComponent<TowerCentry>().DeadAction.AddListener(CountUnbreakable);
           
        }
       
    }

    public void CountUnbreakable()
    {
        UnbreakableCount++;
        if(UnbreakableCount>=2)
        {
            GetComponent<Tower>()._isUnbreakable = false;
        }
    }
}
