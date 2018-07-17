using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneInit : MonoBehaviour {

	// Use this for initialization
	void Start () {
		for (int i = -10; i<10; i++)
        {
            var gox = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            float rad = (20f + i) / 100.0f;
            gox.transform.localScale = new Vector3(rad,rad,rad);
            gox.GetComponent<SphereCollider>().radius = rad;
            gox.transform.parent = transform;
            gox.transform.position = new Vector3(i, 0, 0);
            gox.GetComponent<MeshRenderer>().material.color = Color.red;
            //Debug.Log("Created sphere");
            var goz = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            goz.transform.localScale = new Vector3(rad,rad,rad);
            goz.GetComponent<SphereCollider>().radius = rad;
            goz.transform.parent = transform;
            goz.transform.position = new Vector3(0, 0, i);
            goz.GetComponent<MeshRenderer>().material.color = Color.blue;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
