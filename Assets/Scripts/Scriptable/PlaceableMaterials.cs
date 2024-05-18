using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlaceableMats", menuName = "Scriptable Object/Materials", order = int.MaxValue)]

public class PlaceableMaterials : ScriptableObject
{
    public Material playerMaterial;
    public Material monsterMaterial;
    public Material unkownMaterial;

    public Material ableMaterial;
    public Material unableMaterial;
}
