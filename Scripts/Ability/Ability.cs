using System.Collections;
using UnityEngine;
using LittleRookey.Character.Cooldowns;
using UnityEngine.Events;
using Sisus.Init;

// Ability requires Cooldown. 
namespace LittleRookey.Character.Ability
{
    public enum eAbilityAnimType
    {
        enter,
        retain, 
        temp,
        onHit, 
        onHit2,
        exit
    };
    public abstract class Ability : MonoBehaviour, IHasCooldown
    {
        [HideInInspector]
        protected Animator anim;
        [Header("References")]
        [SerializeField]
        protected CooldownSystem cooldownSystem;

        [SerializeField]
        protected AbilitySettings abilitySettings;

        [Header("Settings")]
        [SerializeField] protected int id;
        [SerializeField] protected KeyCode abilityKey;


        [HideInInspector]
        public int ID => id;
        public float CooldownDuration => abilitySettings.cooldownTime;

        public bool isUsingAbility;

        public AudioSource _abilitySFX;

        public bool _spawnOnSelf;
        [HideIf(nameof(_spawnOnSelf), true)]
        public Transform _abilitySpawnTarget;

        //[Header("Object Settings")]
        //public bool spawnsObject; // if ability spawns some object
        //protected bool hasHealth; // if spawned object has health
        // TODO if has health, assign health to ability health
        


        [Header("Animation Settings")]
        public bool useAnimation;
        [HideIf(nameof(useAnimation), false)]
        [SerializeField]
        protected string enterAnimName, retainAnimName, tempAnimName, onHitAnimation, onHit2Animation, exitAnimName;

        protected bool canMoveOnUseAbility;
        //[HideIf(nameof(GetTempLength), 0, Comparison = UnityComparisonMethod.Equal)]
        [SerializeField]
        [HideIf(nameof(useAnimation), false)]
        protected int tempPlayAfterXTime;

        public Ability(AbilitySettings abilitySett, bool spawnOnSelf)
        {
            abilitySettings = abilitySett;
            _spawnOnSelf = spawnOnSelf;
            useAnimation = false;

        }
        public Ability(AbilitySettings abilitySett, bool spawnOnSelf, bool useAnim, string enter=null, string retain = null, string onHit = null, string onHit2 = null, string exit = null, string temp = null)
        {
            abilitySettings = abilitySett;
            _spawnOnSelf = spawnOnSelf;
            useAnimation = useAnim;
            enterAnimName = enter;
            retainAnimName = retain;
            tempAnimName = temp;
            onHitAnimation = onHit;
            onHit2Animation = onHit2;
            exitAnimName = exit;
        }

        //public Ability(AbilitySettings abilitySett, bool spawnOnSelf)
        //{
        //    abilitySettings = abilitySett;
        //    useAnimation = useAnim;
        //    _spawnOnSelf = spawnOnSelf;

        //}

        int GetTempLength()
        {
            return tempAnimName.Length;
        }

        protected abstract IEnumerator Cast();

        protected virtual void Awake()
        {
            cooldownSystem = GetComponent<CooldownSystem>();
            //id = Animator.StringToHash(this.abilityName);
            id = abilitySettings.abilityName.GetHashCode();
            if (anim == null)
                anim = GetComponent<Animator>();
        }

        public void PlayAnim(eAbilityAnimType animType)
        {
            if (useAnimation)
            {
                switch (animType)
                {
                    case eAbilityAnimType.enter:
                        if (enterAnimName != null)
                            anim.Play(enterAnimName);
                        else
                            Debug.LogError(animType.ToString() + " does not exist in " + gameObject.name);
                        break;
                    case eAbilityAnimType.exit:
                        if (exitAnimName != null)
                            anim.Play(exitAnimName);
                        else
                            Debug.LogError(animType.ToString() + " does not exist in " + gameObject.name);
                        break;
                    case eAbilityAnimType.onHit:
                        if (onHitAnimation != null)
                            anim.Play(onHitAnimation);
                        else
                            Debug.LogError(animType.ToString() + " does not exist in " + gameObject.name);
                        break;
                    case eAbilityAnimType.onHit2:
                        if (onHit2Animation != null)
                            anim.Play(onHit2Animation);
                        else
                            Debug.LogError(animType.ToString() + " does not exist in " + gameObject.name);
                        break;
                    case eAbilityAnimType.retain:
                        if (retainAnimName != null)
                            anim.Play(retainAnimName);
                        else
                            Debug.LogError(animType.ToString() + " does not exist in " + gameObject.name);
                        break;
                    case eAbilityAnimType.temp:
                        if (tempAnimName != null)
                            anim.Play(tempAnimName);
                        else
                            Debug.LogError(animType.ToString() + " does not exist in " + gameObject.name);
                        break;


                }

            }
        }

        public void SetAbilityKey(KeyCode key)
        {
            abilityKey = key;
        }
        
        protected virtual void Update()
        {
            if (cooldownSystem.IsOnCooldown(this.ID))
                return;
            if (Input.GetKeyDown(abilityKey) && !isUsingAbility)
            {
                StartCoroutine(Cast());
            }
        }
    }

}
