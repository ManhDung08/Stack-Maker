using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private float speed = 10.0f;
    [SerializeField] public int brickCount;
    [SerializeField] private GameObject playerBrick;
    [SerializeField] private GameObject people;
    [SerializeField] private GameObject WinPos;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private ParticleSystem firework1;
    [SerializeField] private ParticleSystem firework2;
    [SerializeField] private GameObject closedChest;
    [SerializeField] private GameObject openedChest;


    private string currentAnimName;
    private Vector3 direction;
    private Vector3 target;
    private bool isMoving;
    private bool isHitWall;
    private bool isEndGame;
    private Vector2 initialMousePos;


    // Start is called before the first frame update
    void Start()
    {
        brickCount = 0;
        isMoving = false;
        isHitWall = false;
        isEndGame = false;
        target = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isEndGame)
        {
            return;
        }
        else
        {
            if (isMoving)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                if (Vector3.Distance(transform.position, target) <= 0.01f)
                {
                    isMoving = false;
                }
                if (Vector3.Distance(transform.position, WinPos.transform.position) <= 0.01f)
                {
                    Win();
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    initialMousePos = Input.mousePosition;
                }
                if (Input.GetMouseButtonUp(0))
                {
                    direction = CalculateDirection(Input.mousePosition);
                    if(direction == Vector3.zero)
                    {
                        return;
                    }
                    isHitWall = CheckWall();
                    if (isHitWall)
                    {
                        isMoving = false;
                    }
                    else
                    {
                        target = CalculateTarget(direction);
                        isMoving = true;
                    }
                }
            }
        }
    }

    private Vector3 CalculateDirection(Vector3 finalMousePos)
    {
        float disX = Mathf.Abs(initialMousePos.x - finalMousePos.x);
        float disY = Mathf.Abs(initialMousePos.y - finalMousePos.y);
        if (disX > 0 || disY > 0)
        {
            if (disX > disY)
            {
                //left
                if (initialMousePos.x > finalMousePos.x)
                {
                    people.transform.rotation = Quaternion.Euler(0f, 120f, 0f);
                    return Vector3.left;
                }

                //right
                else
                {
                    people.transform.rotation = Quaternion.Euler(0f, -60f, 0f);
                    return Vector3.right;
                }
            }
            else
            {
                //Down
                if (initialMousePos.y > finalMousePos.y)
                {
                    people.transform.rotation = Quaternion.Euler(0f, 30f, 0f);
                    return Vector3.back;
                }
                //Up
                else
                {
                    people.transform.rotation = Quaternion.Euler(0f, -150f, 0f);
                    return Vector3.forward;
                }
            }
        }
        return Vector3.zero;
    }

    private bool CheckWall()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, 1f))
        {
            if (hit.transform.CompareTag("Wall"))
            {
                return true;
            }
        }
        return false;
    }

    private Vector3 CalculateTarget(Vector3 direction)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity, wallLayer))
        {
            if (direction == Vector3.forward || direction == Vector3.back)
            {
                return transform.position + direction * (Mathf.Abs(hit.transform.position.z - transform.position.z) - 1);
            }
            else
            {
                return transform.position + direction * (Mathf.Abs(hit.transform.position.x - transform.position.x) - 1);
            }
        }

        else
        {
            return WinPos.transform.position;
        }
    }

    private void Jump()
    {

    }

    private void Win()
    {
        isEndGame = true;
        ChangeAnim("win");
        firework1.Play();
        firework2.Play();
        closedChest.SetActive(false);
        openedChest.SetActive(true);
    }



    private void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            anim.ResetTrigger(animName);
            currentAnimName = animName;
            anim.SetTrigger(currentAnimName);
            Debug.Log(animName);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WinPos"))
        {
            Destroy(playerBrick);
            people.transform.position = new Vector3(people.transform.position.x, 5.01f, people.transform.position.z);
            ChangeAnim("jump");
        }
    }
}


