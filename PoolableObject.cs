using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    private ObjectPool _pool;

    public void InitializePool(ObjectPool pool)
    {
        _pool = pool;
    }

    private void OnDisable()
    {
        if (_pool != null)  _pool.ReturnObject(gameObject);
    } 
}
