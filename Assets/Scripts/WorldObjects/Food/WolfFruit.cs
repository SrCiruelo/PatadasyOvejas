public class WolfFruit : Food
{
    /**
     * Gets eaten by a sheep, which morphs into a wolf.
     */
    protected override bool EatenBySheep (Sheep sheep)
    {
        sheep.MorphIntoWolf ();
        return true;
    }
}