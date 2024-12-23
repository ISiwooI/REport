using System.Collections.Generic;

public class ItemDelegator
{
    public List<Item> items = new List<Item>{
        new ICoffe(),
        new IEnergybar(),
        new IRabitDoll(),
        new ISolution()
    };
    public Item GetItem(ItemKind itemKind)
    {


        switch (itemKind)
        {
            case ItemKind.rabitDoll:
                return new IRabitDoll();
            case ItemKind.solution:
                return new ISolution();
            case ItemKind.coffe:
                return new ICoffe();
            case ItemKind.energybar:
                return new IEnergybar();
            default: return null;

        }


    }
}