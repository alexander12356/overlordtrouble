using UnityEngine;
using System.Collections;

public class House : MonoBehaviour
{
#if UNITY_EDITOR
    [ContextMenu("GenerateSimpleHouse")]
    private void GenerateBaseObjects()
    {
        GameObject l_RendererObject = new GameObject();
        l_RendererObject.name = "Renderer";
        l_RendererObject.AddComponent<SpriteRenderer>();
        l_RendererObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
        l_RendererObject.transform.SetParent(transform);
        l_RendererObject.transform.localPosition = Vector3.zero;

        GameObject l_DoorBehindRenderer = new GameObject();
        l_DoorBehindRenderer.name = "BehindRenderer";
        l_DoorBehindRenderer.AddComponent<SpriteRenderer>();
        l_DoorBehindRenderer.transform.SetParent(transform);
        l_DoorBehindRenderer.transform.localPosition = Vector3.zero;

        GameObject l_ColliderObject = new GameObject();
        l_ColliderObject.name = "Collider";
        l_ColliderObject.AddComponent<PolygonCollider2D>();
        l_ColliderObject.transform.SetParent(transform);
        l_ColliderObject.transform.localPosition = Vector3.zero;

        GameObject l_Door = new GameObject();
        l_Door.name = "Door";
        l_Door.AddComponent<FrontDoor>();
        l_Door.transform.SetParent(transform);
        l_Door.transform.localPosition = Vector3.zero;

        GameObject l_DoorFrontRenderer = new GameObject();
        l_DoorFrontRenderer.name = "FrontRenderer";
        l_DoorFrontRenderer.AddComponent<SpriteRenderer>();
        l_DoorFrontRenderer.GetComponent<SpriteRenderer>().sortingOrder = 1;
        l_DoorFrontRenderer.transform.SetParent(l_Door.transform);
        l_DoorFrontRenderer.transform.localPosition = Vector3.zero;

        GameObject l_CheckCollider = new GameObject();
        l_CheckCollider.name = "CheckCollider";
        l_CheckCollider.AddComponent<BoxCollider2D>();
        l_CheckCollider.GetComponent<BoxCollider2D>().isTrigger = true;
        l_CheckCollider.AddComponent<CheckCollide>();
        l_CheckCollider.transform.SetParent(l_Door.transform);
        l_CheckCollider.transform.localPosition = Vector3.zero;

        GameObject l_LayerChecker = new GameObject();
        l_LayerChecker.name = "LayerChecker";
        l_LayerChecker.AddComponent<LayerChecker>();
        l_LayerChecker.transform.SetParent(transform);
        l_LayerChecker.transform.localPosition = Vector3.zero;

        GameObject l_Upper = new GameObject();
        l_Upper.name = "Upper";
        l_Upper.AddComponent<BoxCollider2D>();
        l_Upper.GetComponent<BoxCollider2D>().isTrigger = true;
        l_Upper.AddComponent<CheckCollide>();
        l_Upper.transform.SetParent(l_LayerChecker.transform);
        l_Upper.transform.localPosition = Vector3.zero;

        GameObject l_Warps = new GameObject();
        l_Warps.name = "Warps";
        l_Warps.transform.SetParent(transform);
        l_Warps.transform.localPosition = Vector3.zero;

        GameObject l_WarpTarget = new GameObject();
        l_WarpTarget.name = "WarpTarget";
        l_WarpTarget.transform.SetParent(l_Warps.transform);
        l_WarpTarget.transform.localPosition = Vector3.zero;

        GameObject l_WarpExit = new GameObject();
        l_WarpExit.name = "WarpExit";
        l_WarpExit.AddComponent<BoxCollider2D>();
        l_WarpExit.GetComponent<BoxCollider2D>().isTrigger = true;
        l_WarpExit.AddComponent<Warp>();
        l_WarpExit.transform.SetParent(l_Warps.transform);
        l_WarpExit.transform.localPosition = Vector3.zero;
    }
#endif
}
