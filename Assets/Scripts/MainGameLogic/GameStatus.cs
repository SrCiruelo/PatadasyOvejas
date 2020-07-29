using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatus : MonoBehaviour
{
    /**
     * Number of sheep inside the pen.
     */
    public int sheep_in;

    /**
     * Number of sheep inside of the pen.
     */
    public int sheep_out { get; private set; }

    /**
     * Number of wolves in the map.
     */
    public int wolves;


    /**
     * Only allowed instance of this object
     */
    private static GameStatus _instance;

    /* ================================================================================== */
    /* Update methods, to be called by other classes who want to interact with the status */
    /* ================================================================================== */

    /* ====== */
    /* WOLVES */
    /* ====== */
    
    /**
     * Informs this bus that there's a new wolf in the game.
     *
     * <returns>
     * The new value of <see cref="wolves"/>
     * </returns>
     */
    public int wolfAdded () { return ++wolves; }
    
    /**
     * Informs this bus that a wolf just disappeared from the game.
     *
     * <returns>
     * The new value of <see cref="wolves"/>
     * </returns>
     */
    public int wolfRemoved () { return --wolves; }
    
    /* ===== */
    /* SHEEP */
    /* ===== */

    /**
     * Informs this bus that there's a new sheep on the world.
     * This new sheep is presumed to be inside the pen.
     *
     * To add a sheep outside of the pen, the method <<see cref="sheepEscaped()"/> should be called.
     *
     * <returns>
     * The new value of <see cref="sheep_in"/>
     * </returns>
     */
    public int sheepAdded () { return ++sheep_in; }
    
    /**
     * Informs this bus that one of the sheep escaped from the pen.
     *
     * <returns>
     * The new value of <see cref="sheep_out"/>
     * </returns>
     */
    public int sheepEscaped () { sheep_in--; return ++sheep_out; }

    /**
     * Informs this bus that one of the sheep is back again inside of the pen.
     *
     * <returns>
     * The new value of <see cref="sheep_in"/>
     * </returns>
     */
    public int sheepInPen () { sheep_out--; return ++sheep_in; }


    /**
     * Informs the bus that one of the sheep was completely removed from the game.
     * It's assumed that a sheep can only be deleted if it's outside of the pen-
     *
     * <param name="inside">
     * If 'true', specifies that the sheep was inside of the pen when was removed from the game.
     * </param>
     *
     * 
     * <returns>
     * The new number of sheep (<see cref="sheep_out"/> + <see cref="sheep_in"/>)
     * </returns>
     */
    public int sheepRemoved (bool inside) { return inside? --sheep_in : --sheep_out; }

}
