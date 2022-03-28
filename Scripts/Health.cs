using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using LittleRookey.Character;
using LittleRookey.Character.Ability;

//[RequireComponent(typeof(Canvas))]
[System.Serializable]
public class Health : MonoBehaviour
{
    protected Character character;

    [SerializeField]
    protected int maxHealth, currentHealth, hpRegeneration;

    [SerializeField]
    protected bool isVulnerable;
    [SerializeField]
    protected bool isDead;
    protected Rigidbody2D rb;

    [Header("Ability")]
    public bool isObject;

    [Header("Health Bar Setting")]
    public GameObject prefabHealthBar;
    public HealthBarBehaviour healthBar;

    [HideIf(nameof(isObject), false)]
    [SerializeField]
    protected Color healthBarColor;
    public bool showHealthBar;
    public bool showHealthBarConstantly;
    public bool showHealthBarOnHit;

    public UnityEvent<Ability> OnHealthZero;

    

    //[SerializeField]
    //private Vector3 dmgTextSpawnOffset = Vector3.up;

    private static readonly string objectState = "Object";

    // Should be ran in children class
    protected virtual void Awake()
    {
        if (gameObject.CompareTag(objectState) && !isObject)
            isObject = true;

        rb = GetComponent<Rigidbody2D>();
        if (rb==null && !isObject)
        {
            rb = transform.parent.GetComponent<Rigidbody2D>();
        }
        if (character == null)
        {
            character = GetComponent<Character>();
        }
        
    }

    protected virtual void Start()
    {
        if (healthBar == null)
        {
            var copy = Resources.Load("HealthBar/healthBarPrefab") as GameObject;

            Debug.Log(copy != null);
            Debug.Log(copy.name);
            prefabHealthBar = copy.gameObject;
            healthBar = Instantiate(copy, transform).GetComponent<HealthBarBehaviour>();
        }
        healthBar.SetHealth(currentHealth, maxHealth);
    }
    public virtual void Init()
    {
        healthBar.Init(maxHealth);
        if (isObject) // if health is from objects, not players or enemies
        {
            currentHealth = maxHealth;

        } else // if health is given to enemies or players
        {

        }
    }

    void IncreaseHealth(int inc)
    {
        currentHealth += inc;
        if (inc > 0) // gain health
        {
            
        } else if (inc < 0) // lose health
        {
            
        }
        // TODO play health bar animation

    }

    IEnumerator StartHealthRegen()
    {
        WaitForSeconds sec = new WaitForSeconds(1);
        while (true)
        {
            
            if (isDead)
                break;

            yield return sec;

        }

    } 


    public virtual void GetDamage(int dmg)
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
    }
    
    protected virtual void Dead()
    {
        isDead = true;
        healthBar.gameObject.SetActive(false);
    }

    public void DoKnockBack(Vector3 hitpos, float power, float knockBackTime)
    {
        Debug.Log("Knockbacked");
        
        float dir =  hitpos.x - transform.position.x;
        if (dir < 0) // hit from left
            dir = 1;
        else // hit from right
            dir = -1;
        Vector2 diff = (transform.position - hitpos).normalized;
        Vector2 force = diff * power;
        //transform.DOLocalMove(transform.position + (Vector3)force, knockBackTime);
        //transform.Translate(Vector2.left * power * Time.deltaTime * dir);
        Debug.Log("Direction: " + dir);
        Debug.Log(dir * force);
        rb.AddForce(force, ForceMode2D.Impulse);

        //StartCoroutine(KnockBack(dir, force));

    }

}
