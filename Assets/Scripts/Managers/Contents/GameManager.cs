using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;


public class GameManager 
{
    GameObject _player;
   // Dictionary<int, GameObject> _monsters = new Dictionary<int, GameObject>();
   public HashSet<GameObject> _monsters= new HashSet<GameObject>();

    public Define.WorldObject _playerType = Define.WorldObject.Player;
    public Action<int> OnSpawnEvent;
    public Action GameOverTimeEvent;
    public int _score=0;
    public bool _isBattle = false;
    public float _playTime = 0;

    //Place On Off
    public Action OnPlaceEvent;
    public Action OffPlaceEvent;
    
    // _ghost setactive false , cardunselected , costminus
    public Action SpawnCardEvent;

    public GameObject playerAlter;
    public GameObject EnemyAlter;

    public HashSet<WayPoint> wayPoints = new HashSet<WayPoint>();
    public HashSet<Controller> allUnits = new HashSet<Controller>();
    public HashSet<PlaceableZone> allPlaceZone = new HashSet<PlaceableZone>();


    public bool GameWin = false;

    public void SetGameEnding(bool isWin)
    {
        GameWin = isWin;
        Managers.Scene.LoadScene(Define.Scene.EndingScene);
    }

    public void Init()
    {
    }

    public GameObject Spawn(Define.WorldObject type,string path,Transform parent = null)
    {
        GameObject go =  Managers.Resource.Instantiate(path, parent);

        switch (type)
        {
            case Define.WorldObject.Monster:
                _monsters.Add(go);
                if(OnSpawnEvent!=null)
                {
                    OnSpawnEvent.Invoke(1);
                }
                break;
            case Define.WorldObject.Player:
                _player = go;
                break;


        }

        return go;
    }

    public GameObject SpawnEnemy(Transform parent = null,string name = null)
    {
       
        GameObject go = Managers.Resource.Instantiate($"Enemys/{name}");

        go.transform.SetParent(parent);
        _monsters.Add(go);
        if (OnSpawnEvent != null)
            OnSpawnEvent.Invoke(1);

        return go;

    }


    public GameObject CardSpawn(Define.WorldObject type,GameObject original,Transform transform,Transform parent = null)
    {
        GameObject go = GameObject.Instantiate(original, transform.position,transform.rotation,parent);
        SetUnitInfo(type, go);
        return go;
    }
    public GameObject CardSpawn(Define.WorldObject type,GameObject original,Vector3 Pos, Transform parent = null)
    {
        GameObject go = GameObject.Instantiate(original,Pos,Quaternion.identity,parent);
        SetUnitInfo(type, go);
        return go;
    }
    void SetUnitInfo(Define.WorldObject type,GameObject go)
    {
        go.GetComponent<Unit>()._owner = type;
        go.GetComponent<UnitController>()._owner = type;
        switch (type)
        {
            case Define.WorldObject.Unknown:
                break;
            case Define.WorldObject.Player:
                go.tag = "Player";
                go.GetComponent<UnitController>()._destPos = GetNearWaypoint(go.GetComponent<Controller>());
                SpawnCardEvent?.Invoke();
                OffPlaceEvent?.Invoke();
                break;
            case Define.WorldObject.Monster:
                go.tag = "Enemy";
                go.GetComponent<UnitController>()._destPos = GetNearWaypoint(go.GetComponent<Controller>());
                break;
            case Define.WorldObject.None:
                break;
        }
        go.GetComponent<UnitController>()._isPlaced = true;
        go.transform.LookAt(GetNearWaypoint(go.GetComponent<Controller>()));
        go.GetComponent<UnitController>().OnPlace();
        allUnits.Add(go.GetComponent<UnitController>());
    }

    public GameObject GetPlayer()
    {
        return _player;
    }
    public void SetPlayer(GameObject player)
    {
        _player = player;
    }

    public Define.WorldObject GetWorldObjectType(GameObject go)
    {
        return Define.WorldObject.Unknown;
    }
    public void Despawn(GameObject go)
    {

        if(go.GetComponent<Controller>() != null)
        {
            if(allUnits.Contains(go.GetComponent<Controller>()))
            {
                allUnits.Remove(go.GetComponent<Controller>());
            }
        }
        if(go.GetComponent<WayPoint>()!=null)
        {
            if(wayPoints.Contains(go.GetComponent<WayPoint>())) { 
                 wayPoints.Remove(go.GetComponent<WayPoint>());
            }
        }
        Managers.Resource.Destroy(go);
    }

    public void AddScore(int score)
    {
        _score += score;
    }

    public void GameOver()
    {
        _isBattle = false;
        GameOverTimeEvent?.Invoke();
        float maxPlayTime = PlayerPrefs.GetFloat("PlayTime",0);
        if(_playTime > maxPlayTime)
        {
            PlayerPrefs.SetFloat("PlayTime", _playTime);
        }
        int maxScore = PlayerPrefs.GetInt("MaxScore",0);
        if (_score > maxScore)
        {
            PlayerPrefs.SetInt("MaxScore", _score);
        }

        Managers.Scene.LoadScene(Define.Scene.EndingScene);
    }

    public Transform GetNearWaypoint(Controller go)
    {
        Transform closeTransform = null;
        float closDistance = 9999999999999f;
        foreach (WayPoint obj in wayPoints)
        {
            if (obj._owner == go._owner) continue;

            float distance = Vector3.Distance(go.transform.position, obj.transform.position);
            if (distance < closDistance)
            {
                closDistance = distance;
                closeTransform = obj.transform;
            }
        }

        return closeTransform;
    }
}
