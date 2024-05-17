using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class SpawnBrain : MonoBehaviour
{

    public PlayerDeck _deck;
  

    public float _manaGage = 0;
    public int _maxManaGage = 10;

    public UnitData nextUnit;
    public Define.WorldObject _owner;

    private void Start()
    {
        _owner = Define.WorldObject.Monster;
        StartCoroutine(CoManaCheckAndSapwn());
    }

    public void ManaGen()
    {
        _manaGage += Time.deltaTime * 0.5f;
        if (_manaGage > _maxManaGage)
        {
            _manaGage = _maxManaGage;
        }

    }
    void Update()
    {

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        //Mana
        ManaGen();
        if(nextUnit ==null)
        {
            nextUnit = GetRandomUnitdata();
        }
    }
    UnitData GetRandomUnitdata()
    {
        int randomNum = 0;
        if (_deck.unitDatas.Count > 0)
        {
            randomNum = UnityEngine.Random.Range(0, _deck.unitDatas.Count);

        }
        return _deck.unitDatas[randomNum];
    }

    public Vector3 GetRandomPoint(GameObject SpawnZone)
    {
        // Vector3 zoneSize = SpawnZone.transform.localScale;

        //Vector3 randomPos = new Vector3(Random.Range(-zoneSize.x / 2, zoneSize.x / 2), 0, Random.Range(-zoneSize.z / 2, zoneSize.z / 2));

        MeshRenderer meshCollider = SpawnZone.GetComponent<MeshRenderer>();
        Bounds bounds = meshCollider.bounds;
        float xExtent = bounds.extents.x;
        float zExtent = bounds.extents.z;
        Debug.Log($"x : {xExtent} + z:{zExtent}");

        Vector3 randomPos = new Vector3(Random.Range(-xExtent, xExtent), 0, Random.Range(-zExtent, zExtent));



        return randomPos;
    }

    void SpawnUnit()
    {
        if (!nextUnit) return;
        if (nextUnit.cost <= _manaGage)
        {

            PlaceableZone zone = getRandomZone();
            if (zone)          
             {
                Vector3 pos = zone.transform.position+ GetRandomPoint(zone.gameObject);
                Debug.Log($"x : {pos.x} + y : {pos.y}+z:{pos.z}");
              GameObject go =   Managers.Game.CardSpawn(_owner, nextUnit.HostileUnit, pos);
                if(go)
                {
                    _manaGage-= nextUnit.cost;
                    nextUnit = null;

                }
             }   
      
        }
    }
    PlaceableZone getRandomZone()
    {
       HashSet<PlaceableZone> zones =  Managers.Game.allPlaceZone;
        List<PlaceableZone> randZones = new List<PlaceableZone>();
        foreach(PlaceableZone zone in zones)
        {
            if (zone._owner != _owner) continue;
            randZones.Add(zone);
        }
        int randnum = 0;
        if (randZones.Count>0)
        {
             randnum = Random.Range(0,randZones.Count);

        }
        Debug.Log(randZones[randnum].name);
        return randZones[randnum];
    }

    IEnumerator CoManaCheckAndSapwn()
    {
        while (true)
        {
            SpawnUnit();
            yield return new WaitForSeconds(1f);
        }

    }
}
