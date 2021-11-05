using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour {

	public enum levelTypes {Platformer, Runner};
	public levelTypes levelType;

	public bool sceneActive;



    void Start() {
        sceneActive = true;
    }


    void Update() {
        
    }
}
