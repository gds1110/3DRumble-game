using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Collect : UI_Scene
{
    enum GameObjects
    {
        Slot1, Slot2, Slot3, Slot4, Slot5, Slot6, Slot7, Slot8,
        
        MyCard1, MyCard2, MyCard3, MyCard4, MyCard5,
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
    [SerializeField]
    List<UnitData> ShowColletDatas = new List<UnitData>();
    [SerializeField]
    List<UnitData> ShowActiveCards = new List<UnitData>();


    private void Start()
    {
        Bind<GameObject>(typeof(GameObjects));
        Bind<Button>(typeof(Btns));
        Bind<TextMeshPro>(typeof(Txts));
        GetButton((int)Btns.PrevBtn)?.onClick.AddListener(PagePrev);
        GetButton((int)Btns.NxtBtn)?.onClick.AddListener(PageNext);

      //  GetButton((int)Btns.AllTap).gameObject.BindEvent(OnButtonClicked);
    }

    void ElementalTapButtonClick(Define.UnitElementalType type)
    {
        
    }

    void PageNext()
    {
        Debug.Log("Next");

    }
    void PagePrev()
    {
        Debug.Log("Prev");

    }

    public override void Init()
    {
        base.Init();

    }

}
