using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class UI_MainCardManager : UI_Base
{
    enum CardSlots
    {
        Slot1,Slot2,Slot3,Slot4,Slot5
    }
    enum ManaSlide
    {
        ManaSlide
    }
    enum Texts
    {
        ManagageText
    }

    [SerializeField]
    PlaceObject _placeObject;

    [SerializeField]
    PlayerDeck _PlayerDeck ; // 플레이어

    [SerializeField]
    List<UI_Card> _playerSlots = new List<UI_Card>();

    [SerializeField]
    UI_Card _currentCard;

    public float _manaGage=0;
    public int _maxManaGage=10;
    public int _costText = 0;
    public TextMeshProUGUI _manaText;
    public Slider _manaSlider;

    // Start is called before the first frame update
    void Start()
    {
        Bind<UI_Card>(typeof(CardSlots));
        Bind<Slider>(typeof(ManaSlide));
        Bind<TextMeshProUGUI>(typeof(Texts));
        _placeObject = GetComponent<PlaceObject>();
        _manaText =Get<TextMeshProUGUI>((int)Texts.ManagageText);
        _manaSlider = Get<Slider>((int)ManaSlide.ManaSlide);
        _manaText.text = "0";


        int PlayerCardNum = Enum.GetValues(typeof(CardSlots)).Length;
        for (int i = 0; i < PlayerCardNum; i++)
        {
            _playerSlots.Add(Get<UI_Card>(i));
        }
        LoadDeck();

        for(int i = 0;i < _playerSlots.Count;i++)
        {
            _playerSlots[i].UnitData=GetRandomUnitdata();
        }
        for (int i = 0; i < _playerSlots.Count; i++)
        {
            int temp = i;
            _playerSlots[temp].gameObject.BindEvent(delegate { SelectCard(_playerSlots[temp]); }, Define.UIEvent.LeftClick);
        }

        Managers.Game.SpawnCardEvent -= SpawnCard;
        Managers.Game.SpawnCardEvent += SpawnCard;

    }

    // Update is called once per frame
    void Update()
    {

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        //Mana
        ManaGen();
        ManaGageUpdate();

        //Card
        for (int i = 0; i < _playerSlots.Count;i++)
        {
            if (_playerSlots[i].UnitData!=null)
            {
                _playerSlots[i].Refresh((int)_manaGage);
            }
        }

    }
    public void ManaGen()
    {
        _manaGage += Time.deltaTime*0.5f;
        if (_manaGage > _maxManaGage)
        {
            _manaGage = _maxManaGage;
        }
        
    }

    public void ManaGageUpdate()
    {
        float ratio = _manaGage / _maxManaGage;
        _manaSlider.value = ratio;
        int textmana = Mathf.FloorToInt(_manaGage);
      
            _manaText.text = textmana.ToString();
      
    }

    public override void Init()
    {

    }
    public void LoadDeck()
    {

       _PlayerDeck = Resources.Load("DeckSo", typeof(ScriptableObject)) as PlayerDeck;
        //if (deck)
        //{
        //    for (int i = 0; i < deck.unitDatas.Length; i++)
        //    {
        //        _playerSlots[i].UnitData = deck.unitDatas[i];

        //    }
        //}
    }

    UnitData GetRandomUnitdata()
    {
        int randomNum = 0;
        if(_PlayerDeck.unitDatas.Count>0)
        {
            randomNum = UnityEngine.Random.Range(0, _PlayerDeck.unitDatas.Count);

        }
        return _PlayerDeck.unitDatas[randomNum];
    }
    IEnumerator CoManaRegen()
    {
        yield return new WaitForSeconds(1f);
        _manaGage++;
        if (_manaGage > _maxManaGage)
        {
            _manaGage = _maxManaGage;
        }
        _manaText.text = _manaGage.ToString();
    }

    void SelectCard(UI_Card selected)
    {
        if(selected.UnitData.cost>_manaGage)
        {
            return;
        }

        if(_currentCard)
        {
            _currentCard.UnSelect();
        }
        _currentCard=selected;
        selected.IsSelect();
        _placeObject.UnSetGhost();
        _placeObject.SetGhost(selected.UnitData);
           
        
    }

    void SpawnCard()
    {
        if (_manaGage < _currentCard.UnitData.cost) return;
        if (!_currentCard) return;
        
        _manaGage-=_currentCard.UnitData.cost;  
        _currentCard.UnSelect();
        _currentCard.UnitData = GetRandomUnitdata();
        _currentCard = null;
        
        
    }
}
