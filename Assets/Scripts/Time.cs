using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Time : MonoBehaviour {

    public Text minutes;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        minutes.text = PlayerPrefs.GetInt("Minutes").ToString();
	}
}
