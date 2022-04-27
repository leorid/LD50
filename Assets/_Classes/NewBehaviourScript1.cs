using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Classes
{
	public class NewBehaviourScript1 : MonoBehaviour
	{

		// Use this for initialization
		void Start()
		{
   //         IEnumerator<int> enumerator = GetE();

			//while (enumerator.MoveNext())
			//{
   //             var x = enumerator.Current;
			//}

   //         foreach (var item in enumerator)
			//{

			//}
        }

        IEnumerator<int> GetE() { yield return 1; }

        // Update is called once per frame
        void Update()
		{

		}
	}
}


    // Enemy Class
    public class Enemy : MonoBehaviour
    {
    	float currentDetection;
    	void Update()
    	{
    		UIDetectionManager.SetInfo(gameObject, new DetectionInfo()
    		{
    			detection = currentDetection,
    			position = transform.position,
    		});
    	}
    }
    
    // the helper struct to store required info for the UI
    public struct DetectionInfo
    {
    	public float detection;
    	public Vector3 position;
    }
    
    // the static class to get infos from all enemies and collect them
    public static class UIDetectionManager
    {
    	public static Dictionary<GameObject, DetectionInfo> detectionInfos =     new();
    
    	public static void SetInfo(GameObject handle, DetectionInfo info)
    	{
    		detectionInfos[handle] = info;
    	}
    }
    
    // this is on the UI Canvas Object
    public class UI : MonoBehaviour
    {
    	Camera _mainCamera;
    	private void Start() => _mainCamera = Camera.main;
    	private void Update()
    	{
    		foreach(var info in UIDetectionManager.detectionInfos)
    		{
    			Vector3 screenPosition = _mainCamera.WorldToScreenPoint(info.Value.position);
    			// Spawn or get UI Detection Prefab to display above the head of the enemy
    			// like:
    			// var marker = GetDetectionMarkerForGO(info.Key);
    			// set correct position based on screenPosition 
    		}
    	}
    }