using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class WebManager : MonoBehaviour
{
    [Header("Silk")]
    [SerializeField] private GameObject silkPrefab;
    [SerializeField] public Transform spinnerets;
    [SerializeField] private Image silkMeter;
    [SerializeField] private float maxSilk = 100f;
    private float silkConsumed = 0f;
    //[SerializeField] private float silkRemaining;

    [Header("Active")]
    [SerializeField] public GameObject currentSilk;
    private LineRenderer lr;
    private EdgeCollider2D ec;

    private Vector3 startPoint;
    // private Vector3 lastPoint;

    // [SerializeField] private float silkPathLength;

    private float supportRadius = 0.5f;

    [Header("Web")]
    [SerializeField] private GameObject web;
    [SerializeField] public List<GameObject> placedSilks = new();

    [Header("Layers")]
    [SerializeField] private LayerMask branches;
    [SerializeField] private LayerMask silk;
    [SerializeField] private LayerMask leaves;
    public bool silkActive { get; private set; }

    void Start() {
        UpdateSilkMeter();
        if (silkMeter != null) silkMeter.fillAmount = 1f;
    }

//silk creation
    //instantiates line of silk
    public bool StartSilk() {
        if (silkActive || GetSilkRemaining() <= 0f || !ValidSilk()) return false;

        currentSilk = Instantiate(silkPrefab, spinnerets.position, Quaternion.identity);
        lr = currentSilk.GetComponent<LineRenderer>();
        ec = currentSilk.GetComponent<EdgeCollider2D>();

        silkActive = true;

        startPoint = spinnerets.position;
        //lastPoint = startPoint;

        lr.positionCount = 2;
        lr.useWorldSpace = true;
        ec.edgeRadius = supportRadius;

        lr.SetPosition(0, startPoint);
        lr.SetPosition(1, startPoint);

        ec.SetPoints(new List<Vector2> {Vector2.zero, Vector2.zero}); //edge collider uses local space

        return true;
    }

    //updates position of a silk's last vertex
    public void UpdateSilk() {
        if (!silkActive) return;

        Vector3 currentPoint = spinnerets.position;

        //lr (world space)
        lr.SetPosition(0, startPoint);
        lr.SetPosition(lr.positionCount - 1, currentPoint);

        //ec (local space)
        Vector2 localStart = currentSilk.transform.InverseTransformPoint(startPoint);
        Vector2 localEnd = currentSilk.transform.InverseTransformPoint(currentPoint);

        ec.SetPoints(new List<Vector2> {
            localStart,
            localEnd
        });

        UpdateSilkMeter();
    }

    public void EndSilk(bool refund = false) {
        if (!silkActive) return;

        UpdateSilk();

        if (currentSilk != null) {
            if (refund) {
                Destroy(currentSilk);
            }
            else if (ValidSilk()) {
                silkConsumed += Vector3.Distance(startPoint, spinnerets.position);
                currentSilk.transform.SetParent(web.transform, true);
                placedSilks.Add(currentSilk);
            }
            else {
                //Debug.Log("Invalid silk placement!");
                Destroy(currentSilk);
            }
        }
        currentSilk = null;
        silkActive = false;
        UpdateSilkMeter();
    }

    public float GetSilkRemaining() {
        float currentLength = silkActive
            ? Vector3.Distance(startPoint, spinnerets.position)
            : 0f;

        float totalUsed = silkConsumed + currentLength;
        return Mathf.Clamp01(1f - (totalUsed / maxSilk));
    }

    public void UpdateSilkMeter() {
        if (silkMeter != null) {
            silkMeter.fillAmount = GetSilkRemaining();
        }
    }

    public bool SpinneretsOverLeaves(LayerMask leaves) {
        return Physics2D.OverlapCircle(spinnerets.position, supportRadius, leaves);
    }

    public bool ValidSilk() {
        LayerMask anchorableLayers = branches | silk;

        if (Physics2D.OverlapCircle(spinnerets.position, supportRadius, branches))
            return true;

        foreach (var silkObj in placedSilks) {
            if (silkObj == null) continue;

            var lr = silkObj.GetComponent<LineRenderer>();
            if (lr == null) continue;

            Vector3 a = lr.GetPosition(0);
            Vector3 b = lr.GetPosition(1);

            float distance = DistancePointToSegment(spinnerets.position, a, b);

            if (distance < supportRadius)
                return true;
        }
        return false;
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