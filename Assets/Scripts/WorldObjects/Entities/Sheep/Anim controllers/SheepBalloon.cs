using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepBalloon : StateMachineBehaviour
{
    GameObject target;

    float time_to_pass;
    float time_passed;
    Sheep sheep;
    Sheep_StateVars sheep_vars;
    bool triggered;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        sheep = animator.GetComponent<Sheep>();
        sheep_vars = sheep.GetComponent<Sheep_StateVars>();
        time_to_pass = sheep_vars.sheep_floating_time;
        time_passed = 0;
        triggered = false;
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (triggered)
        {
            return;
        }


        if (time_passed > time_to_pass)
        {
            animator.SetTrigger("FruitFinished");
            triggered = true;
        }
        if (Physics.Raycast(sheep.transform.position, Vector3.down, sheep_vars.raycast_to_float_dist))
        {
            sheep_vars.rb.AddForce(Vector3.up * sheep_vars.floating_gravity_scale, ForceMode.Impulse);
        }
        time_passed += Time.deltaTime;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
