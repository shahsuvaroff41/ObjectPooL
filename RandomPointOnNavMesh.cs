// RandomPointOnNavMesh

using UnityEngine;
using UnityEngine.AI;
// No USAGE ************************************************************************************************
public class RandomPointOnNavMesh : MonoBehaviour
{
    public float range = 10.0f;

   private static bool RandomPoint(Vector3 center, float rangE, out Vector3 result)
    {
        for (var i = 0; i < 30; i++)
        {
            var randomPoint = center + Random.insideUnitSphere * rangE;

            if (!NavMesh.SamplePosition(randomPoint, out var hit, 1.0f, NavMesh.AllAreas)) continue;
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    private void Update()
    {
        if (RandomPoint(transform.position, range, out var point))
        {
            Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
        }
    }
}