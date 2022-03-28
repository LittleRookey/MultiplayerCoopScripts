using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;
public enum eMonsterAI
{
    Stay,
    Chase,
    Defend,
    Attack,
    Skill,
    Avoid,
    Retreat


};

public enum eProjectileType
{
    straight,
    arch
};

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private List<MonsterActing> behaviours;

    MonsterAnimation monsterAnim;
    Animator anim;
    EnemyHealth enemyHealth;


    bool hasTarget;

    public Transform target;

    [Header("AI Setting")]
    public float reactTime = 1f;
    public float walkSpeed = 10f;


    [Header("Melee Setting")]
    public bool hasMelee;

    [HideIf(nameof(hasMelee), false)]
    [Range(0.0f, 5.0f)]
    public float meleeRange;
    public GameObject meleeVFX;
    public Vector2 meleeVFXOffset;


    // sound
    // TODO add Action class for these values
    // and add that class here

    [Header("Bow Setting")]

    public bool hasBow;
    [HideIf(nameof(hasBow), false)]
    [Range(0.0f, 30.0f)]
    public float minBowRange, maxBowRange;
    [HideIf(nameof(hasBow), false)] public GameObject arrow;
    [HideIf(nameof(hasBow), false)] public eProjectileType shootType;
    [HideIf(nameof(hasBow), false)] public GameObject arrowVFX;
    [HideIf(nameof(hasBow), false)] public Vector2 arrowVFXOffset;
    [HideIf(nameof(hasBow), false)] public float arrowShootAngle;
    [HideIf(nameof(hasBow), false)] public float arrowArchHeight;
    [HideIf(nameof(hasBow), false)] public float arrowSpeed;
    [HideIf(nameof(hasBow), false)] public bool stickOnObject;
    [HideIf(nameof(hasBow), false)] public bool destroyOnCollision;


    CapsuleCollider2D capsuleCollider2D;

    [HideInInspector]
    public Enemy enemy;

    int actionCount;

    private void Awake()
    {
        if (anim == null)
        {
            anim = GetComponent<Animator>();
            if (anim == null)
                anim = GetComponentInChildren<Animator>();
        }
        if (monsterAnim == null)
        {
            monsterAnim = GetComponent<MonsterAnimation>();
            if (monsterAnim == null)
                monsterAnim = GetComponentInChildren<MonsterAnimation>();
        }
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        enemy = GetComponent<Enemy>();
    }


    // Start is called before the first frame update
    void Start()
    {
        // checks if monster has default bow action
        for (int i = 0; i < behaviours.Count; i++)
        {
            if (behaviours[i] == MonsterActing.Bow)
                hasBow = true;
        }

        //TODO init
        StartCoroutine(StartAct());
    }

    public void SetTarget(Transform targ)
    {
        target = targ;
    }

    IEnumerator StartAct()
    {
        yield return new WaitForSeconds(reactTime);
        DecideLookDirection();
        Act();
        StartCoroutine(StartAct());
    }
    public void Act()
    {
        // Detect player tag within range
        float dist;
        if (target != null)
        {
            dist = Vector2.Distance(transform.position, target.position);
            ChooseAction(dist);
        }
        else
        { // if target is null, Idle or Patrol

        }
        //Physics2D.OverlapBox(transform.position, detectRange, 0f, LayerMask.GetMask("Player"));


    }

    public void ChooseAction(float dist)
    {
        if (target != null)
        {

            if (hasBow)
            {
                if (minBowRange <= dist && dist <= maxBowRange)
                {
                    monsterAnim.Bow();
                    //ShootProjectile();

                } // has bow but not within range
                else if (dist <= meleeRange)// use melee
                {
                    if (hasMelee)
                    {

                        monsterAnim.Smash();

                    }
                }

            } // if not have range attack, use melee
            else
            {
                if (meleeRange <= dist)// use melee
                {
                    if (hasMelee)
                        monsterAnim.Smash();
                }
            }

        }
        else
        { // if target not found yet
            Patrol();
        }
    }

    public IEnumerator Patrol()
    {
        float sec = Random.Range(1, 3f);
        yield return new WaitForSeconds(sec);
        DecideLookDirection();


    }

    void Walk(float seconds)
    {
        monsterAnim.Walk();

    }

    void DecideLookDirection()
    {
        if (target == null)
        {
            if (Random.Range(0f, 100f) < 50.0f)
            {
                enemy.FaceLeft();
            }
            else
            {
                enemy.FaceRight();
            }
        }
        else
        { // if there is a target, look at target 

            if (transform.position.x > target.transform.position.x)
            {
                // if target is on the left
                enemy.FaceLeft();
            }
            else
            {
                enemy.FaceRight();
            }
        }
    }
    public void ShootProjectile()
    {
        //float actualAngle = arrowShootAngle;
        float ang = 0;
        Debug.Log($"Distance1: {Vector2.Distance(target.position, transform.position)}  Distance2: {(target.position - transform.position).magnitude}");
        if (!MathfP.AngleOfReach((target.position - transform.position).magnitude, arrowSpeed, out ang))
        { // if target is not reachable with current arrow speed
            Debug.Log("Cant reach!!!!!!!!!!!!");
            arrowSpeed += 3f;
            monsterAnim.Idle();
            return;
        }
        if (!enemy.lookRight)
        {
            //actualAngle = 180 - arrowShootAngle;
            ang = 180 - ang;
        }
        Debug.Log("angle:" + ang);
        Debug.Log("Shot Arrow@@@@@@@@@@@@@@@@@@@@@");
        Projectile projectile = Instantiate(arrow, transform.position + (Vector3)arrowVFXOffset, Quaternion.Euler(0, 0, ang)).GetComponent<Projectile>();
        Debug.Log(projectile.transform.rotation.eulerAngles);
        // When enemy faces left, projectile rotation z to 180 

        if (target != null)
        {
            Debug.Log("target position: " + target.position);
            // if target is on the air, possibly jumping
            if (target.position.y >= transform.position.y + 2.0f)
            {
                shootType = eProjectileType.straight;

                //MathfP.AngleOfReach(Vector2.Distance(target.position, transform.position), arrowSpeed, out ang);
                //projectile.transform.rotation = Quaternion.Euler(0, 0, enemy.lookRight ? ang : 180f - ang);
                Debug.Log($"target: {target.position} RealAngle: {ang}");
            }
            else
            {
                shootType = eProjectileType.arch;
            }

            projectile.Init(shootType, target, transform, 5, arrowArchHeight, arrowSpeed, enemy.isPlayer, destroyOnCollision, stickOnObject);
        }
        else
            monsterAnim.Idle();

        //projectile.Init(shootType, target, 5, arrowArchHeight, arrowSpeed, enemy.isPlayer, destroyOnCollision, stickOnObject);
        //Debug.Log("Shot at pos: " + transform.position);
        //Debug.Log(MathfP.ProjectileMotion(transform.position, target.position));
        //projectile.GetComponent<Rigidbody2D>().velocity = MathfP.ProjectileMotion(transform.position, target.position);
        if (shootType == eProjectileType.arch)
            projectile.GetComponent<Rigidbody2D>().velocity = projectile.transform.right * arrowSpeed;
        else if (shootType == eProjectileType.straight)
            projectile.GetComponent<Rigidbody2D>().AddForce(projectile.transform.right * arrowSpeed * 2, ForceMode2D.Impulse);
    }
    //void MoveTowards()

    // Detects ground by shooting box ray to the ground 
    bool GroundDetect()
    {
        float extraHeightText = .1f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(capsuleCollider2D.bounds.center, capsuleCollider2D.bounds.size, 0f, Vector2.down, extraHeightText, LayerMask.GetMask("Ground"));
        //Debug.DrawRay(rb.position, Vector3.down, new Color(0, 1, 0));

        Color rayColor;

        if (raycastHit.collider == null)
        {
            rayColor = Color.yellow;
        }
        else
        {
            rayColor = Color.blue;
        }

        Debug.DrawRay(capsuleCollider2D.bounds.center + new Vector3(capsuleCollider2D.bounds.extents.x, 0), Vector2.down * (capsuleCollider2D.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(capsuleCollider2D.bounds.center - new Vector3(capsuleCollider2D.bounds.extents.x, 0), Vector2.down * (capsuleCollider2D.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(capsuleCollider2D.bounds.center - new Vector3(capsuleCollider2D.bounds.extents.x, capsuleCollider2D.bounds.extents.y), Vector2.right * (capsuleCollider2D.bounds.extents.x), rayColor);



        return raycastHit.collider != null;
    }

    private void FixedUpdate()
    {
        if (GroundDetect())
        {
            Debug.Log("Grounded");
        }
    }

    private void OnDrawGizmos()
    {
        if (hasBow)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, minBowRange);

            Gizmos.DrawWireSphere(transform.position, maxBowRange);
        }
        if (hasMelee)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, meleeRange);
        }

    }
}