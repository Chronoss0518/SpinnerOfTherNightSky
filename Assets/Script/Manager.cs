using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Manager
{
    public const bool IS_MOCK = true;
    public const int MAX_GMAE_PLAYER = 4;
    private Manager() { }

    public static Manager ins { get; private set; } = new Manager();

    public enum GameType
    {
        Normal,
        Pocket,
    }

    public enum MemberType
    {
        CPU,
        NetWorkPlayer,
        None
    }

    public GameType gameType = GameType.Normal;

    public MemberType[] cpuFlgs = 
    { 
        MemberType.CPU,
        MemberType.None,
        MemberType.None
    };

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
