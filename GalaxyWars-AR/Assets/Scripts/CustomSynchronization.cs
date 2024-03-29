using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CustomSynchronization : MonoBehaviour, IPunObservable
{

    Rigidbody rb;
    PhotonView photonView;

    Vector3 networkedPosition;
    Quaternion networkedRotation;

    public bool synchronizeVelocity = true;
    public bool synchronizeAngularVelocity = true;
    public bool isTeleportEnabled = true;
    public float teleportIfDistanceGreaterThan = 1.0f;

    private float distance;
    private float angle;

    private GameObject battleArena;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();

        networkedPosition = new Vector3();
        networkedRotation = new Quaternion();

        battleArena = GameObject.Find("BattleArena");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(!photonView.IsMine)
        {
            rb.position = Vector3.MoveTowards(rb.position, networkedPosition, distance*(1.0f/PhotonNetwork.SerializationRate));
            rb.rotation = Quaternion.RotateTowards(rb.rotation, networkedRotation, angle*(1.0f / PhotonNetwork.SerializationRate));
        }


    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            //Then, phootnView is mine and I am the one who controls this player
            //should send position, velocity etc. data to the other players
            stream.SendNext(rb.position-battleArena.transform.position);
            stream.SendNext(rb.rotation);

            if(synchronizeVelocity) 
            {
                stream.SendNext(rb.velocity);
            }
            
            if(synchronizeAngularVelocity)
            {
                stream.SendNext(rb.angularVelocity);
            }
        }
        else
        {
            //Called on my player gameobject that exists in my remote player's game
            networkedPosition = (Vector3)stream.ReceiveNext() + battleArena.transform.position;
            networkedRotation = (Quaternion)stream.ReceiveNext();

            if(isTeleportEnabled)
            {
                if ((Vector3.Distance(rb.position, networkedPosition)) > teleportIfDistanceGreaterThan)
                {
                    rb.position = networkedPosition;
                }
            }
        
            if(synchronizeVelocity || synchronizeAngularVelocity)
            {
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));

                if(synchronizeVelocity) 
                {
                    rb.velocity = (Vector3)stream.ReceiveNext();
                    networkedPosition += rb.velocity * lag;

                    distance = Vector3.Distance(rb.position, networkedPosition);
                }

                if (synchronizeAngularVelocity)
                {
                    rb.angularVelocity = (Vector3)stream.ReceiveNext();
                    networkedRotation = Quaternion.Euler(rb.angularVelocity * lag) * networkedRotation;

                    angle = Quaternion.Angle(rb.rotation, networkedRotation);
                }
            }
        }
    }
}
