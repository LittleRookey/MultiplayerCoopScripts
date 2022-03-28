
using UnityEngine;

using System.Collections;

namespace LittleRookey.Character.Ability
{
    [RequireComponent(typeof(LittleRookey.Character.Cooldowns.CooldownSystem))]
    public class MagicShield : Ability
    {
        WaitForSeconds delayTime;
        WaitForSeconds durationTime;

        private static readonly string shine = "shine";
        private static readonly string fadeOut = "fadeOut";
        private static readonly string hit = "hit";
        private static readonly string strongHit = "strongHit";

        public MagicShield(AbilitySettings abilitySettings, bool spawnOnSelf) : base(abilitySettings, spawnOnSelf)
        {

        }
        // Start is called before the first frame update
        void Start()
        {
            delayTime = new WaitForSeconds(abilitySettings.castDelayTime);
            durationTime = new WaitForSeconds(abilitySettings.abilityDurationTime);
        }
        

        // assuming this script is attached under player gameobject
        protected override IEnumerator Cast()
        {
            yield return delayTime;
            isUsingAbility = true;

            GameObject vfx =  Instantiate(abilitySettings.abilityVFX, transform.position, Quaternion.identity);
            cooldownSystem.PutOnCooldown(this);
            Animator vfxAnim = vfx.GetComponent<Animator>(); 
            if (_spawnOnSelf)
            {
                vfx.transform.parent = transform;
            } else if(_abilitySpawnTarget != null)
            {
                vfx.transform.parent = _abilitySpawnTarget.transform;
            }
            vfx.transform.localPosition = abilitySettings.abilityVfxOffset;


            //StartCoroutine(PlayTemp());
            yield return durationTime; // after ability duration ends
            isUsingAbility = false;
            vfx.GetComponent<Animator>().Play(fadeOut);


        }

        IEnumerator PlayTemp()
        {
            yield return new WaitForSeconds(tempPlayAfterXTime);
            PlayAnim(eAbilityAnimType.temp);
        }


 
}
}
