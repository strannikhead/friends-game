using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    private GameObject objectToFollow;
    private float zPosition;

    // Start is called before the first frame update
    void Start()
    {
        zPosition = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (objectToFollow != null)
        {
            var objectPosition = objectToFollow.transform.position;
            //(29.05.2024) refactored //review(26.05.2024): Стоит выносить transform в переменную, т.к. его получение - неэфффективная операция
            transform.position = new Vector3(objectPosition.x, objectPosition.y, zPosition);
        }
    }
}
