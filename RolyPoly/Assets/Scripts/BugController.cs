using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugController : MonoBehaviour
{
    //Instances
    [SerializeField]
    GameObject ball;

    //Values
    [SerializeField]
    float movementSpeed;
    [SerializeField]
    LayerMask layers;
    [SerializeField]
    float raycastLength;

    //Components
    Rigidbody2D myRigidbody;
    SpriteRenderer mySpriteRenderer;

    //Variables
    string currentMovementAxis = "Horizontal";
    int currentOrientation = 1;
    float currentHorizontalAxis = 0;
    float currentVerticalAxis = 0;
    float previousHorizontalAxis = 0;
    float previousVerticalAxis = 0;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentHorizontalAxis = Input.GetAxisRaw("Horizontal");
        currentVerticalAxis = Input.GetAxisRaw("Vertical");

        if (currentHorizontalAxis != previousHorizontalAxis || currentVerticalAxis != previousVerticalAxis)
            ResetOrientation();

        if (Input.GetButtonDown("Ball"))
            turnToBall();

        if (IsGrounded())
        {
            Move();

            if(Input.GetAxisRaw(currentMovementAxis) != 0f)
            {
                RaycastHit2D touchedWall = CheckWall();

                if (touchedWall)
                    ClimbWall(touchedWall);
            }
        }
        else
        {
            TurnCorner();
        }
            
        previousHorizontalAxis = Input.GetAxisRaw("Horizontal");
        previousVerticalAxis = Input.GetAxisRaw("Vertical");
    }

    void turnToBall()
    {
        ball.transform.position = transform.position;
        ball.transform.rotation = transform.rotation;
        ball.SetActive(true);
        gameObject.SetActive(false);
    }

    void Move()
    {
        myRigidbody.velocity = transform.right * (movementSpeed * Input.GetAxisRaw(currentMovementAxis) * currentOrientation);

        if(Input.GetAxisRaw(currentMovementAxis) != 0)
            FlipSprite();
    }

    void TurnCorner()
    {
        myRigidbody.velocity = Vector2.zero;

        RaycastHit2D touchedSurface = CheckCorner();

        if (touchedSurface)
        {
            if (touchedSurface.normal.x == 0)
                transform.position = touchedSurface.point + (new Vector2(touchedSurface.normal.x, Mathf.Round(touchedSurface.normal.y)) * 0.5f);
            else if (touchedSurface.normal.y == 0)
                transform.position = touchedSurface.point + (new Vector2(Mathf.Round(touchedSurface.normal.x), touchedSurface.normal.y) * 0.5f);
            else
                transform.position = touchedSurface.point + (touchedSurface.normal * 0.5f);

            transform.rotation = Quaternion.FromToRotation(Vector2.up, touchedSurface.normal);
            if (touchedSurface.normal.y < 0f)
                transform.eulerAngles = new Vector3(0f, 0f, 180f);
        }
    }

    void ClimbWall(RaycastHit2D hit)
    {
        transform.position = hit.point + (new Vector2 (Mathf.Round(hit.normal.x), Mathf.Round(hit.normal.y)) * 0.5f);
        transform.rotation = Quaternion.FromToRotation(Vector2.up, hit.normal);
        if (hit.normal.y < 0f)
            transform.eulerAngles = new Vector3(0f, 0f, 180f);
    }

    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up * -1f, raycastLength, layers);

        if (hit.collider != null)
            return true;

        return false;
    }

    RaycastHit2D CheckCorner()
    {
        RaycastHit2D hit;

        hit = Physics2D.Raycast(transform.position + (transform.up * -1f * raycastLength), transform.right, raycastLength, layers);
        if (hit.collider != null)
            return hit;

        hit = Physics2D.Raycast(transform.position + (transform.up * -1f * raycastLength), transform.right * -1f, raycastLength, layers);
        if (hit.collider != null)
            return hit;

        return hit;
    }

    RaycastHit2D CheckWall()
    {
        RaycastHit2D hit;

        hit = Physics2D.Raycast(transform.position, transform.right * (Mathf.Sign(Input.GetAxisRaw(currentMovementAxis)) * currentOrientation), raycastLength, layers);
        if (hit.collider != null)
            return hit;

        return hit;
    }

    void ResetOrientation()
    {
        if (transform.up.x <= 0.1f && transform.up.x >= -0.1f)
            currentMovementAxis = "Horizontal";
        else
            currentMovementAxis = "Vertical";

        if (transform.up.y < -0.1f || transform.up.x > 0f)
            currentOrientation = -1;
        else
            currentOrientation = 1;
    }

    void FlipSprite()
    {
        if (Input.GetAxisRaw(currentMovementAxis) * currentOrientation > 0)
            mySpriteRenderer.flipX = false;
        else
            mySpriteRenderer.flipX = true;
    }

    void OnEnable()
    {
        ResetOrientation();
    }
}
