//#define ASTARDEBUG
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class NavMeshRenderer : MonoBehaviour {
	
	/** Last level loaded. Used to check for scene switches */
	string lastLevel = "";
	
	/** Used to get rid of the compiler warning that #lastLevel is not used */
	public string SomeFunction () {
		return lastLevel;
	}
	
	// Update is called once per frame
	void Update () {
		#if UNITY_EDITOR
		if (lastLevel == "") {
			lastLevel = EditorApplication.currentScene;
		}
		
		if (lastLevel != EditorApplication.currentScene) {
#if ASTARDEBUG
			Debug.Log ("Level change "+lastLevel+" --> "+EditorApplication.currentScene);
#endif
			DestroyImmediate (gameObject);
		}
		#endif
	}
}