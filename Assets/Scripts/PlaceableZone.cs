using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableZone : MonoBehaviour
{
    public Define.WorldObject _owner;

  
    [SerializeField]
    PlaceableMaterials _placeMaterials;
    private void Start()
    {
        SetOwner(_owner);

        Managers.Game.OnPlaceEvent -= OnPlace;
        Managers.Game.OnPlaceEvent += OnPlace;
        Managers.Game.OffPlaceEvent -= OffPlace;
        Managers.Game.OffPlaceEvent += OffPlace;
        if (GetComponentInParent<TowerController>())
        {
            GetComponentInParent<TowerController>().ConquerEvent.AddListener(SetOwner);
        }
        Managers.Game.allPlaceZone.Add(this);

        OffPlace();
    }
    void OnPlace()
    {
        gameObject.SetActive(true);
    }
    void OffPlace()
    {
        gameObject.SetActive(false);

    }

    void SetOwner(Define.WorldObject owner)
    {
        _owner = owner;

        Material material = _placeMaterials.unkownMaterial;
        switch (owner)
        {
            case Define.WorldObject.Unknown:
                material = _placeMaterials.unkownMaterial;
                break;
            case Define.WorldObject.Player:
                material = _placeMaterials.ableMaterial;

                break;
            case Define.WorldObject.Monster:
                material = _placeMaterials.unableMaterial;

                break;
        }

        GetComponent<MeshRenderer>().material = material;
    }

}
