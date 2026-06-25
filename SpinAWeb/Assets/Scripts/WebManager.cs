using UnityEngine;
using System.Collections.Generic;

public class WebManager : MonoBehaviour
{
    [Header("Silk")]
    [SerializeField] private GameObject silkPrefab;
    [SerializeField] private Transform spinnerets;
    [SerializeField] private float silkTotal = 100f;

    [Header("Web")]
    [SerializeField] private GameObject web;

    [Header("Branches Layer")]
    [SerializeField] private LayerMask branches;

    [Header("Active")]
    private GameObject currentSilk;
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

        if (ValidSilk()) {
            silkTotal -= Vector3.Distance(startWorld, spinnerets.position);
            currentSilk.transform.SetParent(web.transform, true);
        }
        else {
            //Debug.Log("Invalid silk placement!");
            Destroy(currentSilk);
        }

        silkActive = false;
    }

    // public bool ToggleSilkActive() {
    //     silkActive = !silkActive;
    //     return silkActive;
    // }

    public bool ValidSilk() {
        return Physics2D.OverlapCircle(
            spinnerets.position,
            0.1f,
            branches
        );

        //return hit != null;
    }
}