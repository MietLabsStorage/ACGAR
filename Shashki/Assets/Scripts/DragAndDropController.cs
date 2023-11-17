using Assets.Scripts.UnityHelpers.InputHelpers;
using UnityEngine;

public class DragAndDropController : MonoBehaviour
{
    private GameObject DraggedObject { get; set; }
    private Vector3 DraggedObjectOldPosition { get; set; }
    private Color DraggedObjectOldColor { get; set; }

    void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (DraggedObject != null)
        {
            DoWhenDragged(ray);
        }
        else
        {
            DoWhenDropped(ray);
        }
    }

    private void DoWhenDragged(Ray ray)
    {
        if (Physics.Raycast(ray, out var hit, 1000f, LayerMask.GetMask("GroundLayer")))
        {
            DraggedObject.transform.position = new Vector3(hit.point.x, hit.point.y + 1, hit.point.z);
        }

        if (MouseInput.GetButtonDown(MouseButtonCode.Left))
        {
            DraggedObject.transform.position = new Vector3(hit.point.x, hit.point.y - 0.2f, hit.point.z);
            DraggedObject.GetComponent<MeshRenderer>().material.color = new Color(
                DraggedObjectOldColor.r,
                DraggedObjectOldColor.g,
                DraggedObjectOldColor.b,
                DraggedObjectOldColor.a
            );
            DraggedObject = null;
        }

        if (MouseInput.GetButtonDown(MouseButtonCode.Right) || Input.GetKeyDown(KeyCode.Escape))
        {
            DraggedObject.transform.position = DraggedObjectOldPosition;
            DraggedObject.GetComponent<MeshRenderer>().material.color = new Color(
                DraggedObjectOldColor.r,
                DraggedObjectOldColor.g,
                DraggedObjectOldColor.b,
                DraggedObjectOldColor.a
            );
            DraggedObject = null;
        }
    }

    private void DoWhenDropped(Ray ray)
    {
        if (Physics.Raycast(ray, out var hit, 1000f, LayerMask.GetMask("DragableLayer")))
        {
            Debug.Log(1);
            if (MouseInput.GetButtonDown(MouseButtonCode.Left))
            {
                DraggedObject = hit.collider.gameObject;
                DraggedObjectOldPosition = DraggedObject.transform.position;
                var color = DraggedObject.GetComponent<MeshRenderer>().material.color;
                DraggedObjectOldColor = new Color(color.r, color.g, color.b, color.a);
                DraggedObject.GetComponent<MeshRenderer>().material.color = new Color(0.5f, 0.1f, 0.3f, color.a);
            }
        }
    }
}