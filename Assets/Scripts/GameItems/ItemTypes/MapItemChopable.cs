public class MapItemChopable : MapItemBase, IChopable
{
    public void Chop(int damage)
    {
        print("chop with damage " + damage);
        mapItem.healthPoints -= damage;
        if (mapItem.healthPoints <= 0)
        {
            print("item chopped");
            MapItemDone();
        }
    }
}
