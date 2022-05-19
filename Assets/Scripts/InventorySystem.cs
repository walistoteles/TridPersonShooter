using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{

    public List<ItemData> Items = new List<ItemData>();
    public int[] slots;
   
    public async Task<bool> AddItem(ItemData item, int stack)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if(slots[i] == 0)
            {

                Items.Add(item);

                return true;
            }
        }

        return false;
    }
}
