using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    private List<Item> items = new List<Item>();
    [SerializeField] private Transform point1, point2;
    [SerializeField] private float collectDelay = 1.0f;

    public void AddItem(Item item)
    {
        if (items.Count >= 2) return;

        if (items.Count == 0)
        {
            items.Add(item);
            MoveItem(item, point1.position);
        }
        else if (items.Count == 1)
        {
            if (item.Type == items[0].Type)
            {
                items.Add(item);
                MoveItem(item, point2.position);
                StartCoroutine(DelayedCollect());
            }
            else
            {
                ThrowItem(item);
            }
        }
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
        item.SetKinematic(false);
    }

    private IEnumerator DelayedCollect()
    {
        yield return new WaitForSeconds(collectDelay);

        foreach (var item in items)
        {
            item.Collect();
        }
        items.Clear();
    }

    private void MoveItem(Item item, Vector3 position)
    {
        item.OnMove(position, Quaternion.identity, 0.2f);
        item.SetKinematic(true);
    }

    private void ThrowItem(Item item)
    {
        item.Force(Vector3.up * 200 + Vector3.forward * 200);
    }
}
