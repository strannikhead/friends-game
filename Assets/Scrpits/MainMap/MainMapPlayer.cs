using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMapPlayer : MonoBehaviour
{
    [SerializeField]
    private MapNode startNode;
    private Camera mainCamera;
    private readonly float movingTime = 1f;
    private Vector3 velocity;
    [SerializeField]
    private bool isMoving = false;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        MapModel.Initialize(Resources
            .FindObjectsOfTypeAll(typeof(MapNode))
            .Select(x => x.GetComponent<MapNode>())
            .ToArray(), startNode);
        MapModel.PlayerPosition.Enable();
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
            //(29.05.2024) refactored // review(26.05.2024): Rider мне подсказывает, что лучше инициализировать камеру в Start и переиспользовать поле (особенно если камера всегда одна)
            var mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            var targetNode = Physics2D.OverlapPointAll(mousePos)
                .Select(obj => obj.gameObject.GetComponent<MapNode>())
                .Where(obj => obj != null)
                .FirstOrDefault();
            if (targetNode != null && targetNode.node.isEnabled && !targetNode.Equals(MapModel.PlayerPosition))
            {
                MapModel.PlayerPosition = targetNode;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //(29.05.2024) refactored // review(26.05.2024): Может, инкапсулировать логику с загрузкой сцены в MapModel?

            MapModel.TryLoadCurrentScene();
        }
        if ((gameObject.transform.position - MapModel.PlayerPosition.transform.position).magnitude >= 10e-2)
        {
            isMoving = true;
            StartCoroutine(ChangePosition(MapModel.PlayerPosition.transform.position));
        }
    }


    private void FixedUpdate()
    {
        //(29.05.2024) refactored // review(26.05.2024): Может быть, делать это только если velocity != Vector3.zero? Чтобы зря transform не изменять и не запрашивать. Это может быть затратным
        if (velocity != Vector3.zero)
        {
            transform.position += velocity * Time.deltaTime;
        }
    }

    private IEnumerator ChangePosition(Vector3 newPosition)
    {
        velocity = (newPosition - transform.position) / movingTime;
        yield return new WaitForSeconds(movingTime);
        velocity = Vector3.zero;
        //(29.05.2024) refactored // review(26.05.2024): Опять же я бы эту логику инкапсулировал в MapModel
        MapModel.EnableCurrentAndNeibors();
        isMoving = false;
    }
}
