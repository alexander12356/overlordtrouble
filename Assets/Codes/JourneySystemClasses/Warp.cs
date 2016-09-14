using UnityEngine;
using System.Collections;

public class Warp : MonoBehaviour {

    [SerializeField]
    private Transform warpTarget = null;

    IEnumerator OnTriggerEnter2D(Collider2D otherCollider)
    {
        Transform collTransform = otherCollider.gameObject.transform.parent;
        if (collTransform.tag == "Player")
        {
            ScreenFader scrFader = GameObject.FindGameObjectWithTag("Fader").GetComponent<ScreenFader>();

            collTransform.GetComponent<JourneyPlayer>().SetActive(false);

            yield return StartCoroutine(scrFader.FadeToBlack());

            collTransform.position = warpTarget.position;
            Camera.main.transform.position = warpTarget.position;

            collTransform.GetComponent<JourneyPlayer>().SetActive(true);

            yield return StartCoroutine(scrFader.FadeToClear());
        }
        else yield break;
    }
}
