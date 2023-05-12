using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickController : MonoBehaviour
{
    [SerializeField] private LayerMask writelineLayer;

    private GameObject playerBrick;
    private GameObject player;
    private float brickHeight;
    private bool isAdded;
    private bool isHaveBrick;
    private PlayerController playerController;
    


    // Start is called before the first frame update
    void Start()
    {
        isHaveBrick = false;
        isAdded = false;
        brickHeight = 0.3f;
        if(playerBrick == null)
        {
            playerBrick = GameObject.FindGameObjectWithTag("PlayerBrick");
        }
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAdded && Mathf.Abs(transform.position.x - player.transform.position.x) <= 0.1f && Mathf.Abs(transform.position.z - player.transform.position.z) <= 0.1f)
        {
            AddBrickToPlayer();
        }
        RaycastHit hit;
        if (isAdded && !isHaveBrick && Physics.Raycast(transform.position, Vector3.down, out hit, brickHeight, writelineLayer))
        {
            if(hit.transform.childCount == 0)
            {
                player.transform.position += Vector3.down * brickHeight;
                playerBrick.transform.position += Vector3.down * brickHeight;
                transform.SetParent(hit.transform, false);
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.Euler(0f, 0f, -180f);
                playerController.brickCount--;
                isHaveBrick = true;
            }
        }
    }

    private void AddBrickToPlayer()
    {
        transform.SetParent(playerBrick.transform, false);
        player.transform.position += Vector3.up * brickHeight;
        playerBrick.transform.position += Vector3.up * brickHeight;
        playerController.brickCount++;
        transform.localPosition = Vector3.zero - Vector3.up * brickHeight * playerController.brickCount;
        isAdded = true;
    }

    
}
