using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;


[System.Serializable]
public class Manager
{
    [SerializeField]
    bool IS_DEBUG_FLG = true;
    public bool IS_DEBUG { get { return IS_DEBUG_FLG; } }
    
    [SerializeField]

    bool IS_MOCK_FLG = true;
    public bool IS_MOCK { get { return IS_MOCK_FLG; } }


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

    [ReadOnly]
    public GameType gameType = GameType.Normal;

    [ReadOnly]
    public MemberType[] cpuFlgs = 
    { 
        MemberType.CPU,
        MemberType.None,
        MemberType.None
    };

    [ReadOnly]
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

    [ReadOnly]
    public DisplayAspectType aspectType = DisplayAspectType.None;

}
