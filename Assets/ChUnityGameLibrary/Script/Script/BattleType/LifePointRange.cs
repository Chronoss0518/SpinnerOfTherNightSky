using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LifePointData", menuName = "BattleTypeRange/LifePoint", order = 1)]
public class LifePointRange : ScriptableObject
{
    public int lMaxLP = 0;
    public int hMaxLP = 100;
}
