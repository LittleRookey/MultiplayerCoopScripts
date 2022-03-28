using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;

namespace LittleRookey.CharacterStats
{
    [System.Serializable]
    public class CharacterStat
    {
        public float BaseValue;

        public virtual float Value
        {
            get
            {
                if (isDirty || BaseValue != lastBaseValue)
                {
                    lastBaseValue = BaseValue;
                    _value = CalculateFinalValue();
                    isDirty = false;
                }
                return _value;
            }
        }

        protected bool isDirty = true;
        protected float _value; //most recent value
        protected float lastBaseValue = float.MinValue;

        protected readonly List<StatModifier> statModifiers;
        public readonly ReadOnlyCollection<StatModifier> StatModifiers;

        // Constructors 1
        public CharacterStat()
        {
            statModifiers = new List<StatModifier>();
            StatModifiers = statModifiers.AsReadOnly();
        }

        // Constructor2
        public CharacterStat(float baseValue) : this()
        {
            BaseValue = baseValue;
        }

        public virtual void AddModifier(StatModifier mod)
        {
            isDirty = true;
            statModifiers.Add(mod);
            statModifiers.Sort(CompareModifierOrder);
        }

        protected virtual int CompareModifierOrder(StatModifier before, StatModifier after)
        {
            if (before.Order < after.Order)
                return -1;
            else if (before.Order > after.Order)
                return 1;
            return 0;
        }

        protected virtual bool RemoveModifier(StatModifier mod)
        {

            if (statModifiers.Remove(mod))
            {
                isDirty = true;
                return true;
            }
            return false;
        }

        public virtual bool RemoveAllModifiersFromSource(object source)
        {
            bool didRemove = false;
            for (int i = statModifiers.Count - 1; i > 0; i--)
            {
                if (statModifiers[i].Source == source)
                {
                    isDirty = true;
                    didRemove = true;
                    statModifiers.RemoveAt(i);
                }
            }
            return didRemove;
        }


        protected virtual float CalculateFinalValue()
        {
            float finalValue = BaseValue;
            float sumPercentAdd = 0;

            for (int i = 0; i < statModifiers.Count; i++)
            {
                StatModifier mod = statModifiers[i];

                if (mod.Type == eStatModType.Flat)
                {
                    finalValue += mod.Value;
                }
                else if (mod.Type == eStatModType.PercentAdd)
                {
                    sumPercentAdd += mod.Value;

                    if (i + 1 >= statModifiers.Count || statModifiers[i + 1].Type != eStatModType.PercentAdd)
                    {
                        finalValue *= 1 + sumPercentAdd;
                        sumPercentAdd = 0;
                    }
                }
                else if (mod.Type == eStatModType.PercentMult)
                {
                    finalValue *= (1 + mod.Value);
                }
            }

            return (float)Mathf.Round(finalValue);
        }
    }
}
