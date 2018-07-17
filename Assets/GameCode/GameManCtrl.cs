using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManCtrl : MonoBehaviour {

    // Use this for initialization
    private int _nplayers = 0;
    public int nplayers { get { return nplayers; } }

    public int incNplayers()
    {
        _nplayers++;
        return (_nplayers);
    }
    public int GetNplayers()
    {
        return (_nplayers);
    }

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
