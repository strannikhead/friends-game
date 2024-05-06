using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMapPlayer : MonoBehaviour
{
    [SerializeField]
    private MapNode startNode;
    private readonly float movingTime = 1f;
    private Vector3 velocity;
    private bool isMoving = false;
    // Start is called before the first frame update
    void Start()
    {
        MapModel.Initialize(Resources
            .FindObjectsOfTypeAll(typeof(MapNode))
            .Select(x => x.GetComponent<MapNode>())
            .ToArray(), startNode);
        MapModel.playerPos.Enable();
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
            if (targetNode != null && targetNode.node.isEnabled && !targetNode.Equals(MapModel.playerPos))
            {
                MapModel.playerPos = targetNode;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!MapModel.playerPos.node.isVisited)
            {
                MapModel.playerPos.LoadThisScene();
            }
        }
        if (gameObject.transform.position != MapModel.playerPos.transform.position && !isMoving)
        {
            isMoving = true;
            StartCoroutine(ChangePosition(MapModel.playerPos.transform.position));
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
        if (MapModel.playerPos.node.isVisited)
        {
            MapModel.playerPos.EnableNeibors();
        }
        MapModel.playerPos.Enable();
        isMoving = false;
    }
}
