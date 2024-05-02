using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMapPlayer : MonoBehaviour
{
    [SerializeField]
    public MapNode location;
    private readonly float movingTime = 1f;
    private Vector3 velocity;
    // Start is called before the first frame update
    void Start()
    {
        location.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var targetNode = Physics2D.OverlapPointAll(mousePos)
                .Select(obj => obj.gameObject.GetComponent<MapNode>())
                .Where(obj => obj != null)
                .FirstOrDefault();
            if (targetNode != null && targetNode.isEnabled && !targetNode.Equals(location))
            {
                location = targetNode;
                StartCoroutine(ChangePosition(location.transform.position));
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!location.isVisited)
            {
                location.LoadThisScene();
            }
        }
    }


    private void FixedUpdate()
    {
        transform.position += velocity * Time.deltaTime;
    }

    private IEnumerator ChangePosition(Vector3 newPosition)
    {
        velocity = (newPosition - transform.position) / movingTime;
        yield return new WaitForSeconds(movingTime);
        velocity = Vector3.zero;
        if (location.isVisited)
        {
            location.EnableNeibors();
        }
        location.Enable();
    }
}
