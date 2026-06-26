using UnityEngine;
using System.Collections.Generic;

public class WebManager : MonoBehaviour
{
    [Header("Silk")]
    [SerializeField] private GameObject silkPrefab;
    [SerializeField] private Transform spinnerets;
    [SerializeField] private float silkTotal = 100f;
    [SerializeField] private float supportRadius = 0.5f;

    [Header("Web")]
    [SerializeField] private GameObject web;
    [SerializeField] public List<GameObject> placedSilks = new();
    public int caughtBugs; 

    [Header("Layers")]
    [SerializeField] private LayerMask branches;
    [SerializeField] private LayerMask silk;

    [Header("Active")]
    public GameObject currentSilk;
    private LineRenderer lr;
    private EdgeCollider2D ec;
    
    [SerializeField] public bool silkActive = false;
    private Vector3 startWorld;

    //instantiates line of silk
    public void StartSilk() {
        if (silkActive) return;

        currentSilk = Instantiate(silkPrefab, spinnerets.position, Quaternion.identity);
        silkActive = true;

        lr = currentSilk.GetComponent<LineRenderer>();
        ec = currentSilk.GetComponent<EdgeCollider2D>();

        lr.positionCount = 2;
        lr.useWorldSpace = true;
        ec.edgeRadius = supportRadius;

        startWorld = spinnerets.position;

        lr.SetPosition(0, startWorld);
        lr.SetPosition(1, startWorld);

        ec.SetPoints(new List<Vector2> {Vector2.zero, Vector2.zero}); //edge collider uses local space

        // if (!ValidSilk()) {
        //     Debug.Log("Invalid silk placement!");
        //     silkActive = false;
        //     Destroy(currentSilk);
        // }
    }

    //updates position of a silk's last vertex
    public void UpdateSilk() {
        if (!silkActive) return;

        Vector3 endWorld = spinnerets.position;

        //lr (world space)
        lr.SetPosition(0, startWorld);
        lr.SetPosition(lr.positionCount - 1, endWorld);

        //ec (local space)
        Vector2 localStart = currentSilk.transform.InverseTransformPoint(startWorld);
        Vector2 localEnd = currentSilk.transform.InverseTransformPoint(endWorld);

        ec.SetPoints(new List<Vector2> {
            localStart,
            localEnd
        });
    }

    public void EndSilk() {
        if (!silkActive) return;

        UpdateSilk();

        if (ValidSilk()) {
            silkTotal -= Vector3.Distance(startWorld, spinnerets.position);
            currentSilk.transform.SetParent(web.transform, true);
            placedSilks.Add(currentSilk);
        }
        else {
            //Debug.Log("Invalid silk placement!");
            Destroy(currentSilk);
        }

        silkActive = false;
    }

    public bool ValidSilk() {
        LayerMask supportLayers = branches | silk;

        if (Physics2D.OverlapCircle(spinnerets.position, supportRadius, branches))
            return true;

        foreach (var silkObj in placedSilks) {
            if (silkObj == null) continue;

            var lr = silkObj.GetComponent<LineRenderer>();
            if (lr == null) continue;

            Vector3 a = lr.GetPosition(0);
            Vector3 b = lr.GetPosition(1);

            Vector3 p = spinnerets.position;

            float distance = DistancePointToSegment(p, a, b);

            if (distance < supportRadius)
                return true;
        }

        // Collider2D[] hits = Physics2D.OverlapCircleAll(
        //     spinnerets.position,
        //     0.5f,
        //     supportLayers
        // );

        // foreach (var hit in hits)
        // {
        //     if (hit.transform.IsChildOf(currentSilk.transform))
        //         continue;

        //     if (hit.gameObject == currentSilk)
        //         continue;

        //     if (((1 << hit.gameObject.layer) & supportLayers) != 0) {
        //         return true;
        //     }
        // }

        return false;

        // Collider2D hit = Physics2D.OverlapPoint(spinnerets.position, supportLayers);
        // return hit != null;
    }

    private float DistancePointToSegment(Vector3 p, Vector3 a, Vector3 b) {
        Vector3 ab = b - a;
        Vector3 ap = p - a;

        float t = Vector3.Dot(ap, ab) / Vector3.Dot(ab, ab);
        t = Mathf.Clamp01(t);

        Vector3 closest = a + ab * t;
        return Vector3.Distance(p, closest);
    }
}