using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    public Vector3 velocity;
    [SerializeField]
    private Vector3 acceleration = normAcceleration;
    [SerializeField]
    private Vector3 jumpVelocity = new Vector3(0, 15, 0);
    [SerializeField]
    private int direction = 1;
    private float resistanceModule = 6f;
    private float resistanceTreshold = 20f;
    private float xShift = 0;
    private static Vector3 normAcceleration = new Vector3(0, -25f, 0);
    private static Vector3 normVelocity = new Vector3(8f, 0, 0);
    private enum states
    {
        Grounded,
        Roofed,
        LeftBlocked,
        RightBlocked
    }

    [SerializeField]
    private enum holds
    {
        Left,
        Right
    }

    private readonly Dictionary<states, AxesTransformation> statesEffects = new Dictionary<states, AxesTransformation>
    {
        {states.Grounded, new AxesTransformation(x => false, x => x.y < 0)},
        {states.Roofed, new AxesTransformation(x => false, x => x.y > 0) },
        {states.LeftBlocked, new AxesTransformation(x => x.x > 0, x => false) },
        {states.RightBlocked, new AxesTransformation(x => x.x < 0, x => false) }
    };
    [SerializeField]
    private List<states> currentStates = new ();
    private List<holds> currentHolds = new ();
    private bool isGrounded => currentStates.Contains(states.Grounded);
    private bool isRoofed => currentStates.Contains(states.Roofed);
    private bool isLeftBlocked => currentStates.Contains(states.LeftBlocked);
    [SerializeField]
    private bool isLeftHolding => currentHolds.Contains(holds.Left);
    private bool isRightBlocked => currentStates.Contains(states.RightBlocked);
    [SerializeField]
    private bool isRightHolding => currentHolds.Contains(holds.Right);
    private bool isHolding => isLeftHolding || isRightHolding;
    [SerializeField]
    private bool isDashing = false;
    [SerializeField]
    private bool canDoubleJump = true;
    private float hookAreaRadius = 2f;
    private float hookTime = 0.5f;
    private float dashTime = 0.3f;
    private float dashDistance = 4f;
    private float dashCooldown = 1.5f;
    public float dashCharge = 1;
    private bool canDash => dashCharge >= 1;
    private float dashRecharge = 0;
    private float slidingDelay = 1f;
    private Vector3 slidingAcceleration = new Vector3(0, -3f, 0);

    public void ApplyAcceleration(Vector3 acceleration)
    {
        this.acceleration += acceleration;
    }

    public void ApplyVelocity(Vector3 velocity)
    {
        this.velocity += velocity;
    }

    // Start is called before the first frame update
    void Start()
    {
        acceleration = normAcceleration;
        velocity = normVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        xShift = Input.GetAxis("Horizontal");
        if (isDashing)
        {
            return;
        }
        if (Input.GetButtonDown("Jump"))
        {
            if (isLeftHolding)
            {
                JumpInDirection(new Vector3(-1f, 1f, 0));
            }
            if (isRightHolding)
            {
                JumpInDirection(new Vector3(1f, 1f, 0));
            }
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(DashTo(dashTime, dashCooldown, transform.position + new Vector3(dashDistance, 0, 0) * direction, false));
        }
        if (Input.GetMouseButtonDown(0))
        {
            var hookPos = FindClosestHookPos();
            if (hookPos != null && hookPos.isHookable)
            {
                StartCoroutine(DashTo(hookTime, 0, hookPos.transform.position, true));
            }
        }
    }

    private void FixedUpdate()
    {
        if (dashCharge < 1)
        {
            dashCharge += dashRecharge * Time.deltaTime;
        }
        if (!isHolding && !isDashing)
        {
            acceleration = normAcceleration;
        }
        if (velocity.magnitude > resistanceTreshold && !isDashing)
        {
            acceleration = normAcceleration - velocity.normalized * resistanceModule;
        }
        if (velocity.magnitude < 1e-3 && !isHolding)
        {
            velocity = new Vector3(0.05f, 0, 0) * direction;
        }
        if (isGrounded)
        {
            canDoubleJump = true;
            if (!isDashing && !isHolding)
            {
                velocity.x = normVelocity.x * direction;
            }
        }
        foreach (var state in currentStates) 
        {
            ApplyState(state);
        }
        var slowedVelocity = velocity + new Vector3(xShift * velocity.x / 4f, 0, 0) * direction;
        transform.position += slowedVelocity * Time.deltaTime;
        velocity += acceleration * Time.deltaTime;
    }

    private void ApplyState(states state)
    {
        velocity.x = statesEffects[state].X(velocity) ? 0 : velocity.x;
        velocity.y = statesEffects[state].Y(velocity) ? 0 : velocity.y;
        acceleration.x = statesEffects[state].X(acceleration) ? 0 : acceleration.x;
        acceleration.y = statesEffects[state].Y(acceleration) ? 0 : acceleration.y;
    }

    private HookSpot FindClosestHookPos()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var objects = Physics2D.OverlapCircleAll(mousePos, hookAreaRadius);
        var closestPos = objects
            .Select(obj => obj.gameObject)
            .Where(x => x.GetComponent<HookSpot>() != null)
            .OrderBy(x => (x.transform.position - mousePos).magnitude)
            .FirstOrDefault();
        if (closestPos != null)
        {
            return closestPos.GetComponent<HookSpot>();
        }
        return null;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("JumpRefresher"))
        {
            canDoubleJump = true;
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("DashRefresher"))
        {
            dashCharge = 1;
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("Bonus"))
        {
            ScoreSystem.score += collision.GetComponent<Bonus>().price;
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("LeftWall"))
        {
            currentStates.Add(states.LeftBlocked);
        }
        if (collision.CompareTag("RightWall"))
        {
            currentStates.Add(states.RightBlocked);
        }
        if (collision.CompareTag("Ground"))
        {
            Debug.Log("Enter");
            currentStates.Add(states.Grounded);
        }
        if (collision.CompareTag("Roof"))
        {
            currentStates.Add(states.Roofed);
        }
        if (collision.CompareTag("Death"))
        {
            TimeSystem.Reset();
            TimeSystem.Stop();
            SceneManager.LoadScene("LoseScene");
            Destroy(gameObject);
        }
        if (collision.CompareTag("LeftHoldable"))
        {
            direction = -1;
            if (!isLeftHolding)
            {
                currentStates.Add(states.LeftBlocked);
                currentStates.Add(states.Grounded);
                velocity = Vector3.zero;
            }
            currentHolds.Add(holds.Left);
            StartCoroutine(Sliding());
        }
        if (collision.CompareTag("RightHoldable"))
        {
            direction = 1;
            if (!isRightHolding)
            {
                currentStates.Add(states.RightBlocked);
                currentStates.Add(states.Grounded);
                velocity = Vector3.zero;
            }
            currentHolds.Add(holds.Right);
            StartCoroutine(Sliding());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            Debug.Log("Exit");
            currentStates.Remove(states.Grounded);
        }
        if (collision.CompareTag("LeftWall"))
        {
            currentStates.Remove(states.LeftBlocked);
        }
        if (collision.CompareTag("RightWall"))
        {
            currentStates.Remove(states.RightBlocked);
        }
        if (collision.CompareTag("Roof"))
        {
            currentStates.Remove(states.Roofed);
        }
        if (collision.CompareTag("LeftHoldable"))
        {
            Debug.Log("LeftLeft");
            Debug.Log(transform.position);
            currentHolds.Remove(holds.Left);
            if (!isLeftHolding)
            {
                currentStates.Remove(states.LeftBlocked);
                currentStates.Remove(states.Grounded);
            }
        }
        if (collision.CompareTag("RightHoldable"))
        {
            Debug.Log("LeftRight");
            currentHolds.Remove(holds.Right);
            if (!isRightHolding)
            {
                currentStates.Remove(states.RightBlocked);
                currentStates.Remove(states.Grounded);
            }
        }
    }

    private void Jump()
    {
        if (isGrounded || isHolding)
        {
            velocity.y = jumpVelocity.y;
        }
        else if (canDoubleJump)
        {
            velocity.y = jumpVelocity.y;
            canDoubleJump = false;
        }
    }

    private void JumpInDirection(Vector3 direction)
    {
        velocity += jumpVelocity.magnitude * direction.normalized;
        Debug.Log(velocity);
    }

    private IEnumerator Sliding()
    {
        yield return new WaitForSeconds(slidingDelay);
        if (isHolding)
        {
            currentStates.Remove(states.Grounded);
            acceleration = slidingAcceleration;
        }
    }

    private IEnumerator DashTo(float dashTime, float dashCooldown, Vector3 target, bool saveVelocity)
    {
        isDashing = true;
        if (dashCooldown > 0)
        {
            dashCharge = 0;
            dashRecharge = 1 / dashCooldown;
        }
        var direction = target - transform.position;
        velocity = Vector3.zero;
        acceleration = Vector3.zero;
        yield return new WaitForSeconds(0.01f);
        velocity = direction / dashTime;
        yield return new WaitForSeconds(dashTime);
        if (!saveVelocity && !isHolding)
        {
            velocity = normVelocity * this.direction;
        }
        isDashing = false;
        Debug.Log(velocity);
        Debug.Log(acceleration);
        Debug.Log(dashCharge);
        Debug.Log(transform.position);
        Debug.Log(currentHolds.Count);
        Debug.Log(currentStates.Count);
    }
}
namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit { }
}
record AxesTransformation(Func<Vector3, bool> X, Func<Vector3, bool> Y)
{

}