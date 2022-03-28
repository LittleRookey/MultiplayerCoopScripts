using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// setting (int level, 
[CreateAssetMenu(fileName = "AbilitySettings", menuName = "ScriptableObjects/Ability", order = 1)]
[System.Serializable]
public class AbilitySettings : ScriptableObject
{
    public string abilityName;
    public int level;
    public float cooldownTime;
    public float abilityDurationTime;
    public float castDelayTime;

    
    public GameObject abilityVFX;
    public Vector2 abilityVfxOffset;

    //public virtual void Activate(GameObject parent) { }
}
