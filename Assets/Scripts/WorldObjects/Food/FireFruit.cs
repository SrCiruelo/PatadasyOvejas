
public class FireFruit : Food
{
    /**
     * Gets eaten by a sheep, which suddenly catches fire.
     */
    protected override bool EatenBySheep (Sheep sheep)
    {
        sheep.Burn ();
        return true;
    }
}
