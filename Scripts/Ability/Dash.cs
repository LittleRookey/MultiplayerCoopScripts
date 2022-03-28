using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sisus.Init;

namespace LittleRookey.Character.Ability
{
    // Dash Ability
    [RequireComponent(typeof(Rigidbody2D))]
    public class Dash : Ability
    {
        private Rigidbody2D rb;
        [SerializeField] private float dashSpeed;
        

        private float gravityScale;

        public Dash(AbilitySettings abilitySettings, float m_dashSpeed): base(abilitySettings, true)
        {
            dashSpeed = m_dashSpeed;
        }
        protected override void Awake()
        {
            base.Awake();
            rb = GetComponent<Rigidbody2D>();
          
            
        }

        void Start()
        {
            gravityScale = rb.gravityScale;
        }

        protected override IEnumerator Cast()
        {
            isUsingAbility = true;
            anim.SetTrigger("Dash");
            float height = rb.velocity.y;
            yield return new WaitForSeconds(abilitySettings.castDelayTime);
            if (transform.localScale.x > 0)
            {
                //dash right
                rb.velocity = Vector2.right * dashSpeed;
                //rb.AddForce(Vector2.right * dashSpeed, ForceMode2D.Impulse);
            }
            else
            {
                rb.velocity = Vector2.left * dashSpeed;
                //rb.AddForce(Vector2.left * dashSpeed, ForceMode2D.Impulse);
            }
            rb.gravityScale = 0;
            cooldownSystem.PutOnCooldown(this);
            Debug.Log(this.ID + " "+this.CooldownDuration);
            yield return new WaitForSeconds(abilitySettings.abilityDurationTime);
            isUsingAbility = false;
            //rb.velocity = new Vector2(.01f * rb.velocity.normalized.x, height);
            rb.velocity = Vector2.zero;
            rb.gravityScale = gravityScale;
        }

    }

}
