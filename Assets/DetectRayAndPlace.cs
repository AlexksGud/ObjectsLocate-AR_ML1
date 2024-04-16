using UnityEngine;

public class DetectRayAndPlace : MonoBehaviour
{

    [SerializeField] private GameObject screenMesh;

    [SerializeField]
    private float minY, maxY, minZ, maxZ;



    private int _id;
    private bool _hasScreen;

    public int ID => _id;
    public bool HasScreen => _hasScreen;

    private void Awake()
    {
        CreateID();

        void CreateID()
        {
            string combinedString = transform.position.x.ToString("0") + transform.position.y.ToString("0") + transform.position.z.ToString("0");

            if (int.TryParse(combinedString, out int combinedInt))
            {
                _id = combinedInt;
            }
            else
            {
                _id = Random.Range(-100, 1000);
            }
        }

    }

    public void ChooseWall(GameObject newScreen)
    {

        SetScreenMesh(newScreen);

        print($"{newScreen} chosen locted");

    }
    public void ForgetWall()
    {
        Destroy(screenMesh);
        screenMesh = null;
    }

    public void SetScreenMesh(GameObject newMesh)
    {

        if (screenMesh != null)
            Destroy(screenMesh);


        screenMesh = newMesh;
        screenMesh.transform.SetParent(transform);
      //screenMesh.transform.localPosition = Vector3.zero;

        screenMesh.transform.localRotation = Quaternion.identity;

    }

    public void SetScreen(float normlY, float normlZ)
    {

        float z = Mathf.Lerp(minZ, maxZ, normlZ);
        float y = Mathf.Lerp(minY, maxY, normlY);
        screenMesh.transform.localPosition = new Vector3(-1, y, z);

    }

    public void SaveScreenOnWall()
    {
        var nextScreen = Instantiate(screenMesh);        
        screenMesh = null;

        SetScreenMesh(nextScreen);


    }


}

