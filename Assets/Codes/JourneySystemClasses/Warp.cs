using UnityEngine;
using System.Collections;

public class Warp : MonoBehaviour {
    
    [SerializeField]
    private Transform warpTarget = null;

    IEnumerator OnTriggerEnter2D(Collider2D otherCollider)
    {
        Transform collTransform = otherCollider.gameObject.transform.parent;
        if (collTransform.tag == "Player" && enabled)
        {
            ScreenFader l_ScreenFader = JourneySystem.GetInstance().panelManager.screenFader;

            collTransform.GetComponent<JourneyPlayer>().SetActive(false);

            yield return StartCoroutine(l_ScreenFader.FadeToBlack());

            collTransform.position = warpTarget.position;
            Camera.main.transform.position = warpTarget.position;

            collTransform.GetComponent<JourneyPlayer>().SetActive(true);

            yield return StartCoroutine(l_ScreenFader.FadeToClear());
        }
        else yield break;
    }
}
