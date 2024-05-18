using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class UI_MsgPopup : UI_Popup
{
   enum Texts
    {

        PopupMsg,
    }
    enum Btns
    {
        CloseBtn
    }
    private void Start()
    {
     

    }

    public void SetText(string text)
    {
        Get<TextMeshProUGUI>((int)Btns.CloseBtn).text = text;
    }

    public override void Init()
    {
        base.Init();
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Btns));

        Get<Button>((int)Btns.CloseBtn).onClick.AddListener(ClosePopupUI);
    }

    public override void ClosePopupUI()
    {
        base.ClosePopupUI();
    }

}
