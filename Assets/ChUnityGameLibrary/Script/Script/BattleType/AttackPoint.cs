/**
* @file AttackPoint.cs
* @brief �I�u�W�F�N�g�̍U���͂��Ǘ�����@�\
* @author Chronoss0518
* @date 2022/01/02
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/**
* @brief   �I�u�W�F�N�g�̍U���͂��Ǘ�����̃N���X
*/

namespace ChUnity.BattleType
{
    public class AttackPoint : MonoBehaviour
    {
        /**
         * ���̃X�N���v�g�����I�u�W�F�N�g�̍U����
        */
        [SerializeField]
        protected int atk = 0;

        /**
         * ���̃X�N���v�g�S�̂̍ő�U���͂ƍŒ�U����
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
