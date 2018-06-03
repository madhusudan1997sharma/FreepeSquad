using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, 13f + player.transform.position.y, player.transform.position.z - 9.5f);
	}
}