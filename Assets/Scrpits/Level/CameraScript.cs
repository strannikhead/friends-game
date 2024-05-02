using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    private GameObject objectToFollow;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (objectToFollow != null)
        {
            transform.position = new Vector3(objectToFollow.transform.position.x, objectToFollow.transform.position.y, transform.position.z);
        }
    }
}
