public class BalloonFruit : Food
{
    /**
     * Gets eaten by a sheep, which gets converted into a floating sphere.
     */
    protected override bool EatenBySheep (Sheep sheep)
    {
        sheep.MorphIntoBalloon ();
        return true;
    }
}