using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField]
    public Vector3 velocity;
    [SerializeField]
    private Vector3 acceleration = normAcceleration;
    [SerializeField]
    private Vector3 jumpVelocity = new(0, 15, 0);
    [SerializeField]
    private int direction = 1;
    private readonly float resistanceModule = 6f;
    private readonly float resistanceTreshold = 20f;
    private float xShift = 0;
    private static Vector3 normAcceleration = new(0, -25f, 0);
    private static Vector3 normVelocity = new(8f, 0, 0);
    
    public event Action OnJump;
    public event Action OnDash;
    public event Action OnHook;
    public event Action OnMoving;
    public event Action OnNotMoving;
    
    private enum States
    {
        Grounded,
        Roofed,
        LeftBlocked,
        RightBlocked
    }

    [SerializeField]
    private enum Holds
    {
        Left,
        Right
    }

    private readonly Dictionary<States, AxesTransformation> statesEffects = new Dictionary<States, AxesTransformation>
    {
        {States.Grounded, new AxesTransformation(x => false, x => x.y < 0)},
        {States.Roofed, new AxesTransformation(x => false, x => x.y > 0) },
        {States.LeftBlocked, new AxesTransformation(x => x.x > 0, x => false) },
        {States.RightBlocked, new AxesTransformation(x => x.x < 0, x => false) }
    };
    [SerializeField]
    private readonly List<States> currentStates = new();
    private readonly List<Holds> currentHolds = new();
    private bool isGrounded => currentStates.Contains(States.Grounded);
    private bool isRoofed => currentStates.Contains(States.Roofed);
    private bool isLeftBlocked => currentStates.Contains(States.LeftBlocked);
    [SerializeField]
    private bool isLeftHolding => currentHolds.Contains(Holds.Left);
    private bool isRightBlocked => currentStates.Contains(States.RightBlocked);
    [SerializeField]
    private bool isRightHolding => currentHolds.Contains(Holds.Right);
    private bool isHolding => isLeftHolding || isRightHolding;
    [SerializeField]
    private bool isDashing = false;
    [SerializeField]
    private bool canDoubleJump = true;
    private readonly float hookAreaRadius = 2f;
    private readonly float hookTime = 0.5f;
    private readonly float dashTime = 0.3f;
    private readonly float dashDistance = 4f;
    private readonly float dashCooldown = 1.5f;
    public float dashCharge = 1;
    private bool canDash => dashCharge >= 1;
    private float dashRecharge = 0;
    private readonly float slidingDelay = 1f;
    private readonly Vector3 slidingAcceleration = new Vector3(0, -3f, 0);

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
        if (isGrounded && !isLeftBlocked)
        {
            OnMoving?.Invoke();
        }
        else
            OnNotMoving?.Invoke();
        
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
            OnDash?.Invoke();
            StartCoroutine(DashTo(dashTime, dashCooldown, transform.position + new Vector3(dashDistance, 0, 0) * direction, false));
        }
        if (Input.GetMouseButtonDown(0))
        {
            var hookPos = FindClosestHookPos();
            if (hookPos != null && hookPos.isHookable)
            {
                OnHook?.Invoke();
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

    private void ApplyState(States state)
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
            Game.score += collision.GetComponent<Bonus>().price;
            Game.levelScore += collision.GetComponent<Bonus>().price;
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("LeftWall"))
        {
            currentStates.Add(States.LeftBlocked);
        }
        if (collision.CompareTag("RightWall"))
        {
            currentStates.Add(States.RightBlocked);
        }
        if (collision.CompareTag("Ground"))
        {
            currentStates.Add(States.Grounded);
        }
        if (collision.CompareTag("Roof"))
        {
            currentStates.Add(States.Roofed);
        }
        if (collision.CompareTag("Death"))
        {
            TimeSystem.Reset();
            TimeSystem.Stop();
            if (Game.lives == 0)
            {
                MapModel.Reset();
                Game.Reset();
                TimeSystem.Reset();
                SceneManager.LoadScene("AnimatedLoseScene");
            }
            else
            {
                Game.score -= Game.levelScore; 
                Game.levelScore = 0;
                TimeSystem.Reset();
                var UI = FindAnyObjectByType<Canvas>().gameObject;
                var eventSystem = GameObject.Find("EventSystem");
                UI.SetActive(false);
                eventSystem?.SetActive(false);
                SceneManager.LoadScene("ContinueWithLifeScene", LoadSceneMode.Additive);
            }
            Destroy(gameObject);
        }
        if (collision.CompareTag("LeftHoldable"))
        {
            direction = -1;
            if (!isLeftHolding)
            {
                currentStates.Add(States.LeftBlocked);
                currentStates.Add(States.Grounded);
                velocity = Vector3.zero;
            }
            currentHolds.Add(Holds.Left);
            StartCoroutine(Sliding());
        }
        if (collision.CompareTag("RightHoldable"))
        {
            direction = 1;
            if (!isRightHolding)
            {
                currentStates.Add(States.RightBlocked);
                currentStates.Add(States.Grounded);
                velocity = Vector3.zero;
            }
            currentHolds.Add(Holds.Right);
            StartCoroutine(Sliding());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            currentStates.Remove(States.Grounded);
        }
        if (collision.CompareTag("LeftWall"))
        {
            currentStates.Remove(States.LeftBlocked);
        }
        if (collision.CompareTag("RightWall"))
        {
            currentStates.Remove(States.RightBlocked);
        }
        if (collision.CompareTag("Roof"))
        {
            currentStates.Remove(States.Roofed);
        }
        if (collision.CompareTag("LeftHoldable"))
        {
            currentHolds.Remove(Holds.Left);
            if (!isLeftHolding)
            {
                currentStates.Remove(States.LeftBlocked);
                currentStates.Remove(States.Grounded);
            }
        }
        if (collision.CompareTag("RightHoldable"))
        {
            currentHolds.Remove(Holds.Right);
            if (!isRightHolding)
            {
                currentStates.Remove(States.RightBlocked);
                currentStates.Remove(States.Grounded);
            }
        }
    }

    private void Jump()
    {
        if (isGrounded || isHolding)
        {
            OnJump?.Invoke();
            velocity.y = jumpVelocity.y;
        }
        else if (canDoubleJump)
        {
            OnJump?.Invoke();
            velocity.y = jumpVelocity.y;
            canDoubleJump = false;
        }
    }

    private void JumpInDirection(Vector3 direction)
    {
        velocity += jumpVelocity.magnitude * direction.normalized;
    }

    private IEnumerator Sliding()
    {
        yield return new WaitForSeconds(slidingDelay);
        if (isHolding)
        {
            currentStates.Remove(States.Grounded);
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
    }
}
namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit { }
}
record AxesTransformation(Func<Vector3, bool> X, Func<Vector3, bool> Y)
{

}