public class MapItemPickable : MapItemBase, IPickable
{
    public void Pick()
    {
        MapItemHarvestingDone();
    }
}