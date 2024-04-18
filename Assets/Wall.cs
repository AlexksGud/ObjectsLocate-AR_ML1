using UnityEngine;

public class Wall : MonoBehaviour
{

    public float televisionRotationAngle = 0f;
    public XorZWallFixedCoor xorZWallFixedCoor;

    private int _id;
    public int ID => _id;

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

    public void ForgetWall() 
    {

    }

    

}
public enum XorZWallFixedCoor 
{
    X,
    Z
}
