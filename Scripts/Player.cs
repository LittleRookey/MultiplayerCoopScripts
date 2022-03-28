using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LittleRookey.Character;

public enum Class 
{
    Tanker, Warrior, Magician, Supporter
}
public class Player : Character
{


    private void Start()
    {
        anim = GetComponent<Animator>();

    }

    //private void Update()
    //{
    //    StateMachine.CurrentState.LogicUpdate();


    //}

    //private void FixedUpdate()
    //{
    //    StateMachine.CurrentState.PhysicsUpdate();
    //}
}
