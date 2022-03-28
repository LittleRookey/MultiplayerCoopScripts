using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

public class Projectile : MonoBehaviour
{
    public eProjectileType shootType = eProjectileType.arch;

    public int damage;

    public bool isFromPlayer;
    int destroyTime = 5;

    bool destroyOnCollision;
    bool stickOnObject;

    CapsuleCollider2D capsuleCollider;
    Rigidbody2D rb;

    [Tooltip("Position we want to hit")]
    public Vector3 targetPos;

    [Tooltip("Horizontal speed, in units/sec")]
    public float speed = 10;
    //public float maxSpeed;
    [Tooltip("How high the arc should be, in units")]
    public float arcHeight = 1;

    Transform shotFrom;

    TrailRenderer trail;

    Vector3 movePosition;
    Vector3 nextPos;

    float playerX;
    float targetX;
    float dist;
    float nextX;
    float baseY;
    float height;

    bool gotHit;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        trail = GetComponent<TrailRenderer>();
        Debug.Log("Projectile shoot angle: " + transform.rotation.eulerAngles.z);
    }

    public void Init(eProjectileType pType, Transform target, Transform from, int dmg, float arrowHeight, float arrowSpeed, bool fromPlayer, bool destroyOnColl=false, bool stickOnObj=true, int destroyT = 5)
    {
        damage = dmg;
        arcHeight = arrowHeight;
        speed = arrowSpeed;
        isFromPlayer = fromPlayer;
        capsuleCollider.isTrigger = true;
        destroyTime = destroyT;
        destroyOnCollision = destroyOnColl;
        stickOnObject = stickOnObj;

        shotFrom = from;
        targetPos.Set(target.position.x, target.position.y, target.position.z);
        shootType = pType;

    }

    public void InitDefault(Transform target, bool fromPlayer, int dmg=10)
    {
        
        damage = dmg;
        //maxSpeed = 20f;
        //currentSpeed = 1f;
        speed = 20f;
        isFromPlayer = fromPlayer;
        capsuleCollider.isTrigger = true;
        destroyTime = 5;
        destroyOnCollision = false;
        stickOnObject = true;
        rb.isKinematic = false;
        targetPos.Set(target.position.x, target.position.y, target.position.z);
        shootType = eProjectileType.straight;
    }
    private void OnEnable()
    {
        PhysicsSetting();

        Destroy(this.gameObject, destroyTime);
        
        
        //if (capsuleCollider.enabled)
        //{
        //    switch (shootType)
        //    {
        //        case eProjectileType.arch:
        //            Debug.Log("Arch");
        //            //ArrowArch();
        //            ArrowTest();
        //            break;
        //        case eProjectileType.straight:
        //            ArrowStraight();
        //            Debug.Log("Straight");
        //            //ArrowStraight_Physics();
        //            break;

        //    }
        //    //transform.right = rb.velocity;
        //}
    }
   
    void PhysicsSetting()
    {
        rb.isKinematic = false;
        capsuleCollider.enabled = true;
        capsuleCollider.isTrigger = true;
        trail.enabled = true;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        bool collided = false;
        // shoot from enemy
        if (collision.gameObject.CompareTag("Player") && !isFromPlayer)
        {
            collided = true;
            collision.gameObject.GetComponent<Health>().GetDamage(damage);

        }
        else if (collision.gameObject.CompareTag("Object") && !isFromPlayer) 
        {  // handles object attack
            collided = true;
            collision.gameObject.GetComponent<Health>().GetDamage(damage);
        }
        else if (isFromPlayer && collision.gameObject.CompareTag("Enemy"))
        { // shoot from player
            Debug.Log("Tag collided with" + collision.gameObject.tag);
            collided = true;
            collision.gameObject.GetComponent<Health>().GetDamage(damage);
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            collided = true;

        }
        

        // TODO object pool
        if (collided)
        {
            gotHit = true;
            trail.enabled = false;
            rb.velocity = Vector2.zero;
            //rb.constraints = RigidbodyConstraints2D.FreezeAll;
            capsuleCollider.enabled = false;
            if (stickOnObject)
            {
                transform.parent = collision.transform;
                rb.isKinematic = true;
            }
            if (destroyOnCollision)
            {
                Destroy(gameObject);
            }
            //Debug.Log("Tag collided with" + collision.gameObject.tag);
        }
        
    }


    private void Update()
    {
        if (!gotHit)
            transform.right = rb.velocity;


    }

    void ArrowTest()
    {
        rb.velocity = transform.right * speed;
        //Debug.Log("Projectile velocity: " + MathfP.ProjectileMotion(rb.position, targetPos));
        //rb.velocity = MathfP.ProjectileMotion(rb.position, targetPos);
    }

    public void ArrowArch()
    {
        playerX = shotFrom.position.x;
        targetX = targetPos.x;
        dist = targetX - playerX;
        nextX = Mathf.MoveTowards(transform.position.x, targetX, speed * Time.deltaTime);
        baseY = Mathf.Lerp(shotFrom.position.y, targetPos.y, (nextX - playerX) / dist);
        height = arcHeight * (nextX - playerX) * (nextX - targetX) / (-0.25f * dist * dist);

        movePosition = new Vector3(nextX, baseY + height, transform.position.z);

        transform.rotation = LookAtTarget(movePosition - transform.position);
        transform.position = movePosition;

        if (movePosition == targetPos)
        {
            //Destroy(gameObject);
        }
    }

    void ArrowStraight()
    {
        Vector3 nextPos = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        // Rotate to face the next position, and then move there
        transform.rotation = LookAtTarget(nextPos - transform.position);
        transform.position = nextPos;
    }

    public static Quaternion LookAtTarget(Vector2 r)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(r.y, r.x) * Mathf.Rad2Deg);
    }

   




    //void ArrowArch()
    //{
        
    //    float x0 = startPos.x;
    //    float x1 = targetPos.x;
    //    float dist = x1 - x0;
    //    float nextX = Mathf.MoveTowards(transform.position.x, x1, speed * Time.deltaTime);
    //    float baseY = Mathf.Lerp(startPos.y, targetPos.y, (nextX - x0) / dist);
    //    float arc = arcHeight * (nextX - x0) * (nextX - x1) / (-0.25f * dist * dist);
    //    nextPos = new Vector3(nextX, baseY + arc, transform.position.z);

    //    // Rotate to face the next position, and then move there
    //    //Debug.Log(transform.position + " / " + nextPos);
    //    //Debug.Log("lookat: " +LookAt2D(nextPos - transform.position).eulerAngles);
    //    Vector3 rot = LookAt2D(nextPos - transform.position).eulerAngles;
        
    //    if (Mathf.Abs(rot.z) % 360 >= 90.0f && Mathf.Abs(rot.z) % 360 <= 270.0f) // projectile shoot left 
    //    {
    //        //Debug.Log("prev: " + transform.rotation.eulerAngles.z + " after: " + rot.z);
    //        if (Mathf.Abs(transform.rotation.eulerAngles.z) < Mathf.Abs(rot.z))
    //        {
    //            Debug.Log("Look left!");
    //            Debug.Log("prev: " + transform.rotation.eulerAngles.z + " after: " + rot.z);
    //            transform.rotation = LookAt2D(nextPos - transform.position);
    //            Debug.Log("Rotation: " + transform.rotation.eulerAngles);
    //        }
    //    }

    //    //if (transform.rotation.eulerAngles.z > prev.z)
    //    //{
    //    //    transform.rotation = LookAt2D(nextPos  - transform.position);
    //    //}
    //    //transform.rotation = LookAt2D(nextPos - transform.position);
    //    //Debug.Log("prev: "+ prev + " after: " + transform.rotation.eulerAngles);
    //    //prev = transform.rotation.eulerAngles;
    //    transform.position = nextPos;
        
    //}


}
