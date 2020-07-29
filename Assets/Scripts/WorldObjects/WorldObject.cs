using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObject : MonoBehaviour
{
    public virtual bool Interact(WorldObject other_object)
    {
        return false;
    }
}
