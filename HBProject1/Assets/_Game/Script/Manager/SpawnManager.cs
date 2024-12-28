using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private List<Item> itemPrefabs;
    [SerializeField] private Vector3 spawnAreaSize = new Vector3(10, 0, 10);
    private HashSet<Vector3> usedSpawnPositions = new HashSet<Vector3>();
    private HashSet<ItemType> usedItemTypes = new HashSet<ItemType>();

    public List<Item> SpawnItems(int pairCount, Transform parent)
    {
        usedSpawnPositions.Clear();
        usedItemTypes.Clear();
        List<Item> spawnedItems = new List<Item>();

        for (int i = 0; i < pairCount; i++)
        {
            ItemType randomType = GetUniqueRandomItemType();

            for (int j = 0; j < 2; j++)
            {
                Vector3 spawnPosition = GetUniqueSpawnPosition();
                Item newItem = SpawnItem(spawnPosition, randomType, parent);
                spawnedItems.Add(newItem);
            }
        }

        return spawnedItems;
    }

    private ItemType GetUniqueRandomItemType()
    {
        ItemType randomType;
        do
        {
            randomType = (ItemType)Random.Range(1, System.Enum.GetValues(typeof(ItemType)).Length);
        }
        while (usedItemTypes.Contains(randomType));

        usedItemTypes.Add(randomType);
        return randomType;
    }

    private Vector3 GetUniqueSpawnPosition()
    {
        Vector3 spawnPosition;
        do
        {
            spawnPosition = new Vector3(
                Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
                0,
                Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
            ) + transform.position;
        }
        while (usedSpawnPositions.Contains(spawnPosition));

        usedSpawnPositions.Add(spawnPosition);
        return spawnPosition;
    }

    private Item SpawnItem(Vector3 position, ItemType itemType, Transform parent)
    {
        Item randomItemPrefab = itemPrefabs.Find(item => item.Type == itemType);

        if (randomItemPrefab == null)
        {
            Debug.LogError($"No prefab found for item type {itemType}!");
            return null;
        }

        return Instantiate(randomItemPrefab, position, Quaternion.identity, parent);
    }
}
