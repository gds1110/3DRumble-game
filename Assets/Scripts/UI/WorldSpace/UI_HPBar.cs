using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HPBar : UI_Base
{

    enum GameObjects
    {
        HPBar
    }
    BaseCombat _stat;

  
    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        _stat = transform.parent.GetComponent<BaseCombat>();
    }

    private void Update()
    {
        Transform parent = transform.parent;
        transform.position = parent.position + Vector3.up * (parent.GetComponent<Collider>().bounds.size.y+1);
        transform.rotation = Camera.main.transform.rotation;
        //transform.LookAt(Camera.main.transform);

        //if (_stat != null)
        //{
        //    //float ratio = _stat._l / (float)_stat.MaxHp;
        //    //   SetHpRatio(ratio);
        //    float ratio = _stat._currentLife / _stat._life;
        //    SetHpRatio(ratio);
          
        //}

        
    }

    public void SetHpRatio(float ratio)
    {
        GetObject((int)GameObjects.HPBar).GetComponent<Slider>().value =ratio;
    }
}
