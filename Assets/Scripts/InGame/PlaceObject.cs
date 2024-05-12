using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.EventSystems;

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
            }
        }
        SetGhost(_unitDatas[0]);
    }

    // Update is called once per frame
    void Update()
    {
        if (_ghost == null)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("UI Hit");
            _ghost.SetActive(false);
            return;
        }
        //else
        //{
        //   if(_ghost!=null)
        //    {
        //        _ghost.SetActive(true);
        //    }
        //}

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
                            Instantiate(_currentUnitdata?.FriendlyUnit, _ghost.transform.position, _ghost.transform.rotation);
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

    void SetGhost(UnitData ghost)
    {
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
        }
        else
        {
            GameObject tempGhost = Instantiate(ghost.PlaceGhost, this.gameObject.transform);
            _placeGhosts.Add(ghost, tempGhost);
            SkinnedMeshRenderer[] _meshs = tempGhost.GetComponentsInChildren<SkinnedMeshRenderer>();
            _GhostsRenderers.Add(tempGhost, _meshs);
        }

    }
}
