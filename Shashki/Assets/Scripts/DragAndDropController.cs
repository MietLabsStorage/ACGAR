using Assets.Scripts.UnityHelpers;
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
        var isGround = Physics.Raycast(ray, out var groundHit, 20f, LayerMask.GetMask(Layers.GroundLayer));
        var isCkeck = Physics.Raycast(ray, out var checkHit, 20f, LayerMask.GetMask(Layers.Check));

        if (isGround || isCkeck)
        {
            var hit = isCkeck ? checkHit : groundHit;
            var isQueen = DraggedObject.transform.rotation.x == 1;
            Debug.Log(DraggedObject.transform.rotation.x);
            var addedY = isQueen ? 0.85f : 0;
            DraggedObject.transform.position = new Vector3(hit.point.x, hit.point.y + 1 + addedY, hit.point.z);

            if (hit.collider.tag == Tags.WhiteCheck && MouseInput.GetButtonDown(MouseButtonCode.Left))
            {
                DraggedObject.transform.position = new Vector3(hit.point.x, hit.point.y + addedY, hit.point.z);
                DraggedObject.GetComponent<MeshRenderer>().material.color = new Color(
                    DraggedObjectOldColor.r,
                    DraggedObjectOldColor.g,
                    DraggedObjectOldColor.b,
                    DraggedObjectOldColor.a
                );
                DraggedObject = null;
            }
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

        if (Input.GetKeyDown(KeyCode.U))
        {
            DraggedObject.transform.rotation = new Quaternion(DraggedObject.transform.rotation.x == 1 ? 0 : 1, 0, 0, 0);
        }
    }

    private void DoWhenDropped(Ray ray)
    {
        if (Physics.Raycast(ray, out var hit, 20f, LayerMask.GetMask("DragableLayer")))
        {
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