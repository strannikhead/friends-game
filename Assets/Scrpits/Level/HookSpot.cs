using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class HookSpot : MonoBehaviour
{
    public bool isHookable = false;
    private bool isOnCooldown = false;
    [SerializeField]
    private Player player;
    [SerializeField]
    private GameObject activeSpot;
    private GameObject instanciatedSpot;
    [SerializeField]
    private float hookRange = 5;
    [SerializeField]
    private float cooldown = 2;
    private readonly HashSet<string> blockSightTags = new HashSet<string>
    {
        "Ground",
        "Roof",
        "RightWall",
        "LeftWall",
        "Enemy",
        "Death"
    };
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        Debug.Log(player.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            return;
        }
        var isInRange = (player.transform.position - transform.position).magnitude < hookRange;
        if (!isInRange || isOnCooldown)
        {
            if (instanciatedSpot != null)
            {
                Destroy(instanciatedSpot);
                isHookable = false;
                instanciatedSpot = null;
            }
            return;
        }
        var ray = Physics2D.RaycastAll(transform.position, player.transform.position - transform.position);
        var rayHit = false;
        foreach (var hit in ray)
        {
            if (blockSightTags.Contains(hit.collider.tag))
            {
                rayHit = false;
                break;
            }
            if (hit.collider.tag == "Player")
            {
                rayHit = true;
                break;
            }
        }
        if (rayHit) 
        {
            if (!isHookable)
            {
                instanciatedSpot = Instantiate(activeSpot, transform.position, Quaternion.identity);
            }
            isHookable = true;
        }
        else
        {
            if (instanciatedSpot != null)
            {
                Destroy(instanciatedSpot);
                isHookable = false;
                instanciatedSpot = null;
            }
        }
    }

    public IEnumerator EnterCooldown()
    {
        isHookable = false;
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldown);
        isOnCooldown = false;
    }
}
