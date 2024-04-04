using System.Collections.Generic;
using UnityEngine;

public static class ObjectPoolExtension
{
    public static Pool<T> CreateObjectPool<T>(this List<T> prefabs, int initialSize, List<Transform> parents = null) where T : Component
    {
        List<int> indexes = new();
        return new Pool<T>(() =>
        {
            int randomPrefabIndex = Random.Range(0, prefabs.Count);
            int randomParentIndex = Extensions.GetUniqueRandomIndex(indexes, parents.Count);
            T newObj = GameObject.Instantiate(prefabs[randomPrefabIndex], parents[randomParentIndex]);
            newObj.gameObject.transform.localPosition = Vector3.zero;
            newObj.gameObject.SetActive(false);
            if (parents != null) newObj.transform.SetParent(parents[randomParentIndex]);
            return newObj;

        }, initialSize);
    }


    public static Pool<GameObject> CreateObjectPool(this List<GameObject> prefabs, int initialSize, List<Transform> parents = null)
    {
        List<int> indexes = new();
        return new Pool<GameObject>(() =>
        {
            int randomPrefabIndex = Random.Range(0, prefabs.Count);
            int randomParentIndex = Extensions.GetUniqueRandomIndex(indexes, parents.Count);
            GameObject newObj = GameObject.Instantiate(prefabs[randomPrefabIndex], parents[randomParentIndex]);
            newObj.transform.localPosition = Vector3.zero;
            newObj.SetActive(false);
            if (parents != null) newObj.transform.SetParent(parents[randomParentIndex]);
            return newObj;

        }, initialSize);

    }

}