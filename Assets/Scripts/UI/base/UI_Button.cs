using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Button : UI_Popup
{
    [SerializeField]
    Text _text;

    enum Buttons
    {
        PointButton,
    }

    enum Texts
    {
        PointText,
        ScoreText
    }
    enum GameObjects
    {
        TestObject,
    }

    enum Images
    {
        ItemIcon,
        Blocker
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));
        Get<Text>((int)Texts.ScoreText).text = "Bind Text";

        //GameObject go = GetImage((int)Images.ItemIcon).gameObject;
        //BindEvent(go, (PointerEventData data) => { go.transform.position = data.position; }, Define.UIEvent.Drag);


        GetButton((int)Buttons.PointButton).gameObject.BindEvent(OnButtonClicked);
    }

    int _score = 0;

    public void OnButtonClicked(PointerEventData data)
    {
        _score++;
       GetText((int)Texts.ScoreText).text = $"���� : {_score}";
    }
}
