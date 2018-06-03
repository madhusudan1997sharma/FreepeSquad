using UnityEngine;
using System.Collections;

public class Enemy_AI : MonoBehaviour {

    float speed = 0.05f;
    int enemyhealth;
    GameObject player;

	void Start () {
        enemyhealth = 100;
	}

    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        transform.LookAt(player.transform);
        transform.Translate(Vector3.forward * speed);
        if (enemyhealth == 0)
        {
            Destroy(this.gameObject);
        }
        transform.position = new Vector3(transform.position.x, 5.5f, transform.position.z);
    }

    void OnParticleCollision()
    {
        enemyhealth -= 5;
    }
}
