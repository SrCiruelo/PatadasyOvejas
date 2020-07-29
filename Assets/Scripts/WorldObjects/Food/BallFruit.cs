public class BallFruit : Food
{
    /**
     * Gets eaten by a sheep, which gets converted into a sphere.
     */
    protected override bool EatenBySheep (Sheep sheep)
    {
        sheep.MorphIntoBall ();
        return true;
    }
}