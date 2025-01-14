using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Manager
{
    public const bool IS_MOCK = true;

    private Manager() { }

    public static Manager ins { get; private set; } = new Manager();

    public enum GameType
    {
        Normal,
        Pocket,
    }

    public enum MemberType
    {
        Mach = 2,
        Survival3,
        Survival4,
    }

    public GameType gameType = GameType.Normal;

    public MemberType memberNum = MemberType.Mach;

    public bool[] cpuFlgs = { true, true, true };

    public int useBookNo = 0;

    public enum DisplayAspectType
    {
        //c‰¡‚ª“¯‚¶”ä—¦//
        None,
        //c‚ª‰¡‚æ‚è‘å‚«‚¢//
        VerticalScreen,
        //‰¡‚ª‚æ‚è‘å‚«‚¢//
        LandscapeScreen
    }

    public DisplayAspectType aspectType = DisplayAspectType.None;

}
