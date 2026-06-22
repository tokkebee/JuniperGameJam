using UnityEngine;
using System.Collections.Generic;

// This script handles the player's web placement.
public class WebSpinner : MonoBehaviour
{
    [SerializeField] private GameObject silkPrefab;

    public void StartSilk(Vector3 startPosition, Transform endPosition) {
        LineRenderer lr = silkPrefab.GetComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.useWorldSpace = true;

        lr.SetPosition(0, silkPrefab.transform.position);
    }

    // [SerializeField] private GameObject silkPrefab;
    // private List<SilkInstance> web = new();

    // public class SilkInstance {
    //     public LineRenderer lr;
    //     public Vector3 start;
    //     public Vector3 end;
    // }

    // public void CreateSilk(Vector3 start, Vector3 end) {
    //     GameObject obj = Instantiate(silkPrefab);

    //     LineRenderer lr = obj.GetComponent<LineRenderer>();
    //     lr.positionCount = 2;
    //     lr.useWorldSpace = true;

    //     web.Add(new SilkInstance {
    //         lr = lr,
    //         start = start,
    //         end = end
    //     });
    // }
}
