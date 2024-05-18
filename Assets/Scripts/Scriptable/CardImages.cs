using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CardImages", menuName = "Scriptable Object/CardData", order = int.MaxValue)]
public class CardImages : ScriptableObject
{
    [Header ("ElementalType")]
    public Sprite Fire;
    public Sprite Ice;
    public Sprite Earth;
    public Sprite Dark;
    public Sprite Light;
    public Sprite Wind;
    public Sprite Normal;
    [Header("AttackType")]
    public Sprite Meele;
    public Sprite Arange;
    public Sprite Both;

    public Sprite DefaultImg;
    public Sprite DefaultMiniImg;
}
