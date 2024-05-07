using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ElementalMat", menuName = "Scriptable Object/ElementalMat", order = int.MaxValue)]
public class ElementalCalculate : ScriptableObject
{

    static readonly Dictionary<(Define.UnitElementalType, Define.UnitElementalType), float> ElementalMatrix = new Dictionary<(Define.UnitElementalType, Define.UnitElementalType), float>
    {
        {(Define.UnitElementalType.Normal,Define.UnitElementalType.Normal),1f},
        {(Define.UnitElementalType.Normal,Define.UnitElementalType.Ice),1f},
        {(Define.UnitElementalType.Normal,Define.UnitElementalType.Wind),1f},
        {(Define.UnitElementalType.Normal,Define.UnitElementalType.Light),1f},
        {(Define.UnitElementalType.Normal,Define.UnitElementalType.Earth),1f},
        {(Define.UnitElementalType.Normal,Define.UnitElementalType.Fire),1f},
        {(Define.UnitElementalType.Normal,Define.UnitElementalType.Dark),1f},

        {(Define.UnitElementalType.Ice,Define.UnitElementalType.Normal),1f},
        {(Define.UnitElementalType.Ice,Define.UnitElementalType.Ice),1f},
        {(Define.UnitElementalType.Ice,Define.UnitElementalType.Wind),1f},
        {(Define.UnitElementalType.Ice,Define.UnitElementalType.Light),1f},
        {(Define.UnitElementalType.Ice,Define.UnitElementalType.Earth),1f},
        {(Define.UnitElementalType.Ice,Define.UnitElementalType.Fire),1.5f},
        {(Define.UnitElementalType.Ice,Define.UnitElementalType.Dark),1f},

        {(Define.UnitElementalType.Wind,Define.UnitElementalType.Normal),1.5f},
        {(Define.UnitElementalType.Wind,Define.UnitElementalType.Ice),1f},
        {(Define.UnitElementalType.Wind,Define.UnitElementalType.Wind),1f},
        {(Define.UnitElementalType.Wind,Define.UnitElementalType.Light),1f},
        {(Define.UnitElementalType.Wind,Define.UnitElementalType.Earth),1f},
        {(Define.UnitElementalType.Wind,Define.UnitElementalType.Fire),1f},
        {(Define.UnitElementalType.Wind,Define.UnitElementalType.Dark),1f},

        {(Define.UnitElementalType.Light,Define.UnitElementalType.Normal),1f},
        {(Define.UnitElementalType.Light,Define.UnitElementalType.Ice),1f},
        {(Define.UnitElementalType.Light,Define.UnitElementalType.Wind),1f},
        {(Define.UnitElementalType.Light,Define.UnitElementalType.Light),1f},
        {(Define.UnitElementalType.Light,Define.UnitElementalType.Earth),1f},
        {(Define.UnitElementalType.Light,Define.UnitElementalType.Fire),1f},
        {(Define.UnitElementalType.Light,Define.UnitElementalType.Dark),1.5f},

        {(Define.UnitElementalType.Earth,Define.UnitElementalType.Normal),1f},
        {(Define.UnitElementalType.Earth,Define.UnitElementalType.Ice),1f},
        {(Define.UnitElementalType.Earth,Define.UnitElementalType.Wind),1.5f},
        {(Define.UnitElementalType.Earth,Define.UnitElementalType.Light),1f},
        {(Define.UnitElementalType.Earth,Define.UnitElementalType.Earth),1f},
        {(Define.UnitElementalType.Earth,Define.UnitElementalType.Fire),1f},
        {(Define.UnitElementalType.Earth,Define.UnitElementalType.Dark),1f},

        {(Define.UnitElementalType.Fire,Define.UnitElementalType.Normal),1f},
        {(Define.UnitElementalType.Fire,Define.UnitElementalType.Ice),1f},
        {(Define.UnitElementalType.Fire,Define.UnitElementalType.Wind),1f},
        {(Define.UnitElementalType.Fire,Define.UnitElementalType.Light),1.5f},
        {(Define.UnitElementalType.Fire,Define.UnitElementalType.Earth),1f},
        {(Define.UnitElementalType.Fire,Define.UnitElementalType.Fire),1f},
        {(Define.UnitElementalType.Fire,Define.UnitElementalType.Dark),1f},

        {(Define.UnitElementalType.Dark,Define.UnitElementalType.Normal),1f},
        {(Define.UnitElementalType.Dark,Define.UnitElementalType.Ice),1f},
        {(Define.UnitElementalType.Dark,Define.UnitElementalType.Wind),1f},
        {(Define.UnitElementalType.Dark,Define.UnitElementalType.Light),1f},
        {(Define.UnitElementalType.Dark,Define.UnitElementalType.Earth),1.5f},
        {(Define.UnitElementalType.Dark,Define.UnitElementalType.Fire),1f},
        {(Define.UnitElementalType.Dark,Define.UnitElementalType.Dark),1f},
        

    };

    public static float GetMultiplier(Define.UnitElementalType attacker, Define.UnitElementalType defender)
    {
        if(ElementalMatrix.TryGetValue((attacker,defender),out float multiplier))
         {
            return multiplier;
        }

        return 1.0f;
    }

}
