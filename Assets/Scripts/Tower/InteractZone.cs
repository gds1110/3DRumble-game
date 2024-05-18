using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InteractZone : UI_Base
{
    enum UIObjects
    {
        CircleSlider,
        CircleFill,
        FillBg,
    }
    public Define.WorldObject _owner = Define.WorldObject.Unknown;
    public bool _isConquered = false;
    public bool _isConquering = false;
    public float _conquerTime=5f;
    float _currentConquerTime = 0;
    public Controller _conquerUnit;
    public TowerController _ownerTower;
    public UnityEvent<Define.WorldObject> ConquerEvent;


    Slider _conquerSlider;
    Image _fillBg;
    Image _CircleFill;

    Color _playerColor = Color.green;
    Color _enemyColor = Color.red;
    Color _noneColor = Color.black;

    float _timer;

    // Start is called before the first frame update
    void Start()
    {
        Bind<GameObject>(typeof(UIObjects));
        _conquerSlider= Get<GameObject>((int)UIObjects.CircleSlider).GetComponent<Slider>();
        _fillBg= Get<GameObject>((int) UIObjects.FillBg).GetComponent<Image>();
        _CircleFill = Get<GameObject>((int) UIObjects.CircleFill).GetComponent<Image>();
        _ownerTower = GetComponentInParent<TowerController>();
        _owner = _ownerTower._owner;

        _fillBg.color = _noneColor;
       

        //Managers.Game.OnPlaceEvent -= OnPlace;
        //Managers.Game.OnPlaceEvent += OnPlace;
        //Managers.Game.OffPlaceEvent -= OffPlace;
        //Managers.Game.OffPlaceEvent += OffPlace;

       // OffPlace();
    }

    void OnPlace()
    {
        gameObject.SetActive(true);
    }
    void OffPlace()
    {
        gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        _conquerSlider.value = _currentConquerTime / _conquerTime;
        if (!_isConquering || !_conquerUnit)
        {
            if (_currentConquerTime > 0)
            {
                _currentConquerTime -= Time.deltaTime;
            }
        }

    }

    private void FixedUpdate()
    {
        //if(_isConquering == false&&_currentConquerTime>0)
        //{

        //}

        //if (_isConquered == false) return;
        //if(_conquerUnit== null) return; 
   

    }

    public void StartConquer(Controller conquerUnit)
    {
        switch (conquerUnit._owner)
        {
            case Define.WorldObject.Unknown:
                _CircleFill.color = _noneColor;
                break;
            case Define.WorldObject.Player:
                _CircleFill.color = _playerColor;
                break;
            case Define.WorldObject.Monster:
                _CircleFill.color = _enemyColor;
                break;
            case Define.WorldObject.None:
                _CircleFill.color = _noneColor;
                break;
        }
        _isConquering = true;
        

    }

    private void OnTriggerEnter(Collider other)
    {

        if (_conquerUnit) return;
        Controller controller = other.GetComponent<Controller>();
        if(!controller) return;
        if (controller._unit._movementType == Define.MovemnetType.Aerial) return;
        if(controller._owner==_owner) return;
        if (other.GetComponentInParent<IConquerAble>() != null)
           {

            _conquerUnit = controller;
              other.GetComponent<IConquerAble>().StartConquer(_ownerTower);
              StartConquer(other.GetComponent<Controller>());
           }
        
    }


    public void StopConquer()
    {
        _isConquering = false;
        if (_owner != Define.WorldObject.None || _owner != Define.WorldObject.Unknown) _isConquered = false;
    }
    public void EndConquer(Controller controller)
    {
        controller.GetComponent<IConquerAble>().EndConquer(_ownerTower);
        _ownerTower.EndConquer(controller);
        _isConquering = false;
        ConquerEvent?.Invoke(_owner);
        if(_owner!=Define.WorldObject.Unknown||_owner!=Define.WorldObject.None) _isConquered = true;
        else { _isConquered = false; }
    }
    public void Conquering(Controller controller)
    {

        _currentConquerTime += Time.deltaTime;
        if(_currentConquerTime > _conquerTime)
        {
            if(_owner==Define.WorldObject.Unknown||_owner==Define.WorldObject.None)
            {
                _owner = controller._owner;
                _ownerTower.gameObject.tag = controller.tag;
             
            }
            else if(_owner!=Define.WorldObject.Unknown&&_owner!=controller._owner)
            {
                _owner=Define.WorldObject.Unknown;
                _ownerTower.gameObject.tag = "None";

            }
            EndConquer(controller);
        
            switch (_owner)
            {
                case Define.WorldObject.Unknown:
                    _fillBg.color = _noneColor;
                    break;
                case Define.WorldObject.Player:
                    _fillBg.color = _playerColor;
                    break;
                case Define.WorldObject.Monster:
                    _fillBg.color = _enemyColor;
                    break;
                case Define.WorldObject.None:
                    _fillBg.color = _noneColor;
                    break;
            }
            _currentConquerTime = 0;
        }

    }
    public void DoneConquer(Controller controller)
    {
        
    }

    public override void Init()
    {

        Bind<GameObject>(typeof(UIObjects));
    }
}
