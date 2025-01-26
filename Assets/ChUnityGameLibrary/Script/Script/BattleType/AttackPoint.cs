/**
* @file AttackPoint.cs
* @brief オブジェクトの攻撃力を管理する機能
* @author Chronoss0518
* @date 2022/01/02
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/**
* @brief   オブジェクトの攻撃力を管理するのクラス
*/

namespace ChUnity.BattleType
{
    public class AttackPoint : MonoBehaviour
    {
        /**
         * このスクリプトを持つオブジェクトの攻撃力
        */
        [SerializeField]
        protected int atk = 0;

        /**
         * このスクリプト全体の最大攻撃力と最低攻撃力
        */
        [SerializeField]
        private AttackPointRange range;

        public bool isSetRange { get { return range != null; } }

        public int attackPoint
        {
            set {
                if(range != null) atk = (range.lATK > value ? range.lATK : (range.hATK < value ? range.hATK : value)); 
            }
            get { return atk; }
        }

        public int lATK { get { return range != null ? range.lATK : 0 ; } }
        public int hATK { get { return range != null ? range.hATK : 0xffff ; } }


    }

}
