using System.Diagnostics;

public class Items
{
    public enum ItemType 
    {
        DoubleJump,
        Swing,
        FireBall,
        SowrdBuff
    }

    public static int GetCost(ItemType itemType)
    {
        switch (itemType)
        {
            default:
            
            case ItemType.DoubleJump:
                return 3;
                
            case ItemType.Swing:
                return 8;
                
            case ItemType.FireBall:
                return 999;
                
            case ItemType.SowrdBuff:
                return 5;
        }
    }
}
