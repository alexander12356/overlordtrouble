using UnityEngine;
using System.Collections;

public class Warp : MonoBehaviour {

    [SerializeField]
    private Transform warpTarget = null;

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        Debug.Log("An object collided");
        otherCollider.gameObject.transform.parent.transform.position = warpTarget.position;
        // Camera.main.transform.position = warpTarget.position;
    }
}
