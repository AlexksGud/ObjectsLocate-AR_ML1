using UnityEngine;

public class TV : MonoBehaviour
{
    [SerializeField] private Collider boxCol;
    private bool located;

    public bool Located
    {
        get { return located; }
        set
        {
            located = value;

            if (value)
                boxCol.enabled = true;
            else
                boxCol.enabled = false;
        }
    }
}
