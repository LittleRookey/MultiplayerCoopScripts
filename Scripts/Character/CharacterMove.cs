using UnityEngine;
using LittleRookey.Character.Ability;
using ExtensionMethods;

namespace LittleRookey.Character.Move
{
    public class CharacterMove : MonoBehaviour
    {

        
        private Dash playerDash;
        public float maxSpeed;
        public float jumpForce;
        [SerializeField]
        private int jumpMaxCount;
        private int jumpCount;
        private Rigidbody2D rb;

        private Animator anim;

        [SerializeField]
        public bool canMove, isJumping;

        [SerializeField]
        private float airResistance;

        CapsuleCollider2D capsuleCollider2D;

        private readonly int hashWalk = Animator.StringToHash("isWalking");

        private readonly int hashIdle = Animator.StringToHash("isIdle");
        private readonly int hashGround = Animator.StringToHash("isGround");
        private readonly int hashJump = Animator.StringToHash("Jump");

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            playerDash = GetComponent<Dash>();
            capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        }

        private void Start()
        {
            canMove = true;

        }
    
        private void Move()
        {
            // if not attacking 
            // move that amount 
            if (canMove)
            {

                float h = Input.GetAxisRaw("Horizontal");
                Debug.Log(h);
                rb.AddForce(Vector2.right * h, ForceMode2D.Impulse);
                if (rb.velocity.x > maxSpeed) // right max speed
                {
                    rb.velocity = new Vector2(maxSpeed, rb.velocity.y);

                }
                else if (rb.velocity.x < maxSpeed * -1) // left max speed
                {
                    rb.velocity = new Vector2(maxSpeed * -1, rb.velocity.y);

                }

            }

        }

        private void Jump()
        {
            //if (Character.Instance.isAttacking)
            //    return;

            if (jumpCount < jumpMaxCount)
            {
                isJumping = true;
                // manage jump animation play once
                jumpCount += 1;
                //anim.SetBool(hashJump, true);
                anim.SetTrigger(hashJump);
                rb.velocity = Vector2.up * jumpForce;
                //rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                Debug.Log("Jump");
            }     

        }

        void FlipSprites()
        {
            if (Input.GetAxisRaw("Horizontal") == -1)
                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            else if(Input.GetAxisRaw("Horizontal") == 1)
                transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);

        }

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
                
                // to stop rolling on the ground
                anim.SetTrigger("isGround");
                jumpCount = 0;
                isJumping = false;

                
                rayColor = Color.blue;
            }

            Debug.DrawRay(capsuleCollider2D.bounds.center + new Vector3(capsuleCollider2D.bounds.extents.x, 0), Vector2.down * (capsuleCollider2D.bounds.extents.y + extraHeightText), rayColor);
            Debug.DrawRay(capsuleCollider2D.bounds.center - new Vector3(capsuleCollider2D.bounds.extents.x, 0), Vector2.down * (capsuleCollider2D.bounds.extents.y + extraHeightText), rayColor);
            Debug.DrawRay(capsuleCollider2D.bounds.center - new Vector3(capsuleCollider2D.bounds.extents.x, capsuleCollider2D.bounds.extents.y), Vector2.right * (capsuleCollider2D.bounds.extents.x), rayColor);



            return raycastHit.collider != null;
        }
        void Update()
        {
            // Jump
            if (Input.GetKeyDown(KeyCode.Space))
                Jump();
            // Go back to IDLE animation
            if (Mathf.Abs(rb.velocity.x) < 0.05f)
            {
                anim.SetBool(hashIdle, true);
                //anim.SetBool(hashJump, false);
            }
            else
            {
                anim.SetBool(hashIdle, false);
                //anim.SetBool(hashWalk, true);
            }

            // Air resistance, stops moving
            //if (Input.GetButtonUp("Horizontal"))
            //    rb.velocity = new Vector2(airResistance * rb.velocity.normalized.x, rb.velocity.y);

            // when move left, flips sprite to left. 
            if (Input.GetButton("Horizontal"))
                FlipSprites();
        }
        void FixedUpdate()
        {
            if (!playerDash.isUsingAbility)
                if (canMove)
                {
                    float h = Input.GetAxisRaw("Horizontal");
                    rb.AddForce(Vector2.right * h, ForceMode2D.Impulse);
                    if (Mathf.Abs(h) > 0)
                    {
                        anim.SetBool(hashWalk, true);
                        anim.SetBool(hashIdle, false);
                    } else 
                    {
                        anim.SetBool(hashWalk, false);
                        anim.SetBool(hashIdle, true);
                    }
                    // Forcemode로 넉백을 표현 가능할듯!
                    if (rb.velocity.x > maxSpeed) // right max speed
                    {
                        rb.velocity = new Vector2(maxSpeed, rb.velocity.y);

                    }
                    else if (rb.velocity.x < maxSpeed * -1) // left max speed
                    {
                        rb.velocity = new Vector2(maxSpeed * -1, rb.velocity.y);

                    }

                }

            // Landing Platform
            // Checking ground
            if (rb.velocity.y <= 0)
            {
                //Collider2D coll = Physics2D.OverlapCircle(groundDetectPos.position, groundDetectRadius, groundLayer);
                //if(coll!= null)
                //{
                //    Debug.Log(coll.name);
                //    isGround = true;
                //} else
                //{
                //    isGround = false;
                //}

                // USING RAYS

                GroundDetect();
                //Debug.DrawRay(rb.position, Vector3.down, new Color(0, 1, 0));

                //RaycastHit2D rayHit = Physics2D.Raycast(rb.position, Vector2.down, .2f, LayerMask.GetMask("Ground"));
                //if (rayHit.collider != null)
                //{
                //    if (rayHit.distance < .3f)
                //    {
                //        // to stop rolling on the ground
                //        //anim.SetBool(hashJump, false);
                //        anim.SetTrigger("isGround");
                //        jumpCount = 0;
                //        //isGround = true;

                //    }

                //}
            }


        }

    }
}
