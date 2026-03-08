using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Spawn : MonoBehaviour
{
    private Vector3 _point;
    private NavMeshTriangulation _triangulation;

    private void Awake()
    {
        _triangulation = NavMesh.CalculateTriangulation();
    }

    private void RandomPointOnNavMesh_Triangulation()
    {
        if (_triangulation.vertices.Length == 0)
        {
            _point = transform.position;
            return;
        }

        //  Random triangle 
        var triIndex =
            Random.Range(0, _triangulation.indices.Length / 3) * 3;

        var v0 = _triangulation.vertices[_triangulation.indices[triIndex]];
        var v1 = _triangulation.vertices[_triangulation.indices[triIndex + 1]];
        var v2 = _triangulation.vertices[_triangulation.indices[triIndex + 2]];

        // Random position in current triangle 
        var r1 = Random.value;
        var r2 = Random.value;

        if (r1 + r2 > 1f)
        {
            r1 = 1f - r1;
            r2 = 1f - r2;
        }

        var randomPoint =
            v0 + r1 * (v1 - v0) + r2 * (v2 - v0);

        // Safety check 
        _point = NavMesh.SamplePosition(randomPoint, out var hit, 0.5f, NavMesh.AllAreas) ? hit.position : transform.position;
    }

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            RandomPointOnNavMesh_Triangulation();

            var obj = ObjectPool.Instance.GetObject(_point);

            StartCoroutine(DespawnAfterTime(obj, 10f));

            yield return new WaitForSeconds(2f);
        }
    }

    private IEnumerator DespawnAfterTime(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
    }
}
