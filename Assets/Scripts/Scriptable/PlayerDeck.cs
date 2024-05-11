using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Purchasing.MiniJSON;

[CreateAssetMenu(fileName = "PlayerDeck", menuName = "Scriptable Object/PlayerDeck", order = int.MaxValue)]

[SerializeField]
public class PlayerDeck : ScriptableObject
{
    [SerializeField]
    public UnitData[] unitDatas = new UnitData[5];

    public void Save()
    {
        var json = JsonUtility.ToJson(unitDatas);
        // Write to disk
    }

    public void Load()
    {
        // Read from disk
       // JsonUtility.FromJsonOverwrite(json, unitDatas);
    }
}
