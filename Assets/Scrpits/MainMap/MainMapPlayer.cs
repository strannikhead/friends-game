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
    [SerializeField]
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
        if (isMoving)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            // review(26.05.2024): Rider мне подсказывает, что лучше инициализировать камеру в Start и переиспользовать поле (особенно если камера всегда одна)
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
            // review(26.05.2024): Может, инкапсулировать логику с загрузкой сцены в MapModel?
            if (!MapModel.playerPos.node.isVisited)
            {
                MapModel.playerPos.LoadThisScene();
            }
        }
        if ((gameObject.transform.position - MapModel.playerPos.transform.position).magnitude >= 10e-2)
        {
            isMoving = true;
            StartCoroutine(ChangePosition(MapModel.playerPos.transform.position));
        }
    }


    private void FixedUpdate()
    {
        // review(26.05.2024): Может быть, делать это только если velocity != Vector3.zero? Чтобы зря transform не изменять и не запрашивать. Это может быть затратным
        transform.position += velocity * Time.deltaTime;
    }

    private IEnumerator ChangePosition(Vector3 newPosition)
    {
        velocity = (newPosition - transform.position) / movingTime;
        yield return new WaitForSeconds(movingTime);
        velocity = Vector3.zero;
        // review(26.05.2024): Опять же я бы эту логику инкапсулировал в MapModel
        if (MapModel.playerPos.node.isVisited)
        {
            MapModel.playerPos.EnableNeibors();
        }
        MapModel.playerPos.Enable();
        isMoving = false;
    }
}
