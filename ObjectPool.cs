using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    private readonly Queue<GameObject> _pool = new();
    public static ObjectPool Instance { get; private set; }

    public void Awake()
    {
        Instance = this;
        for (var i = 0; i < 7; i++)
        {
            var obj = Instantiate(prefab, transform);
            obj.SetActive(false);
            obj.GetComponent<PoolableObject>().InitializePool(this);
            _pool.Enqueue(obj);
        }

    }
    
    public GameObject GetObject(Vector3 spawnPoint)
    {
           var obj = _pool.Dequeue();
        obj.transform.position = spawnPoint; 
         obj.SetActive(true);
         
       
         // NavMeshAgent-i sıfırla
         var agent = obj.GetComponent<NavMeshAgent>();
         if (!agent) return obj;
         agent.ResetPath(); // əvvəlki path-i təmizlə
         agent.enabled = true; // əmin ol ki aktivdir
        
         
         return obj;
    }

    public void ReturnObject(GameObject obj)
    {
       // obj.SetActive(false); ***** INSIDE THE POOLABLE OBJECT *****
        obj.transform.SetLocalPositionAndRotation(transform.position, Quaternion.identity);
        _pool.Enqueue(obj);
        
    }
}