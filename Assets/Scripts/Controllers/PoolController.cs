using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class PoolController : MonoBehaviour
{
    #region Variables
    [SerializeField] private List<GameObject> pooledObjectPrefabs;
    [SerializeField] private List<Transform> pooledObjectTransforms;
    [SerializeField] private short pooledObjectAmount;
    [SerializeField] private float disableWaitTime;

    private Pool<GameObject> _pool;

    #endregion

    #region Unity Callbacks

    private void Awake()
    {
        _pool = ObjectPoolExtension.CreateObjectPool(pooledObjectPrefabs, pooledObjectAmount, pooledObjectTransforms);

        for (int i = 0; i < 1; i++) ActivePoolObject();

    }


    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    #endregion


    #region Other Methods

    private void SubscribeEvents()
    {
        EventSignals.Instance.OnDirtClean += StartDeactivateCoroutine;
    }


    private void UnsubscribeEvents()
    {
        EventSignals.Instance.OnDirtClean -= StartDeactivateCoroutine;
    }

    private void StartDeactivateCoroutine(GameObject obj)
    {
        StartCoroutine(DeactivatePoolObject(obj, disableWaitTime));
    }


    private void ActivePoolObject()
    {
        GameObject obj = _pool.GetObjectFromPool();

        obj.TryGetComponent(out SpriteRenderer _spriteRenderer);
        if (_spriteRenderer != null) _spriteRenderer.material.DOFade(1, 0.1f);


        obj.TryGetComponent(out MeshRenderer _meshRenderer);
        if (_meshRenderer != null) _meshRenderer.material.DOFade(1, 0.1f);

        obj.SetActive(true);
    }

    private IEnumerator DeactivatePoolObject(GameObject obj, float waitTime)
    {
        yield return Extensions.GetWaitForSeconds(waitTime);
        obj.SetActive(false);
        _pool.ReturnObjectToPool(obj);
        ActivePoolObject();
    }


    #endregion




}
