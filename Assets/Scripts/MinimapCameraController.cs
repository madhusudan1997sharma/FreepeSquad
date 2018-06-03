using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MinimapCameraController : MonoBehaviour
{
    GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, 10, player.transform.position.z);
    }
}