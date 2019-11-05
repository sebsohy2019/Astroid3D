using UnityEngine;

public class ShotScript : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {


    }



    public void Shoot(int type)
    {
        if (type == 1)
        {
            GameObject go = Instantiate(LogicController.lc.prefabDict["playerLaser"], transform.position, transform.rotation);

        }
        else
        {
            GameObject go = Instantiate(LogicController.lc.prefabDict["alienLaser"], transform.position, transform.rotation);

        }
    }
}
