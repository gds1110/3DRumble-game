using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_Card : UI_Popup
{
    enum GameObjects
    {
        NameText,
        AttackTypeImg,
        ElementalTypeImg,
        CharacterImg,
        CostText,
        BackNameText,
        BElementalText,
        BAttackTypeText,
        BMovementTypeText,
        BTargetTypeText,
        BTargetMovemnetTypeText,
        BLifeText,
        BDamageText,
        BAttackSpdText,
        BSpdText,
    }
  

    public TMP_Text _name;
    public TMP_Text _cost;

    public Image _attackType;
    public Image _elementalType;
    public Image _characterImg;


    public TMP_Text _bName;
    public TMP_Text _bElemental;
    public TMP_Text _bMovemnet;
    public TMP_Text _bAttackType;
    public TMP_Text _bTargetType;
    public TMP_Text _bTargetMovementType;
    public TMP_Text _bLife;
    public TMP_Text _bDamage;
    public TMP_Text _bAttackSpd;
    public TMP_Text _bSpd;



    [SerializeField]
    UnitData _unitData;
    [SerializeField]
    CardImages _CardImageSO;
UnitData UnitData { get { return _unitData; } set { _unitData = value; } }

    // Start is called before the first frame update
    void Start()
    {
        Bind<GameObject>(typeof(GameObjects));

        _name = Get<GameObject>((int)GameObjects.NameText).GetComponent<TMP_Text>();
        _cost = Get<GameObject>((int)GameObjects.CostText).GetComponent<TMP_Text>();
        _attackType= Get<GameObject>((int)GameObjects.AttackTypeImg).GetComponent<Image>();
        _elementalType = Get<GameObject>((int)GameObjects.ElementalTypeImg).GetComponent<Image>();
        _characterImg = Get<GameObject>((int)GameObjects.CharacterImg).GetComponent<Image>();
        _bName = Get<GameObject>((int)GameObjects.BackNameText).GetComponent<TMP_Text>();
        _bElemental = Get<GameObject>((int)GameObjects.BElementalText).GetComponent<TMP_Text>();
        _bMovemnet = Get<GameObject>((int)GameObjects.BMovementTypeText).GetComponent<TMP_Text>();
        _bAttackType = Get<GameObject>((int)GameObjects.BAttackTypeText).GetComponent<TMP_Text>(); ;
        _bTargetType = Get<GameObject>((int)GameObjects.BTargetTypeText).GetComponent<TMP_Text>();
        _bTargetMovementType = Get<GameObject>((int)GameObjects.BTargetMovemnetTypeText).GetComponent<TMP_Text>();
        _bLife = Get<GameObject>((int)GameObjects.BLifeText).GetComponent<TMP_Text>();
        _bDamage = Get<GameObject>((int)GameObjects.BDamageText).GetComponent<TMP_Text>();
        _bAttackSpd = Get<GameObject>((int)GameObjects.BAttackSpdText).GetComponent<TMP_Text>();
        _bSpd = Get<GameObject>((int)GameObjects.BSpdText).GetComponent<TMP_Text>();
        SetCardInfo();
    }
    void SetCardInfo()
    {
        if (_unitData != null)
        {
            _name.text = _unitData.DisplayName;
            _cost.text = _unitData.cost.ToString();
            _characterImg.sprite = _unitData.UnitPortrait;
            if (_CardImageSO != null)
            {
                string belemental= "";
                switch (_unitData._elementalType)
                {
                    case Define.UnitElementalType.Fire:
                        _elementalType.sprite = _CardImageSO.Fire;
                        belemental = "ȭ��";
                        break;
                    case Define.UnitElementalType.Ice:
                        _elementalType.sprite = _CardImageSO.Ice;
                        belemental = "����";

                        break;
                    case Define.UnitElementalType.Earth:
                        _elementalType.sprite = _CardImageSO.Earth; 
                        belemental = "����";

                        break;
                    case Define.UnitElementalType.Wind:
                        _elementalType.sprite = _CardImageSO.Wind;
                        belemental = "�ٶ�";

                        break;
                    case Define.UnitElementalType.Dark:
                        _elementalType.sprite = _CardImageSO.Dark;
                        belemental = "���";

                        break;
                    case Define.UnitElementalType.Light:
                        _elementalType.sprite = _CardImageSO.Light;
                        belemental = "��";

                        break;
                    case Define.UnitElementalType.Normal:
                        _elementalType.sprite = _CardImageSO.Normal;
                        belemental = "�븻";

                        break;
                }
                string battacktype = "";
                switch (_unitData._attackType)
                {
                    case Define.AttackType.Meele:
                        _attackType.sprite = _CardImageSO.Meele;
                        battacktype = "����";
                        break;
                    case Define.AttackType.Arange:
                        _attackType.sprite = _CardImageSO.Arange;
                        battacktype = "���Ÿ�";
                        break;
                    case Define.AttackType.Both:
                        _attackType.sprite = _CardImageSO.Both;
                        battacktype = "��Ƽ";

                        break;
                }

                string bmovemnt = "";
                string btargettype = "";
                string btargetmovement = "";
                switch (_unitData._movementType)
                {
                    case Define.MovemnetType.Ground:
                        bmovemnt = "����";
                        break;
                    case Define.MovemnetType.Aerial:
                        bmovemnt = "����";

                        break;
                    case Define.MovemnetType.Both:
                        bmovemnt = "��Ƽ";

                        break;
                }
                switch (_unitData._targetType)
                {
                    case Define.TargetType.Unit:
                        btargettype = "����";
                        break;
                    case Define.TargetType.Building:
                        btargettype = "�ǹ�";

                        break;
                    case Define.TargetType.Both:
                        btargettype = "��Ƽ";

                        break;
                }
                switch (_unitData._targetMovementType)
                {
                    case Define.MovemnetType.Ground:
                        btargetmovement = "����";
                        break;
                    case Define.MovemnetType.Aerial:
                        btargetmovement = "����";

                        break;
                    case Define.MovemnetType.Both:
                        btargetmovement = "��Ƽ";

                        break;
                }




                _bElemental.text = "�Ӽ� : "+ belemental;
               
                _bMovemnet.text ="�̵�Ÿ�� : "+bmovemnt;
                _bAttackType.text ="����Ÿ�� : "+ battacktype;
                _bTargetType.text ="Ÿ��Ÿ�� : "+btargettype;
                _bTargetMovementType.text ="Ÿ���̵�Ÿ�� : "+btargetmovement;
                _bLife.text = "ü�� : " + _unitData._life.ToString();
                _bDamage.text = "���ݷ� : " + _unitData._damage.ToString();
                _bAttackSpd.text ="���ݼӵ� : " + _unitData._attackRatio.ToString();
                _bSpd.text ="�̵��ӵ� : " + _unitData._speed.ToString();

            }



        }
        else
        {
            _name.text = "�� ����";
            _cost.text = "0";
            _characterImg.sprite = _CardImageSO.DefaultImg;

            _bElemental.text = "�Ӽ� : �� ����";

            _bMovemnet.text = "�̵�Ÿ�� : �� ����";
            _bAttackType.text = "����Ÿ�� : �� ����";
            _bTargetType.text = "Ÿ��Ÿ�� : �� ����";
            _bTargetMovementType.text = "Ÿ���̵�Ÿ�� : �� ����";
            _bLife.text = "ü�� : 0";
            _bDamage.text = "���ݷ� : 0";
            _bAttackSpd.text = "���ݼӵ� : 0";
            _bSpd.text = "�̵��ӵ� : 0";
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
