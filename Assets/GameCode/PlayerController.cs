using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    [SyncVar]
    private Color defcolor = Color.green;
    // Use this for initialization

    [Command]
    void CmdChangeColor(Color newcolor)
    {
        defcolor = newcolor;
    }
    void Start()
    {
        GameManCtrl gmc = GameObject.FindObjectOfType<GameManCtrl>();
        gmc.incNplayers();
        CmdChangeColor( new Color(0.3f, 0.3f, 0.9f) );
        var nplayer = gmc.GetNplayers();
        Debug.Log("nplayer:" + nplayer);
        if (nplayer<=1)
        {
            CmdChangeColor( new Color(0.9f, 0.3f, 0.3f) );
        }
        //if (isLocalPlayer)
        //{
        //    transform.position = new Vector3(2 * (nplayer - 1), 0, 0);
        //}
        GetComponent<MeshRenderer>().material.color = defcolor;
    }
    public override void OnStartLocalPlayer()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<MeshRenderer>().material.color = defcolor;

        if (!isLocalPlayer) return;
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdFire();
        }
    }


    [Command]
    void CmdFire()
    {
        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);

        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;


        // Spawn the bullet on the Clients
        NetworkServer.Spawn(bullet);

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 4.0f);
    }
}
