using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteFallenObjects : MonoBehaviour
{
    private GameStatus _commbus;
    
    // Start is called before the first frame update
    void Start()
    {
        _commbus = Instances.GetGameStatus();
    }

    private void OnTriggerEnter (Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Player":
                Debug.Log ("You died :(");
                break;
            
            case "Sheep":
                Destroy (other.gameObject);
                _commbus.sheepRemoved (false);
                break;
            
            case "Wolf":
                Destroy (other.gameObject);
                _commbus.wolfRemoved();
                break;
            
//            default:
//                Destroy (other.gameObject);
//                break;
        }
    }

}
