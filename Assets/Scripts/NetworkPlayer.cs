using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class NetworkPlayer : MonoBehaviourPun, IPunObservable , IPunInstantiateMagicCallback
{
    [SerializeField] int maxHealth;
    [SerializeField] float movementSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] TextMeshProUGUI playerName;
    [SerializeField] GameObject bulletPrefab;
    private float coolDown = 0.0f;
    private float timerRate = 1f;
    [SerializeField] float bulletSpeed = 800f;

    int health;
    Rigidbody rb;
    Material SphereMaterial;
    MeshRenderer meshRenderer;

    void Start()
    {
        if (photonView.IsMine)
        {
            health = maxHealth;
            rb = GetComponent<Rigidbody>();
            SphereMaterial = Resources.Load<Material>("localplayer");
            meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.material = SphereMaterial;
        }
    }
    
    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                health -= 5;
            }
            Vector2 input;
            input.x = Input.GetAxis("Horizontal");
            input.y = Input.GetAxis("Vertical");
            Vector3 currentVelocity = rb.velocity;
            currentVelocity.x = Mathf.Clamp(currentVelocity.x + (input.x * movementSpeed * Time.deltaTime), -maxSpeed, maxSpeed);
            currentVelocity.z = Mathf.Clamp(currentVelocity.z + (input.y * movementSpeed * Time.deltaTime), -maxSpeed, maxSpeed);

            rb.velocity = currentVelocity;

            if (Time.time >= coolDown)
            {
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    photonView.RPC("Shoot", RpcTarget.All, 1, 0);
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    photonView.RPC("Shoot", RpcTarget.All, -1, 0);
                }
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    photonView.RPC("Shoot", RpcTarget.All, 0, 1);
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    photonView.RPC("Shoot", RpcTarget.All, 0, -1);
                }
            }
        }
    }

    [PunRPC]
    void Shoot(int x,int z)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
        Rigidbody rigidbody = bullet.GetComponent<Rigidbody>();
        if (x > 0)
        {
            rigidbody.AddForce( bulletSpeed * transform.right);
        }
        else if (x < 0)
        {
            rigidbody.AddForce( bulletSpeed * - transform.right);
        }
        else if (z > 0)
        {
            rigidbody.AddForce( bulletSpeed * transform.forward);
        }
        else if (z < 0)
        {
            rigidbody.AddForce( bulletSpeed * -transform.forward);
        }
        coolDown = Time.time + timerRate;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(health);
        }
        else
        {
            health = (int)stream.ReceiveNext();
        }
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        playerName.text = info.Sender.NickName;
    }
}
