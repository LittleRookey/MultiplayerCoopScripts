using UnityEngine;
using DamageNumbersPro;

public class EnemyHealth : Health
{

    //[Header("DamageTextSettings/Player")]
    //[SerializeField]
    //private DamageNumber playerDmgTxt;
    //[SerializeField]
    //private Color p_normalDamageColor, p_criticalDamageColor, p_healColor;
    [Header("DamageTextSettings/Enemy")]
    [SerializeField]
    private DamageNumber enemyDmgTxt;
    [SerializeField]
    private Color e_normalDamageColor, e_criticalDamageColor, e_healColor;
    // Start is called before the first frame update
    public bool getAttacked;

    MonsterAnimation monsterAnim;

    protected override void Awake()
    {
        base.Awake();
        if (healthBar == null)
        {
            healthBar = GetComponentInChildren<Canvas>().GetComponent<HealthBarBehaviour>();
        }
        if (monsterAnim)
            monsterAnim = GetComponent<MonsterAnimation>();
    }

    protected override void Start()
    {
        base.Start();
    }

    public override void GetDamage(int dmg)
    {

        if (isVulnerable)
            return;
        currentHealth -= dmg;

        healthBar.SetHealth(currentHealth, maxHealth);
        if (currentHealth < 0)
        {
            currentHealth = 0;
            // 
            //OnHealthZero.Invoke();
            Dead();
        }
        getAttacked = true;
        
        Debug.Log("EnemyHealth");
    }

    protected override void Dead()
    {
        base.Dead();
        healthBar.gameObject.SetActive(false);
        monsterAnim.Die();
    }

}
