using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager 
{
 
    public void PlayEffect(GameObject hitEffect, Vector3 pos,Vector3 normal, Transform parent =null)
    {
        GameObject effect = Object.Instantiate(hitEffect, pos,Quaternion.LookRotation(normal));
        //effect.transform.LookAt(pos - bullet.transform.position);
        //GameObject effect = Object.Instantiate(bullet._explosionEffect,pos,Quaternion.identity);
        effect.transform.rotation = Camera.main.transform.rotation;
        effect.SetActive(true);
        if(parent != null)
        {
            effect.transform.parent = parent;  
        }
        Poolable poolable = effect.GetComponent<Poolable>();
        if(poolable != null)
        {
           // Managers.Pool.PopAutoPush(poolable);
        }
        Object.Destroy(effect,1f);
    }
}
