using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.UI.GridLayoutGroup;

public class PlaceObject : MonoBehaviour
{

    public GameObject _ghost;
    public GameObject _placed;

    Dictionary<UnitData, GameObject> _placeGhosts = new Dictionary<UnitData, GameObject>();
    Dictionary<GameObject, SkinnedMeshRenderer[]> _GhostsRenderers = new Dictionary<GameObject, SkinnedMeshRenderer[]>();
    UnitData _currentUnitdata;
    [SerializeField]
    UnitData[] _unitDatas;
    [SerializeField]
    PlaceableMaterials _placeMaterials;
    [SerializeField]
    Define.WorldObject _currentUnder=Define.WorldObject.None;
    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        if(_unitDatas.Length>0)
        {
            for(int i=0;i<_unitDatas.Length;i++)
            {
                GameObject tempGhost = Instantiate(_unitDatas[i].PlaceGhost,this.gameObject.transform);
                _placeGhosts.Add(_unitDatas[i], tempGhost);
                SkinnedMeshRenderer[] _meshs = tempGhost.GetComponentsInChildren<SkinnedMeshRenderer>();
                _GhostsRenderers.Add(tempGhost, _meshs);
                tempGhost.SetActive(false);

                var go = Instantiate(_unitDatas[i].FriendlyUnit);
                    Managers.Game.Despawn(go);
               

            }

        }
        //SetGhost(_unitDatas[5]);
        if (_ghost)
        {
            _ghost.transform.localScale = Vector3.one * 2;
        }
        Managers.Game.SpawnCardEvent -= UnSetGhost;
        Managers.Game.SpawnCardEvent += UnSetGhost;
    }

    // Update is called once per frame
    void Update()
    {

        if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("UI Hit");
           if(_ghost)
            _ghost.SetActive(false);

        }


        if (_ghost == null)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

     
        if(Physics.Raycast(ray,out hit,200f,(1<<12|1<<9)))
        {
            if(hit.collider != null)
            {
                PlaceableZone zone = hit.collider.gameObject.GetComponent<PlaceableZone>();
                if(zone != null)
                {
                    SetGhostRenerder(zone._owner);
                    _ghost.SetActive(true);
                    _ghost.transform.position = hit.point;


                    if(zone._owner==Define.WorldObject.Player)
                    {
                        if(Input.GetMouseButtonDown(0))
                        {

                            
                            Managers.Game.CardSpawn(Define.WorldObject.Player, _currentUnitdata?.FriendlyUnit, _ghost.transform);
                            
                        }

                    }
                }

                Debug.Log(hit.collider.name);
                if(hit.collider.gameObject.layer==1<<9)
                {
                    _ghost.SetActive(false);
                }
            }
            else
            {
                _ghost.SetActive(false);
                _currentUnder = Define.WorldObject.None;
            }

        }
    }

    void SetGhostRenerder(Define.WorldObject hitOwner)
    {
        if (_ghost == null) return;
        if (hitOwner == _currentUnder) return;
        if (_placeMaterials == null) return;
        SkinnedMeshRenderer[] renderers;
        
        _GhostsRenderers.TryGetValue(_ghost, out renderers);
        Material material= _placeMaterials.unkownMaterial;
        switch (hitOwner)
        {
            case Define.WorldObject.Unknown:
                material= _placeMaterials.unkownMaterial;
                break;
            case Define.WorldObject.Player:
                material = _placeMaterials.ableMaterial;

                break;
            case Define.WorldObject.Monster:
                material = _placeMaterials.unableMaterial;

                break;
            case Define.WorldObject.None:
                material = _placeMaterials.unkownMaterial;

                break;
        }
        if (renderers.Length>0)
        {
            for(int i=0;i<renderers.Length;i++)
            {
                renderers[i].material = material;
            }
        }

        _currentUnder = hitOwner;
    }

    public void SetGhost(UnitData ghost)
    {
        if (ghost == null) return;
        if (ghost.PlaceGhost == null) return;
        if(_ghost!=null)
        {
            _ghost.gameObject.SetActive(false);
            _ghost = null;
        }


        _currentUnitdata = ghost;
        GameObject temp;
        _placeGhosts.TryGetValue(ghost,out temp);
        
        if(temp!=null)
        {
            _ghost = temp;
            _ghost.SetActive(true);
            _ghost.transform.localScale = Vector3.one * 2;

            Managers.Game.OnPlaceEvent?.Invoke();
        }
        else
        {
            GameObject tempGhost = Instantiate(ghost.PlaceGhost, this.gameObject.transform);
            _placeGhosts.Add(ghost, tempGhost);
            SkinnedMeshRenderer[] _meshs = tempGhost.GetComponentsInChildren<SkinnedMeshRenderer>();
            _GhostsRenderers.Add(tempGhost, _meshs);
        }

    }

    public void UnSetGhost()
    {
        if(_ghost)
            _ghost.SetActive(false);

        Managers.Game.OffPlaceEvent?.Invoke();

        _ghost = null;
        _currentUnitdata = null;
    }
}
