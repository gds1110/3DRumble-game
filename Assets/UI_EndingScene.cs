using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_EndingScene : UI_Scene
{
    enum Texts
    {

        EndingTxt,
    }
    enum Btns
    {
        ExitBtn
    }
    public override void Init()
    {
        base.Init();
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Btns));

        Get<Button>((int)Btns.ExitBtn).onClick.AddListener(GoLogin);
    }
    public void IsWinOrDefeat(bool  isWin)
    {
        string inText = "Default";
        switch (isWin)
        {
            case true:
                inText = "WIN!";
                break;

                case false:
                inText = "LOSE";
                break;
        }
        Get<TextMeshProUGUI>((int)Texts.EndingTxt).text = inText;

    }


    void GoLogin()
    {
        Managers.Scene.LoadScene(Define.Scene.Login);
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
