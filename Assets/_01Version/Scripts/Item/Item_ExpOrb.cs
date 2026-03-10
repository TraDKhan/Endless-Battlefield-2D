public class Item_ExpOrb : PooledItem
{
    public int expAmount = 1;

    protected override void OnCollected()
    {
        //PlayerExpSystem.Instance.AddExp(expAmount);
    }
}
