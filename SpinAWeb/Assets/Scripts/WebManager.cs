using UnityEngine;

public class WebManager : MonoBehaviour
{
    [Header("Silk")]
    [SerializeField] private GameObject silkPrefab;
    [SerializeField] private Transform spinnerets;
    [SerializeField] private float silkTotal = 100f;

    [Header("Active")]
    [SerializeField] public bool silkActive = false;
    private GameObject currentSilk;
    private LineRenderer lr;

    [Header("Web")]
    [SerializeField] private GameObject web;

    [Header("Branches")]
    [SerializeField] private LayerMask branches;

    //instantiates line of silk
    public void StartSilk() {
        currentSilk = Instantiate(silkPrefab, spinnerets);
        silkActive = true;

        lr = currentSilk.GetComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.useWorldSpace = true;

        lr.SetPosition(0, spinnerets.transform.position);

        if (!ValidSilk()) {
            Debug.Log("Invalid silk placement!");
            silkActive = false;
            Destroy(currentSilk);
        }
    }

    //updates position of a silk's last vertex
    public void UpdateSilk() {
        if (silkActive) {
            lr.SetPosition(lr.positionCount - 1, spinnerets.position);
        }
    }

    // public LineRenderer getCurrentSilkLR() {
    //     return currentSilk.GetComponent<LineRenderer>();
    // }

    public void EndSilk() {
        // float silkLength = Vector3.Distance(lr.GetPosition(0), lr.GetPosition(lr.positionCount - 1));
        // if ((silkTotal - silkLength) >= 0) {
        //     silkActive = false;
        // }
        // else {

        // }

        if (ValidSilk()) {
            silkTotal -= Vector3.Distance(lr.GetPosition(0), lr.GetPosition(lr.positionCount - 1));
            currentSilk.transform.SetParent(web.transform, false);
            silkActive = false;
        }
        else {
            Debug.Log("Invalid silk placement!");
            Destroy(currentSilk);
        }
    }

    public bool ToggleSilkActive() {
        silkActive = !silkActive;
        return silkActive;
    }

    public bool ValidSilk() {
        //raycast apparently doesnt work well for this scenario
        // Debug.Log("checking silk validity");
        // //return Physics.Raycast(spinnerets.position, transform.TransformDirection(Vector3.forward), 1, branches);
        // bool valid = Physics2D.Raycast(spinnerets.position, transform.TransformDirection(Vector3.forward), 1, branches);
        // if (valid) {
        //     Debug.Log("Valid silk");
        // }
        // else {
        //     Debug.Log("Invalid silk");
        // }
        // return valid;

        Collider2D hit = Physics2D.OverlapCircle(
            spinnerets.position,
            0.1f,
            branches
        );

        return hit != null;
    }
}