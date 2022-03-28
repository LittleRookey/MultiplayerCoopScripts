using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;
using LittleRookey.Character.Move;
using LittleRookey.Character.Attack;
using LittleRookey.CharacterStats;

namespace LittleRookey.Character
{
    public class Character : MonoBehaviour
    {
        //public static Character Instance;

        //[SerializeField]
        //private AnimancerComponent animancer;

        public Animator anim;
        public bool isPlayer;
        public string m_name;
        [SerializeField] private int level = 1;
        public CharacterStat p_maxHP;
        private float p_currentHP;
        public CharacterStat Strength; // increass physical power 
        public CharacterStat Vitality; // increases health and durability(³»±¸µµ)
        public CharacterStat Intelligance;
        public CharacterStat Dexterity;
        public CharacterStat Luck;

        public CharacterStat defense;
        public CharacterStat shield;
        public CharacterStat criticalChance;
        public CharacterStat criticalDamage;
        public CharacterStat armorPenetration;
        public CharacterStat magicPenetration;

        public CharacterStat attackSpeed;

        public float mass;
        public float moveSpeed;

        //[Header("Basic Character Scripts(Required)")]
        //public CharacterMove charMove;
        //public PlayerAttack playerAttack;
        //public Health health;
        //private bool isHurt;

            //}
            //public bool isAttacking
            //{
            //    get
            //    {
            //        return playerAttack.isAttacking;
            //    }
            //    set
            //    {
            //        playerAttack.isAttacking = value;
            //    }
            //}

        //    private void Awake()
        //{
            //if (Instance == null)
            //    Instance = this;
            //if (animancer == null)
            //    animancer = GetComponent<AnimancerComponent>();
            //if (anim == null)
            //    anim = GetComponent<Animator>();
            //if (charMove == null)
            //    charMove = GetComponent<CharacterMove>();
            //if (playerAttack == null)
            //    playerAttack = GetComponent<PlayerAttack>();
            //if (health == null)
            //    health = GetComponent<Health>();
        //}

    }
}
