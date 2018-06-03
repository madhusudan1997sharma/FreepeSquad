using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.Networking;
using gui = UnityEngine.GUILayout;

public class PlayerController : MonoBehaviour
{
    public GameObject Enemy;
    public GameObject Explosion;
    public ParticleSystem bullet;
    public GameObject playerName;
    Control_Joystick controljoystick;
    Shooting_Joystick shootingjoystick;
    int network;
    GameObject HealthText;
    float health = 100;
    int life;

    //NETWORKING STUFF FOR INTERPOLATION
    private Vector3 lastPosition;
    private Quaternion lastRotation;
    private Transform myTransform;

    // Use this for initialization
    void Start()
    {
        network = PlayerPrefs.GetInt("network");
        HealthText = GameObject.FindGameObjectWithTag("Health");
        HealthText.GetComponent<Text>().text = health.ToString();
        if (network == 1)
        {
            if (GetComponent<NetworkView>().isMine)
            {
                myTransform = transform;
                GetComponent<NetworkView>().RPC("UpdateMovement", RPCMode.OthersBuffered, myTransform.position, myTransform.rotation);
                controljoystick = GameObject.Find("Control_Joystick").GetComponent<Control_Joystick>();
                shootingjoystick = GameObject.Find("Shooting_Joystick").GetComponent<Shooting_Joystick>();
            }
        }
        else if (network == 0)
        {
            controljoystick = GameObject.Find("Control_Joystick").GetComponent<Control_Joystick>();
            shootingjoystick = GameObject.Find("Shooting_Joystick").GetComponent<Shooting_Joystick>();
            Invoke("EnemyCreater", 4);
        }
        playerName.GetComponent<TextMesh>().text = PlayerPrefs.GetString("PlayerName");
    }
    void Update()
    {
        if (network == 1)
        {
            if (GetComponent<NetworkView>().isMine)
            {
                transform.position += (new Vector3(controljoystick.GetComponent<Control_Joystick>().Horizontal() * 0.165f, 0, controljoystick.GetComponent<Control_Joystick>().Vertical() * 0.165f));

                transform.rotation = Quaternion.LookRotation(new Vector3(shootingjoystick.GetComponent<Shooting_Joystick>().Horizontal(), 0, shootingjoystick.GetComponent<Shooting_Joystick>().Vertical()), Vector3.up);


                //INTERPOLATION OF POSITION THROUGH RPC CALLS
                lastPosition = myTransform.position;
                GetComponent<NetworkView>().RPC("UpdateMovement", RPCMode.All, myTransform.position, myTransform.rotation);

                //INTERPOLATION OF ROTATION THROUGH RPC CALLS
                if (Quaternion.Angle(myTransform.rotation, lastRotation) >= 1)
                {
                    GetComponent<NetworkView>().RPC("UpdateMovement", RPCMode.All, myTransform.position, myTransform.rotation);
                }
                life = (int)health;
                HealthText.GetComponent<Text>().text = life.ToString();

                if (life <= 0)
                {
                    GameObject explosion = Network.Instantiate(Explosion, transform.position, Quaternion.identity, 1) as GameObject;
                    GetComponent<NetworkView>().RPC("Destroy", RPCMode.All);
                    health = 100;
                    transform.position = new Vector3(Random.Range(-10, 10), 3, Random.Range(-10, 10));
                }
            }
        }
        else if (network == 0)
        {
            transform.position += (new Vector3(controljoystick.GetComponent<Control_Joystick>().Horizontal() * 0.165f, 0, controljoystick.GetComponent<Control_Joystick>().Vertical() * 0.165f));

            transform.rotation = Quaternion.LookRotation(new Vector3(shootingjoystick.GetComponent<Shooting_Joystick>().Horizontal(), 0, shootingjoystick.GetComponent<Shooting_Joystick>().Vertical()), Vector3.up);

            life = (int)health;
            HealthText.GetComponent<Text>().text = life.ToString();

            if (life <= 0)
            {
                health = 100;
                GameObject explosion = GameObject.Instantiate(Explosion, transform.position, Quaternion.identity) as GameObject;
                Destroy(GameObject.FindGameObjectWithTag("Explosion"), 2);
                transform.position = new Vector3(Random.Range(-10, 10), 3, Random.Range(-10, 10));
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy" && !IsInvoking("healthreducer"))
        {
            InvokeRepeating("healthreducer", 0, 1);
        }
    }

    void OnCollisionExit(Collision other)
    {
        CancelInvoke("healthreducer");
    }

    void healthreducer()
    {
        if (PlayerPrefs.GetInt("HealthUpgrade") == 1)
        {
            health -= 3f;
        }
        else if (PlayerPrefs.GetInt("HealthUpgrade") == 2)
        {
            health -= 2.5f;
        }
        else if (PlayerPrefs.GetInt("HealthUpgrade") == 3)
        {
            health -= 2f;
        }
        else if (PlayerPrefs.GetInt("HealthUpgrade") == 4)
        {
            health -= 1.5f;
        }
        else if (PlayerPrefs.GetInt("HealthUpgrade") == 5)
        {
            health -= 0.5f;
        }
    }

    void EnemyCreater()
    {
        GameObject enemy = GameObject.Instantiate(Enemy, new Vector3(Random.Range(-10, 10), 4, Random.Range(-10, 10)), Quaternion.identity) as GameObject;
        Invoke("EnemyCreater", Random.Range(6, 12));
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void startshoot()
    {
        if (network == 1)
        {
            if (GetComponent<NetworkView>().isMine)
            {
                bullet.enableEmission = true;
                GetComponent<NetworkView>().RPC("Bullet", RPCMode.All, true);
            }
        }
        else if (network == 0)
        {
            bullet.enableEmission = true;
        }
    }

    public void stopshoot()
    {
        if (network == 1)
        {
            if (GetComponent<NetworkView>().isMine)
            {
                bullet.enableEmission = false;
                GetComponent<NetworkView>().RPC("Bullet", RPCMode.All, false);
            }
        }
        else if (network == 0)
        {
            bullet.enableEmission = false;
        }
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void OnParticleCollision()
    {
        if (PlayerPrefs.GetInt("HealthUpgrade") == 1)
        {
            health -= 3f;
        }
        else if (PlayerPrefs.GetInt("HealthUpgrade") == 2)
        {
            health -= 2.5f;
        }
        else if (PlayerPrefs.GetInt("HealthUpgrade") == 3)
        {
            health -= 2f;
        }
        else if (PlayerPrefs.GetInt("HealthUpgrade") == 4)
        {
            health -= 1.5f;
        }
        else if (PlayerPrefs.GetInt("HealthUpgrade") == 5)
        {
            health -= 0.5f;
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    void OnTriggerEnter(Collider other)
    {
        PlayerPrefs.SetInt("Pick", 0);
        if (other.gameObject.tag == "Tank")
        {
            GameObject.Find("Pick").GetComponent<Button>().interactable = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (PlayerPrefs.GetInt("Pick") == 1)
        {
            GetComponent<NetworkView>().RPC("Guns", RPCMode.All, other.gameObject.tag);
        }
    }

    void OnTriggerExit()
    {
        PlayerPrefs.SetInt("Pick", 0);
        CancelInvoke("Picker");
        GameObject.Find("Pick").GetComponent<Button>().interactable = false;
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




    [RPC]
    void UpdateMovement(Vector3 newPosition, Quaternion newRotation)
    {
        transform.position = newPosition;
        transform.rotation = newRotation;
    }

    [RPC]
    void Bullet(bool emission)
    {
        if (emission == true)
        {
            bullet.enableEmission = true;
        }
        else if (emission == false)
        {
            bullet.enableEmission = false;
        }
    }

    [RPC]
    void Guns(string gunName)
    {
        if (gunName == "Uzi")
        {
            bullet.emissionRate = 7;
        }
    }

    [RPC]
    void Destroy()
    {
        Destroy(GameObject.FindGameObjectWithTag("Explosion"), 2);
    }




    void OnPlayerDisconnected(NetworkPlayer player)
    {
        Network.DestroyPlayerObjects(player);
        Network.RemoveRPCs(player);
    }
}