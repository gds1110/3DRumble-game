using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LoginScene : UI_Scene
{

    enum Btns
    {
        PlayBtn, DeckBtn, ExitBtn,
    }



    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Btns));

        Get<Button>((int)Btns.DeckBtn).onClick.AddListener(GoDeckBuildScene);
        Get<Button>((int)Btns.ExitBtn).onClick.AddListener(ExitBtn);
        Get<Button>((int)Btns.PlayBtn).onClick.AddListener(GoPlayScene);
    }
    public void GoPlayScene()
    {
        PlayerDeck deck = Resources.Load("DeckSo", typeof(ScriptableObject)) as PlayerDeck;
        bool isData = true;
        if (deck != null)
        {
            foreach (var item in deck.unitDatas)
            {
                if (item == null)
                {
                    isData = false;
                }

            }
        }
        if (!deck) isData = false;

        if (isData) { Managers.Scene.LoadScene(Define.Scene.Game); }
        else
        {
            var t = Managers.UI.ShowPopupUI<UI_MsgPopup>("MsgPopup");
            t.Init();
            t.SetText("Need DeckBuilding");
        }
    }
    public void GoDeckBuildScene()
    {
        Managers.Scene.LoadScene(Define.Scene.Deck);
    }
    public void ExitBtn()
    {
        Application.Quit();
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
