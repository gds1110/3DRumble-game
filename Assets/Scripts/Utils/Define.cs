using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define 
{
    public enum WorldObject
    {
        Unknown,
        Player,
        Monster,
        None
    }

    public enum Layer
    {
        Monster=6,
        Ground=3
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }

    public enum Scene
    {
        UnKnown,
        Login,
        Lobby,
        Game,
        EndingScene

    }
    public enum UIEvent
    {
        LeftClick,
        RightClick,
        Drag,
    }

    public enum MouseEvent
    {
        Press,
        Click
    }

    public enum CmeraMode
    {
        QuarterView,
        ThirdPersonView
    }

    public enum EffectType
    {
        Default,
    }
    public enum AttackType
    {
        Meele,
        Arange,
        Both
    }
    public enum MovemnetType
    {
        Ground,
        Aerial,
        Both
    }
 
    // Unit or Building or Both
    public enum TargetType
    {
        Unit,
        Building,
        Both
    }
    public enum State
    {
        Die,
        Moving,
        Idle,
        Attack
    }
    public enum UnitGrade
    {
        Normal,
        Rare,
        Unique,
        Legend
    }
    public enum UnitElementalType
    {
        Fire,
        Ice,
        Earth,
        Wind,
        Dark,
        Light,
        Normal,
        All
    }
}
