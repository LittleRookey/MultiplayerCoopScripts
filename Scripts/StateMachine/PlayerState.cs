using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState 
{
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;

    protected float startTime;
    private string animBoolName;

    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;

    }

    public virtual void Enter()
    {
        DoChecks();
        player.anim.SetBool(animBoolName, true);
        startTime = Time.time;
        
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }
    // Logic gets called every frame
    public virtual void LogicUpdate()
    {

    }
    // Gets called every fixed update
    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    // look for ground, look for wall, do it once and check
    public virtual void DoChecks()
    {

    }
}
