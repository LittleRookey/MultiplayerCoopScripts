using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LittleRookey.Character;
public class Enemy : Character
{
    public Transform player;
    Rigidbody rb;
    Health health;

    private MonsterAnimation monsterAnim;
    public List<MonsterActing> monsterActingBehavior;

    //public int moveDir;
    public bool lookRight;

    public bool spawnByFacingRight;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = transform.GetComponent<Rigidbody>();
        monsterAnim = GetComponent<MonsterAnimation>();
        if (monsterAnim == null)
            monsterAnim = transform.GetChild(0).GetComponent<MonsterAnimation>();
        if (health == null)
            health = GetComponent<Health>();
        if (anim == null)
            anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        if (spawnByFacingRight)
        {
            FaceRight();
            //transform.localScale = new Vector3(1 , transform.localScale.y, transform.localScale.z);
            //moveDir = 1;
        } else
        {
            FaceLeft();
            //transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            //moveDir = -1;
        }
        monsterAnim.Walk();
    }

    public void Init()
    {
        if (spawnByFacingRight)
            FaceRight();
        else
            FaceLeft();

        

    }

    private void OnEnable()
    {
        Init();

    }
    public void Flip(Transform targ)
    {
        // left
        if (transform.position.x > targ.position.x)
        {
            FaceLeft();
            //moveDir = 1;
        }
        else
        {
            FaceRight();
            //moveDir = -1;
        }
    }

    public void FaceLeft()
    {
        transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        lookRight = false;
    }

    public void FaceRight()
    {
        transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        lookRight = true;
    }
}
