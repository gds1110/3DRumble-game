using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class LoginScene : BaseScene
{

   
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Login;

    }

    public override void Clear()
    {
    }


    // Start is called before the first frame update
    void Start()
    {


        Init();
        
      //var t =   Managers.UI.ShowPopupUI<UI_MsgPopup>("MsgPopup");
      //  t.Init();
      //   t.SetText("Test Message");
    }
    private void Update()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

}
