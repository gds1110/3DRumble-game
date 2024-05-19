using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UI_Collect : UI_Scene
{
    enum CollectCardSlot
    {
        Slot1, Slot2, Slot3, Slot4, Slot5, Slot6, Slot7, Slot8,Slot9,Slot10,Slot11,Slot12,Slot13,Slot14,Slot15,Slot16,
        
    }
    enum HorizontalSlot
    {
        MyCard1, MyCard2, MyCard3, MyCard4, MyCard5, MyCard6, MyCard7
    }
   enum Btns
    {
        AllTap, FireTap, IceTap, EarthTap, WindTap, DarkTap, LightTap, NormalTap,
        Exit,Done,PrevBtn,NxtBtn,
    }
    enum Txts
    {
        PageNumTxt,
    }



    [SerializeField]
    List<UnitData> unitDatas = new List<UnitData>();


    [Header("Slot")]
    [SerializeField]
    List<UnitData> _showColletDatas = new List<UnitData>();
    [SerializeField]
    List<UI_Card> _slots = new List<UI_Card>();

    [Header("PlayerDeck")]
    [SerializeField]
    List<UnitData> _PlayerDeck = new List<UnitData>(); // 플레이어
    [SerializeField]
    List<UI_MiniCard> _playerSlots = new List<UI_MiniCard>();
   
    List<string> _playerNameDeck= new List<string> ();
    [Header("PageNum")]
    [SerializeField]
    int _currentPage = 0;
    [SerializeField]
    int _maxPageNum = 0;
    public int CurrentPage
    {
        get { return _currentPage; }
        set
        {
            _currentPage = value;

        }
    }
    private void Start()
    {
        Bind<UI_Card>(typeof(CollectCardSlot));
        Bind<UI_MiniCard>(typeof(HorizontalSlot));
        Bind<Button>(typeof(Btns));
        Bind<TextMeshProUGUI>(typeof(Txts));
        GetButton((int)Btns.PrevBtn)?.onClick.AddListener(PagePrev);
        GetButton((int)Btns.NxtBtn)?.onClick.AddListener(PageNext);

        int CollectCardNum = Enum.GetValues(typeof(CollectCardSlot)).Length;
        for(int i=0;i< CollectCardNum;i++)
        {
            _slots.Add(Get<UI_Card>(i));
        }
        int PlayerCardNum = Enum.GetValues(typeof(HorizontalSlot)).Length;
        for(int i=0;i< PlayerCardNum; i++)
        {
            _playerSlots.Add(Get<UI_MiniCard>(i));
        }

        GetTMP((int)Txts.PageNumTxt).text = "1";
        GetButton((int)Btns.AllTap)?.onClick.AddListener(delegate { ElementalTapButtonClick(Define.UnitElementalType.All); });
        GetButton((int)Btns.FireTap)?.onClick.AddListener(delegate { ElementalTapButtonClick(Define.UnitElementalType.Fire); });
        GetButton((int)Btns.IceTap)?.onClick.AddListener(delegate { ElementalTapButtonClick(Define.UnitElementalType.Ice); });
        GetButton((int)Btns.EarthTap)?.onClick.AddListener(delegate { ElementalTapButtonClick(Define.UnitElementalType.Earth); });
        GetButton((int)Btns.WindTap)?.onClick.AddListener(delegate { ElementalTapButtonClick(Define.UnitElementalType.Wind); });
        GetButton((int)Btns.DarkTap)?.onClick.AddListener(delegate { ElementalTapButtonClick(Define.UnitElementalType.Dark); });
        GetButton((int)Btns.LightTap)?.onClick.AddListener(delegate { ElementalTapButtonClick(Define.UnitElementalType.Light); });
        GetButton((int)Btns.NormalTap)?.onClick.AddListener(delegate { ElementalTapButtonClick(Define.UnitElementalType.Normal); });
        GetButton((int)Btns.Done)?.onClick.AddListener(SaveDeck);
        GetButton((int)Btns.Exit)?.onClick.AddListener(Exit);

        for (int i=0;i<_slots.Count;i++)
        {
            int temp = i;
            _slots[temp].gameObject.BindEvent(delegate  { AddToPlayerSlot(_slots[temp]); },Define.UIEvent.RightClick);
            _slots[temp].gameObject.BindEvent(delegate  { StartCoroutine(RotateCard(_slots[temp])); },Define.UIEvent.LeftClick);
        }
        for(int i=0;i<_playerSlots.Count;i++)
        {
            int temp = i;

            _playerSlots[temp].gameObject.BindEvent(delegate { SubstractPlayerSlot(_playerSlots[temp]); }, Define.UIEvent.RightClick);
        }

        LoadDeck();
    }
  
    void AddToPlayerSlot(UI_Card uiCard)
    {
        if (uiCard == null)
            return;
        if (uiCard.UnitData == null)
            return;
        for (int i = 0; i < _playerSlots.Count; i++)
        {
            if (_playerSlots[i].UnitData == uiCard.UnitData)
            {
                return;
            }
        }
        UI_MiniCard emptyCard = GetEmptySlot();
        if (emptyCard == null)
            return;
        emptyCard.UnitData = uiCard.UnitData;

    }
    void SubstractPlayerSlot(UI_MiniCard card)
    {
        if(card== null) return;
        if (card.UnitData == null) return;
        card.UnitData = null;
    }

    UI_MiniCard GetEmptySlot()
    {
        for (int i = 0; i < _playerSlots.Count; i++)
        {
            if (_playerSlots[i].UnitData == null)
            {
                return _playerSlots[i];
            }
        }
        return null;
    }

    void ElementalTapButtonClick(Define.UnitElementalType type)
    {
        _showColletDatas.Clear();
        foreach (UnitData data in unitDatas)
        {
            if (type == Define.UnitElementalType.All|| type == data._elementalType) 
            {
                _showColletDatas.Add(data);
            }
        }
        int CollectCardNum = Enum.GetValues(typeof(CollectCardSlot)).Length;
        _maxPageNum = (int)Mathf.Floor((_showColletDatas.Count - 1) / CollectCardNum);
        _currentPage = 0;

        ShowCardCollection();
    }

    void ShowCardCollection()
    {
        if(_showColletDatas.Count<=0)
        {
            return;
        }
        int CollectCardNum = Enum.GetValues(typeof(CollectCardSlot)).Length;
        int i = _currentPage * CollectCardNum;
        foreach(UI_Card card in _slots)
        {

            card.UnitData = null;
            if (i >= _showColletDatas.Count)
            {
                continue;
            }
            
            card.UnitData = _showColletDatas[i];
            i++;
          
        }
        GetTMP((int)Txts.PageNumTxt).text = $"{(_currentPage + 1)} / {_maxPageNum+1} ";


    }


    void PageNext()
    {

        Debug.Log("Next");
        _currentPage++;
        if (_currentPage > _maxPageNum)
        {
            _currentPage = 0;
        }
        GetTMP((int)Txts.PageNumTxt).text = $"{(_currentPage + 1)} / {_maxPageNum} ";
        ShowCardCollection();

    }
    void PagePrev()
    {
        Debug.Log("Prev");
        _currentPage--;
        if(_currentPage < 0 )
        {
            _currentPage = _maxPageNum;
        }
            GetTMP((int)Txts.PageNumTxt).text = $"{(_currentPage + 1)} / {_maxPageNum} ";
        ShowCardCollection();

    }
    public bool IsSaveDeck()
    {
        return Directory.Exists(Application.persistentDataPath + "/playerDeck");
    }

    public void SaveDeck()
    {

        for (int i = 0; i < _playerSlots.Count; i++)
        {
            _PlayerDeck.Add(_playerSlots[i].UnitData);

            if (_playerSlots[i].UnitData != null)
            {
                _playerNameDeck.Add(_playerSlots[i].UnitData.name);
            }
            else
            {
                _playerNameDeck.Add("Null");

            }
        }


        if(!IsSaveDeck())
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/playerDeck");
            Debug.Log(Application.persistentDataPath + "/playerDeck");
        }
        if (!Directory.Exists(Application.persistentDataPath + "/playerDeck/Deck_data"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/playerDeck/Deck_data");
        }
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerDeck/Deck_data/PlayerDeck_save.txt");
        Debug.Log(Application.persistentDataPath + "/playerDeck/Deck_data/");
        var json = JsonUtility.ToJson(_PlayerDeck);
        bf.Serialize(file, json);
        file.Close();

        PlayerDeck deck = new PlayerDeck();
        for(int i=0;i< _playerSlots.Count;i++)
        {
            deck.unitDatas.Add(_playerSlots[i].UnitData);
        }
        AssetDatabase.CreateAsset(deck, "Assets/Resources/DeckSo.asset");

        Managers.Scene.LoadScene(Define.Scene.Login);
    }
    public void Exit()
    {
        Managers.Scene.LoadScene(Define.Scene.Login);
    }

    public void LoadDeck()
    {

        PlayerDeck deck = Resources.Load("DeckSo", typeof(ScriptableObject)) as PlayerDeck;
        if (deck)
        {
            for (int i = 0; i < deck.unitDatas.Count; i++)
            {
                _playerSlots[i].UnitData = deck.unitDatas[i];

            }
        }
    }
    private IEnumerator RotateCard(UI_Card uI_Card)
    {
        yield return new WaitForSeconds(0.01f);
        if (uI_Card._isFlipping == false)
        {
            uI_Card._isFlipping = true;
            if (uI_Card.transform.rotation.y == 0f)
            {
                for (float i = 0; i <= 180f; i += 10f)
                {
                    uI_Card.transform.rotation = Quaternion.Euler(0f, i, 0f);
                    if (i == 90f)
                    {
                        uI_Card.FlipCard();
                    }
                    if(i==180f)
                    {
                        uI_Card._isFlipping = false;
                    }
                    yield return new WaitForSeconds(0.01f);
                }
            }
            else
            {
                for (float i = 180f; i >= 0; i -= 10f)
                {
                    uI_Card.transform.rotation = Quaternion.Euler(0f, i, 0f);
                    if (i == 90f)
                    {
                        uI_Card.FlipCard();
                    }
                    if (i == 180f)
                    {
                        uI_Card._isFlipping = false;
                    }
                    yield return new WaitForSeconds(0.01f);

                }
            }
        }
        else
        {
            yield return new WaitForSeconds(0.01f);
        }
    }

    public override void Init()
    {
        base.Init();

    }

}
