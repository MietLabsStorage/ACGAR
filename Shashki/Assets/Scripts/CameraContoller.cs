using Assets.Scripts.UnityHelpers.InputHelpers;
using UnityEngine;

public class CameraContoller : MonoBehaviour
{
    public float rotateSpeed = 5000.0f;
    public float speed = 5.0f;
    public float boostValue = 2f;
    public float jumpStartBoost = 50f;

    private bool isJump = false;
    private float jumpBoost;

    public CameraContoller()
    {
        jumpBoost = jumpStartBoost;
    }

    private void Update()
    {
        var horizontal = Input.GetAxis(AxisConsts.Horizontal);
        var vertical = Input.GetAxis(AxisConsts.Vertical);
        var boost = Input.GetKey(KeyCode.LeftShift) ? boostValue : 1f;

        var commonMultiplier = boost * Time.deltaTime;

        var rotate = rotateSpeed * Input.GetAxis(AxisConsts.MouseScrollWheel) * commonMultiplier * Vector3.up;
        transform.Rotate(rotate, Space.World);
        transform.Translate(commonMultiplier * speed * new Vector3(horizontal, 0, vertical), Space.Self);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJump = true;
        }

        if (isJump)
        {
            if (jumpBoost <= 0)
            {
                isJump = false;
                jumpBoost = jumpStartBoost;
            }
            else
            {
                jumpBoost -= 1;
                var addY = jumpBoost * commonMultiplier;
                addY = transform.position.y + addY < 0 ? transform.position.y : addY;
                transform.position += transform.up * addY;
            }
        }
    }
}
