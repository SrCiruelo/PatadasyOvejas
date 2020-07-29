using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : WorldObject
{
    /**
     * Gets eaten by another world object.
     */
    public override bool Interact (WorldObject other_object)
    {
        /* We only want to interact with sheep */
        if (other_object.GetType () == typeof (Sheep))
        {
            Sheep sheep = (Sheep) other_object;
            return EatenBySheep (sheep);
        }

        return false;
    }

    /**
     * Gets eaten by a sheep.
     */
    protected virtual bool EatenBySheep (Sheep sheep)
    {
        return true;
    }
}
