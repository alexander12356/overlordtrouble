using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveItemAction : MonoBehaviour
{
    [SerializeField]
    List<AddedItem> m_ItemList = null;

    public void Run()
    {
        for (int i = 0; i < m_ItemList.Count; i++)
        {
            PlayerInventory.GetInstance().SetItemCount(m_ItemList[i].id, m_ItemList[i].count);
        }
    }
}
