using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LittleRookey.Character.Move;
using LittleRookey.Character;

namespace LittleRookey.Character.Attack
{
    enum eAttackType
    {
        single,
        all
    };

    [RequireComponent(typeof(Character))]
    public class PlayerAttack : MonoBehaviour
    {
        [Header("Attack Settings")]
        [SerializeField]
        private int attack;

        [SerializeField] 
        private eAttackType attackType;

        
        public float attackRange;
        public bool isAttacking;

        [SerializeField]
        private Transform attackPoint;

        public LayerMask enemyLayers;

        private Animator anim;
        // attack speed calculation from league of legend
        // at level 10, 3.22% * 9 *
        // b = base, g = growth statistic, n = champion l;evel, n-1 = total amount of level ups
        // health 600 
        // at level 2, n=2, g = 106, b= 600
        // 600 + 106 * (2-1) * (.7025 + 0.0175 * (2-1) )
        // statistic = b + g * (n-1) * (.7025 + 0.0175 * (n-1))
        // g = growth statistic
        // stats Increase = g * (.65 + 0.035 * newlevel)
        //[SerializeField] private float attackSpeed;

        [Header("Knockback Settings")]
        public bool enableKnockBack;
        private bool isKnockback = true;

        public float knockbackPower { get; private set; }
        [HideIf(nameof(enableKnockBack), false)]
        public float KnockBackPower;

        [HideIf(nameof(enableKnockBack), false)]
        [SerializeField]
        private float knockBackTime;

        private Character character;
        private CharacterMove charMove;

        private readonly int hashAttack_v = Animator.StringToHash("1hand-attack-v");
        private readonly int hashAttack_h = Animator.StringToHash("1hand-attack-h");
        private readonly int hashAttack_stabmiddle = Animator.StringToHash("1hand-attack-stab-middle");

        private void Awake()
        {
            if (character == null)
                character = GetComponent<Character>();
            if (charMove == null)
                charMove = GetComponent<CharacterMove>();
            if (anim == null)
                anim = GetComponent<Animator>();
        }
        void Start()
        {
            knockbackPower = KnockBackPower;
            //isAttacking = false;
        }

        // used on animation event
        public void GiveDamage()
        {
            // if hit nothing
            isAttacking = false;
            switch (attackType)
            {
                // Single Attack
                case eAttackType.single:
                    Collider2D hitEnemy = Physics2D.OverlapCircle(attackPoint.position, attackRange, enemyLayers);
                    if (hitEnemy == null)
                    {
                        charMove.canMove = true;
                        return;
                    }
                    hitEnemy.GetComponent<Health>().GetDamage(attack);
                    if (enableKnockBack)
                    {
                        hitEnemy.GetComponent<Health>().DoKnockBack(transform.position, knockbackPower, knockBackTime);
                    }
                    break;

                // Area attack
                case eAttackType.all:
                    Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
                    if (hitEnemies == null)
                    {
                        charMove.canMove = true;
                        return;
                    }
                    foreach (Collider2D enem in hitEnemies)
                    {
                        enem.GetComponent<Health>().GetDamage(attack);
                        if (enableKnockBack)
                        {
                            enem.GetComponent<Health>().DoKnockBack(transform.position, knockbackPower, knockBackTime);

                        }
                    }
                    break;

            }
         

            // give knock back if enableKnockBack is true
            
            charMove.canMove = true;
            
        }
        /*       
        Steps
        1) Compare Attack Classes
        2) Determine Type of Hit
        3) Roll for Damage
        4) Reduce Damage for Block
        5) Reduce Damage for Armor
        6) Reduce Damage for Resistances
        7) Deal Damage
        8) Reduce Defender's and Attacker's Attack Class

        Types of Hit
        Critical miss - The attacker falls over.
        Miss - The attacker swings at air.
        Parry - The defender hits the attacker's attack, both lose time.
        Block - The defender has a shield reduce the damage before their armor reduces it a second time.
        Hit - The defender takes raw damage.
        Direct Hit - The defender takes damage that ignores armor.
        Critical Hit - The defender takes strong damage.
        Brutal Hit - The defender takes strong damage that ignores armor.

        The formula for the reduction is:
        Armor / (Damage + Armor) = Percent Damage Reduced.

        */
        private IEnumerator OnAttack()
        {
            WaitForSeconds attackDelay = new WaitForSeconds(1);
            charMove.canMove = false;
            isAttacking = true;
            anim.SetTrigger("OnOneHandAttack");

            yield return attackDelay;

            isAttacking = false;
        }
        public void Attack()
        {
            // Single EnemyLayer enemy hit
            //charMove.canMove = false;
            //OneHandAttack();
            if (!isAttacking)
            {
                charMove.canMove = false;
                //isAttacking = true;
                anim.SetTrigger("OnOneHandAttack");
                Debug.Log("attack");
            }
            //Collider2D hitEnemy = Physics2D.OverlapCircle(attackPoint.position, attackRange, enemyLayers);
            //if (hitEnemy == null)
            //{
            //    // if hit nothing
            //    return;
            //}
            //hitEnemy.GetComponent<Health>().GiveDamage(attack);
            // TODO instantiate enemy's attached hit vfx or animation effect! 
            // Also add knockback if possible. 

            //if (hitEnemy.GetComponent<Health>() == null)
            //{
            //    Debug.LogWarning("Enemy does not have Health Component! Attach it now!");
            //}

            // ranged attack hit

            //Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
            //foreach(Collider2D enemy in hitEnemies)
            //{
            //    enemy.GetComponent<Health>().GiveDamage(attack);
            //}

        }




        //IEnumerator KnockBack(float dir)
        //{
        //    isKnockback = true;
        //    float ctime = 0;
        //    while (ctime < .2f)
        //    {
        //        if(transform.localScale.x < 0)
        //        {
        //            transform.Translate(Vector2.left * knockbackPower * Time.deltaTime * dir);
        //        } else
        //        {
        //            transform.Translate(Vector2.left * knockbackPower * Time.deltaTime * -1f * dir);
        //        }
        //        ctime += Time.deltaTime;
        //        yield return null;
        //    }
        //    isKnockback = false;
        //}
        //public void Attack()
        //{
        //    isAttacking = true;
        //    charMove.canMove = false;
        //}
        private void OnDrawGizmosSelected()
        {
            if (attackPoint == null)
                return;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
            }
        }

    }
}
