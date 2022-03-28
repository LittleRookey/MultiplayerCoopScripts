using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Run : StateMachineBehaviour
{
    private float speed = 2.5f;
    Player player;
    
    Rigidbody2D rb;
    Enemy enem;
    EnemyAI enemAI;
    Vector2 offsetRight;
    Vector2 offsetLeft;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
        rb = animator.transform.parent.GetComponent<Rigidbody2D>();
        enem = animator.transform.parent.GetComponent<Enemy>();
        enemAI = animator.transform.parent.GetComponent<EnemyAI>();
        speed = enem.moveSpeed;
        offsetRight = Vector3.right * 5;
        offsetLeft = Vector3.left * 5;
        
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        //enem.Flip(player.transform);
        //Vector2 target = new Vector2(player.transform.position.x, rb.position.y);
        

        if (enem.lookRight)
        {
            rb.velocity += Vector2.right * speed * Time.deltaTime;
        } else
        {
            rb.velocity += Vector2.right * speed * Time.deltaTime;
        }
     

        // TODO if player is within range, attack using animator.settrigger or animator.play
        //if (Vector2.Distance(player.transform.position, rb.position) )
        //Debug.Log((player.transform.position - (Vector3)rb.position).sqrMagnitude + " " + Vector2.Distance(player.transform.position, rb.position));
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
