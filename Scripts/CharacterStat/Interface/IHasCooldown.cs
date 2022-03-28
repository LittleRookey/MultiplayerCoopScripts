using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LittleRookey.Character.Cooldowns
{
    public interface IHasCooldown
    {
        int ID { get; }
        float CooldownDuration { get; }

    }
}
