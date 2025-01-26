using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "AttackData", menuName = "BattleTypeRange/AttackPoint", order = 1)]
public class AttackPointRange : ScriptableObject
{
    public int lATK = 0;
    public int hATK = 100;
}