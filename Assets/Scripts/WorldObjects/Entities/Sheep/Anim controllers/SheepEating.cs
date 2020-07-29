using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepEating : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    Sheep sheep;
    Sheep_StateVars sheep_vars;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        sheep = animator.GetComponent<Sheep>();
        sheep_vars = sheep.GetComponent<Sheep_StateVars>();

        /* Depending on the fruit type, the effect will be different */
        sheep_vars.current_chasing_food.Interact (sheep);
        
        /* The fruit has been eaten, so it should be removed from the map */
        Food food = sheep_vars.current_chasing_food;

        if (food)
        {
            Destroy (sheep_vars.current_chasing_food.gameObject);
            sheep_vars.current_chasing_food = null;
        }
    }

}
