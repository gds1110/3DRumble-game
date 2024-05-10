using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MiniCard : UI_Base
{

    enum GameObjects
    {
        NameText,
        CharacterImage,
        CostText,
    }


    public TMP_Text _name;
    public TMP_Text _cost;
    public Image _characterImg;


    [SerializeField]
    UnitData _unitData;
    [SerializeField]
    CardImages _CardImageSO;
    UnitData UnitData { get { return _unitData; } set { _unitData = value; } }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));

        _name = Get<GameObject>((int)GameObjects.NameText).GetComponent<TMP_Text>();
        _cost = Get<GameObject>((int)GameObjects.CostText).GetComponent<TMP_Text>();
        _characterImg = Get<GameObject>((int)GameObjects.CharacterImage).GetComponent<Image>();

        SetInfo();
    }

    void SetInfo()
    {
        if(_unitData != null)
        {
            _name.text = _unitData.Name;
            _cost.text = _unitData.cost.ToString();
            _characterImg.sprite = _unitData.UnitPortrait;

        }
        else
        {
            _name.text = "ºó ½½·Ô";
            _cost.text = "0";
            _characterImg.sprite = _CardImageSO.DefaultMiniImg;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
