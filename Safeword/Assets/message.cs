using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class message : MonoBehaviour {

    public GameObject textObject;
    
	// Use this for initialization
	void Start () {
		 SetText("Velkommen til en fager ny verden af fliser");
	}
    
    public void SetText(string str){
        textObject.GetComponent<Text>().text = str;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}



//dø din gammelnok