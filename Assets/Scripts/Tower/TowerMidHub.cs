using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerMidHub : MonoBehaviour
{
    public Material _greyMaa;
    public Material _redMaa;
    public Material _blueMaa;
    [SerializeField]
    MeshRenderer[] changeRenderes;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TowerController>().ConquerEvent.AddListener(SetMat);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetMat(Define.WorldObject type)
    {
        Material mat = _greyMaa;

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

        for (int i = 0; i < changeRenderes.Length; i++)
        {
            changeRenderes[i].material = mat;
        }
    }
}
