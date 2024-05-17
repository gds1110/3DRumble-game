using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingScene : BaseScene
{
    public override void Clear()
    {
        
    }
   public  UI_EndingScene UI_EndingScene = null;
    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.EndingScene;
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
        if(UI_EndingScene != null)
        {
            UI_EndingScene.Init();
            UI_EndingScene.IsWinOrDefeat(Managers.Game.GameWin);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
