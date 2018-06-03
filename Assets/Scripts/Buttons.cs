using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Net;

public class Buttons : MonoBehaviour {

    public GameObject PlayerPrefab;
    public GameObject playerCamera;
    public GameObject VirtualWorld;
    public GameObject BackToFuture;
    public GameObject DesertCity;

    public Canvas CanvasStart;
    public Canvas CanvasSettings;
    public Canvas CanvasSingleMultiPlayer;
    public Canvas CanvasHostJoin;
    public Canvas CanvasMapChooser;
    public Canvas CanvasGameplay;
    public Canvas CanvasStore;
    public InputField PlayerName;
    public Text HealthUpgrade;
    public Text EnergyUpgrade;
    public Text HealthUpgradeButtonText;
    public Text EnergyUpgradeButtonText;
    public Text[] Minutes;
    public Button VirtualWorldButton;
    public Button BackToFutureButton;
    public Button DesertCityButton;

    void Awake()
    {
        if (PlayerPrefs.GetString("PlayerName") == "")
        {
            PlayerName.text = "NOOB";
            PlayerPrefs.SetString("PlayerName", "NOOB");
        }
        else if (PlayerPrefs.GetString("PlayerName") != "")
        {
            PlayerName.text = PlayerPrefs.GetString("PlayerName");
        }

        if (!PlayerPrefs.HasKey("HealthUpgrade"))
        {
            PlayerPrefs.SetInt("HealthUpgrade", 1);
        }
        if (!PlayerPrefs.HasKey("EnergyUpgrade"))
        {
            PlayerPrefs.SetInt("EnergyUpgrade", 1);
        }
        PlayerPrefs.SetString("Map", "BTF");
        //PlayerPrefs.DeleteAll();
    }

    void Update()
    {
	for(int i=0; i<Minutes.Length; i++)
	{
	    Minutes[i].text = PlayerPrefs.GetInt("Minutes").ToString();
	}
        HealthUpgrade.text = "x" + PlayerPrefs.GetInt("HealthUpgrade").ToString();
        EnergyUpgrade.text = "x" + PlayerPrefs.GetInt("EnergyUpgrade").ToString();
        if (PlayerPrefs.GetString("Map") == "VW")
        {
            VirtualWorldButton.interactable = false;
            BackToFutureButton.interactable = true;
            DesertCityButton.interactable = true;
        }
        else if (PlayerPrefs.GetString("Map") == "BTF")
        {
            VirtualWorldButton.interactable = true;
            BackToFutureButton.interactable = false;
            DesertCityButton.interactable = true;
        }
        else if (PlayerPrefs.GetString("Map") == "DC")
        {
            VirtualWorldButton.interactable = true;
            BackToFutureButton.interactable = true;
            DesertCityButton.interactable = false;
        }
    }

    public void StartClick()
    {
        Application.runInBackground = true;
        CanvasStart.GetComponent<Canvas>().enabled = false;
        CanvasSingleMultiPlayer.GetComponent<Canvas>().enabled = true;
    }

    public void SettingsClick()
    {
        CanvasStart.GetComponent<Canvas>().enabled = false;
        CanvasSettings.GetComponent<Canvas>().enabled = true;
    }

    public void SingleplayerClick()
    {
        int rnd = Random.Range(1, 4);
        if (rnd == 1)
        {
            Instantiate(VirtualWorld, new Vector3(0, 0, 0), Quaternion.identity);
        }
        else if (rnd == 2)
        {
            Instantiate(BackToFuture, new Vector3(0, 2, 0), Quaternion.identity);
        }
        else if (rnd == 3)
        {
            Instantiate(DesertCity, new Vector3(0, 0, 0), Quaternion.identity);
        }
        PlayerPrefs.SetInt("network", 0);
        CanvasSingleMultiPlayer.GetComponent<Canvas>().enabled = false;
        CanvasGameplay.GetComponent<Canvas>().enabled = true;
        Instantiate(PlayerPrefab, new Vector3(Random.Range(-10, 10), 3, Random.Range(-10, 10)), Quaternion.identity);
        GameObject camera = Instantiate(playerCamera, transform.position, Quaternion.identity) as GameObject;
        camera.transform.eulerAngles = new Vector3(50, 0, 0);
        GameObject.Find("MinimapCamera").GetComponent<MinimapCameraController>().enabled = true;
    }

    public void MultiplayerClick()
    {
        PlayerPrefs.SetInt("network", 1);
        CanvasSingleMultiPlayer.GetComponent<Canvas>().enabled = false;
        CanvasHostJoin.GetComponent<Canvas>().enabled = true;
    }

    public void HostClick()
    {
        CanvasHostJoin.GetComponent<Canvas>().enabled = false;
        CanvasMapChooser.GetComponent<Canvas>().enabled = true;
    }

    public void GoClick()
    {
        Network.InitializeServer(10, 25000, false);
        Network.sendRate = 29;
    }

    public void JoinClick()
    {
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            Network.Connect("192.168.1.3", 25000);
        }
        else
        {
            Network.Connect("192.168.43.1", 25000);
        }
    }

    public void PickClick()
    {
        PlayerPrefs.SetInt("Pick", 1);
    }

    public void ExitClick()
    {
        Network.Disconnect();
        Application.LoadLevel("gameplay");
    }

    public void DoneClick()
    {
        PlayerPrefs.SetString("PlayerName", PlayerName.text);
        Application.LoadLevel("gameplay");
    }

    public void StoreClick()
    {
        CanvasStart.GetComponent<Canvas>().enabled = false;
        CanvasStore.GetComponent<Canvas>().enabled = true;
    }

    public void UpgradeHealthClick()
    {
        if (PlayerPrefs.GetInt("HealthUpgrade") == 1)
        {
            if (PlayerPrefs.GetInt("Minutes") >= 60)
            {
                PlayerPrefs.SetInt("Minutes", PlayerPrefs.GetInt("Minutes") - 60);
                PlayerPrefs.SetInt("HealthUpgrade", 2);
                HealthUpgradeButtonText.text = "Upgrade for 150 Minutes";
            }
        }
        else if (PlayerPrefs.GetInt("HealthUpgrade") == 2)
        {
            if (PlayerPrefs.GetInt("Minutes") >= 150)
            {
                PlayerPrefs.SetInt("Minutes", PlayerPrefs.GetInt("Minutes") - 150);
                PlayerPrefs.SetInt("HealthUpgrade", 3);
                HealthUpgradeButtonText.text = "Upgrade for 250 Minutes";
            }
        }
        else if (PlayerPrefs.GetInt("HealthUpgrade") == 3)
        {
            if (PlayerPrefs.GetInt("Minutes") >= 250)
            {
                PlayerPrefs.SetInt("Minutes", PlayerPrefs.GetInt("Minutes") - 250);
                PlayerPrefs.SetInt("HealthUpgrade", 4);
                HealthUpgradeButtonText.text = "Upgrade for 500 Minutes";
            }
        }
        else if (PlayerPrefs.GetInt("HealthUpgrade") == 4)
        {
            if (PlayerPrefs.GetInt("Minutes") >= 500)
            {
                PlayerPrefs.SetInt("Minutes", PlayerPrefs.GetInt("Minutes") - 500);
                PlayerPrefs.SetInt("HealthUpgrade", 5);
            }
        }
    }

    public void UpgradeEnergyClick()
    {

    }

    public void VWClick()
    {
        PlayerPrefs.SetString("Map", "VW");
    }

    public void BTFClick()
    {
        PlayerPrefs.SetString("Map", "BTF");
    }

    public void DCClick()
    {
        PlayerPrefs.SetString("Map", "DC");
    }










    void OnServerInitialized()
    {
        HOSTcreatePlayer();
    }

    public void HOSTcreatePlayer()
    {
        CanvasMapChooser.GetComponent<Canvas>().enabled = false;
        CanvasGameplay.GetComponent<Canvas>().enabled = true;
        if (PlayerPrefs.GetString("Map") == "VW")
        {
            GameObject map = Network.Instantiate(VirtualWorld, new Vector3(0, 0, 0), Quaternion.identity, 1) as GameObject;
        }
        else if (PlayerPrefs.GetString("Map") == "BTF")
        {
            GameObject map = Network.Instantiate(BackToFuture, new Vector3(0, 2, 0), Quaternion.identity, 1) as GameObject;
        }
        else if (PlayerPrefs.GetString("Map") == "DC")
        {
            GameObject map = Network.Instantiate(DesertCity, new Vector3(0, 0, 0), Quaternion.identity, 1) as GameObject;
        }
        GameObject g = Network.Instantiate(PlayerPrefab, new Vector3(Random.Range(-10, 10), 3, Random.Range(-10, 10)), Quaternion.identity, 1) as GameObject;
        GameObject camera = Instantiate(playerCamera, transform.position, Quaternion.identity) as GameObject;
        camera.transform.eulerAngles = new Vector3(50, 0, 0);
        GameObject.Find("MinimapCamera").GetComponent<MinimapCameraController>().enabled = true;
    }



    void OnConnectedToServer()
    {
        JOINcreatePlayer();
    }

    public void JOINcreatePlayer()
    {
        CanvasHostJoin.GetComponent<Canvas>().enabled = false;
        CanvasGameplay.GetComponent<Canvas>().enabled = true;
        GameObject g = Network.Instantiate(PlayerPrefab, new Vector3(Random.Range(-10, 10), 3, Random.Range(-10, 10)), Quaternion.identity, 1) as GameObject;
        GameObject camera = Instantiate(playerCamera, transform.position, Quaternion.identity) as GameObject;
        camera.transform.eulerAngles = new Vector3(50, 0, 0);
        GameObject.Find("MinimapCamera").GetComponent<MinimapCameraController>().enabled = true;
    }
}