using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableZone : MonoBehaviour
{
    public Define.WorldObject _owner;

    [SerializeField]
    Material[] materials = null;

    private void Start()
    {
        SetOwner(_owner);
    }

    void SetOwner(Define.WorldObject owner)
    {
        _owner = owner;
        int num = 0;
        switch (owner)
        {
            case Define.WorldObject.Unknown:
                num = 0;
                break;
            case Define.WorldObject.Player:
                num = 1;

                break;
            case Define.WorldObject.Monster:
                num = 2;

                break;
        }

        GetComponent<MeshRenderer>().material = materials[num];
    }

}
