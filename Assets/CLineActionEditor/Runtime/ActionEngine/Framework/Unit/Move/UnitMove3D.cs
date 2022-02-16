using SuperCLine.ActionEngine;
using UnityEngine;

public class UnitMove3D : MonoBehaviour, IUnitMove
{
    public CharacterController Controller { get; private set; }
    public bool Enabled
    {
        get => enabled;
        set => enabled = value;
    }

    public bool CanJump => true;
    
    private const float SkinWidth = 0.05f;
    
    private void Awake()
    {
        Controller = gameObject.GetComponent<CharacterController>();
        if (Controller == null)
            Controller = gameObject.AddComponent<CharacterController>();
    }

    public void Move(Unit unit, Vector3 offset, ref bool mOnGround, ref bool mOnTouchSide)
    {
        if (unit.UnitType == EUnitType.EUT_Player)
        {
            if (offset.y != 0)
                mOnGround = false;
            if (unit.UController != null && enabled)
            {
                //float y = UUnit.transform.position.y;
                CollisionFlags collisionFlags = Controller.Move(offset);

                if ((collisionFlags & CollisionFlags.Below) == CollisionFlags.Below)
                    mOnGround = true;
                if ((collisionFlags & CollisionFlags.Sides) == CollisionFlags.Sides)
                    mOnTouchSide = true;
                else if (offset.x != 0f || offset.z != 0f)
                    mOnTouchSide = false;
            }
            else
            {
                unit.SetPosition(unit.Position + offset);
            }
        }
        else
        {
            if (unit.EnableCollision)
            {
                float radius2 = 2 * unit.Radius;
                // xoz
                if (offset.x != 0f && offset.z != 0f)
                {
                    Vector3 trans = new Vector3(offset.x, 0, offset.z);
                    Vector3 dir = trans.normalized;
                    float dist = trans.magnitude + radius2 + SkinWidth;
                    Vector3 origin = new Vector3(unit.Position.x, unit.Position.y + radius2, unit.Position.z) - dir * SkinWidth;

                    mOnTouchSide = false;
                    RaycastHit hitInfo;
                    if (Physics.Raycast(origin, dir, out hitInfo, dist, unit.ULayerMask))
                    {
                        mOnTouchSide = true;
                        float num = hitInfo.distance - radius2;

                        offset.x = (num > 0f ? dir.x * num : 0f);
                        offset.z = (num > 0f ? dir.z * num : 0f);

                    }
                }

                // y
                mOnGround = false;
                if (offset.y < 0f)
                {
                    Vector3 origin = new Vector3(unit.Position.x, unit.Position.y + radius2, unit.Position.z);
                    float dist = radius2 - offset.y;

                    RaycastHit hitInfo;
                    if (Physics.Raycast(origin, Vector3.down, out hitInfo, dist, unit.ULayerMask))
                    {
                        float num = hitInfo.distance - radius2;
                        offset.y = (Mathf.Abs(num) <= 0.001f ? 0f : -num);

                        mOnGround = true;
                    }
                }
            }
            else
            {
                mOnGround = false;
            }

            unit.SetPosition(unit.Position + offset);
        }
    }
}
