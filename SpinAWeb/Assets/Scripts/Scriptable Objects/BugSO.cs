using UnityEngine;

[CreateAssetMenu(fileName = "BugSO", menuName = "Scriptable Objects/BugSO")]
public class BugSO : ScriptableObject
{
    //FEEL FREE TO ADJUST POINTS VALUES IN THE INSPECTOR AS NEEDED
    public int pointsValue;
    public int webRecharge;
    public Sprite caughtImage; 
}
