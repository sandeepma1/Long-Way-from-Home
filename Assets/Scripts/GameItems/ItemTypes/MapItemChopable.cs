public class MapItemChopable : MapItemBase, IChopable
{
    public void Chop(int damage)
    {
        GEM.PrintDebug("chop with damage " + damage);
        mapItem.healthPoints -= damage;
        if (mapItem.healthPoints <= 0)
        {
            GEM.PrintDebug("item chopped");
            MapItemHarvestingDone();
        }
    }
}
