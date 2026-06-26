using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class WebManager : MonoBehaviour
{
    [Header("Silk")]
    [SerializeField] private GameObject silkPrefab;
    [SerializeField] private Transform spinnerets;
    [SerializeField] private Image silkMeter;
    [SerializeField] private float maxSilk = 100f;

    [Header("Active")]
    [SerializeField] public GameObject currentSilk;
    private LineRenderer lr;
    private EdgeCollider2D ec;

    private Vector3 lastPoint;
    private Vector3 startPoint;

    private float silkRemaining;
    private float silkUsedThisLine;

    private float supportRadius = 0.5f;

    [Header("Web")]
    [SerializeField] private GameObject web;
    [SerializeField] public List<GameObject> placedSilks = new();
    public int caughtBugs; 

    [Header("Layers")]
    [SerializeField] private LayerMask branches;
    [SerializeField] private LayerMask silk;
    
    public bool silkActive { get; private set; }

    void Start() {
        silkRemaining = maxSilk;
        if (silkMeter != null) silkMeter.fillAmount = 1f;
    }

    //instantiates line of silk
    public bool StartSilk() {
        if (silkActive || silkRemaining <= 0f) return false;

        currentSilk = Instantiate(silkPrefab, spinnerets.position, Quaternion.identity);
        lr = currentSilk.GetComponent<LineRenderer>();
        ec = currentSilk.GetComponent<EdgeCollider2D>();

        silkActive = true;

        startPoint = spinnerets.position;
        lastPoint = startPoint;
        silkUsedThisLine = 0f;

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
        //if (silkRemaining <= 0f) return;

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

        float delta = Vector3.Distance(lastPoint, currentPoint);
        ConsumeSilk(delta);
        silkUsedThisLine += delta;

        lastPoint = currentPoint;

        if (silkRemaining <= 0f) {
            EndSilk(true);
        }

        //UpdateSilkMeter();
    }

    public void EndSilk(bool refundUnused = false) {
        if (!silkActive) return;

        //UpdateSilk();

        if (refundUnused) {
            silkRemaining = Mathf.Clamp(silkRemaining + silkUsedThisLine, 0f, maxSilk);
        }

        if (currentSilk != null) {
            if (ValidSilk()) {
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
    }

    public float GetSilkRemaining() => silkRemaining;

    public void ConsumeSilk(float amount) {
        if (!silkActive) return;
        
        //silkRemaining -= amount;
        silkRemaining = Mathf.Max(0f, silkRemaining - amount);

        if (silkMeter != null)
            silkMeter.fillAmount = silkRemaining / maxSilk;
    }

    // public void UpdateSilkMeter() {
    //     float usedSilk = Vector3.Distance(previousEnd, spinnerets.position);

    //     silkRemaining -= usedSilk;
    //     silkRemaining = Mathf.Max(0f, silkRemaining);

    //     silkMeter.fillAmount = silkRemaining / maxSilk;

    //     previousEnd = spinnerets.position;
    // }

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