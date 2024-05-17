using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TowerCentry : MonoBehaviour
{
    public Material _greyMaa;
    public Material _redMaa;
    public Material _blueMaa;
    [SerializeField]
    MeshRenderer[] changeRenderes;

    public UnityEvent DeadAction;

    private void Start()
    {
        GetComponent<Tower>().DeadAction.AddListener(OnDead);
        GetComponent<TowerController>().ConquerEvent.AddListener(SetMat);
    }
    public void OnDead()
    {
        DeadAction?.Invoke();
    }

    public void SetMat(Define.WorldObject type)
    {
        Material mat= _greyMaa;
     
        switch (type)
        {
            case Define.WorldObject.Unknown:
                mat = _greyMaa;
                break;
            case Define.WorldObject.Player:
                mat = _blueMaa;
                break;
            case Define.WorldObject.Monster:
                mat = _redMaa;
                break;
            case Define.WorldObject.None:
                mat = _greyMaa;
                break;
        }

        for(int i=0;i<changeRenderes.Length;i++)
        {
            changeRenderes[i].material = mat;
        }
    }
}
