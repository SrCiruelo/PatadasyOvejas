using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepWolf : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Sheep sheep = animator.GetComponent<Sheep>();
        
        SpawnManager manager = Instances.GetSpawnManager();
        GameStatus game = Instances.GetGameStatus ();

        Vector3 position = sheep.transform.position;
        
        Destroy (sheep.gameObject);
        game.sheepRemoved (false);

        Instantiate (manager.wolfPrefab, position, Quaternion.identity);
        game.wolfAdded ();
    }

}
