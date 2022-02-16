using SuperCLine.ActionEngine;
using UnityEngine;

public class UnitMove2D : MonoBehaviour, IUnitMove
{
    public CharacterController Controller { get; private set; }
    public bool Enabled
    {
        get => enabled;
        set => enabled = value;
    }

    public bool CanJump => false;

    private bool _mFacingRight = true;
    
    private void Awake()
    {
        Controller = gameObject.GetComponent<CharacterController>();
        if (Controller == null)
            Controller = gameObject.AddComponent<CharacterController>();
    }
    
    public void Move(Unit unit, Vector3 offset, ref bool mOnGround, ref bool mOnTouchSide)
    {
        if (CanJump)
        {
            offset.z = 0; // 丢弃forward方向
            
            if (unit.UnitType == EUnitType.EUT_Player)
            {
                if (offset.y != 0)
                    mOnGround = false;
                if (unit.UController != null && enabled)
                {
                    CollisionFlags collisionFlags = Controller.Move(offset);
                    if ((collisionFlags & CollisionFlags.Below) == CollisionFlags.Below)
                        mOnGround = true;
                    if ((collisionFlags & CollisionFlags.Sides) == CollisionFlags.Sides)
                        mOnTouchSide = true;
                    else if (offset.x != 0f || offset.y != 0f)
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
                        float dist = trans.magnitude + radius2;
                        Vector3 origin = new Vector3(unit.Position.x, unit.Position.y + radius2, unit.Position.z);

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
        else
        {
            // 如果没有跳跃功能，则为XY移动
            offset.y = offset.z;
            offset.z = 0;
            unit.SetPosition(unit.Position + offset);
        }
        ww
        if (offset.x > 0 && !_mFacingRight)
        {
            Flip();
        }
        else if (offset.x < 0 && _mFacingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        _mFacingRight = !_mFacingRight;
        var trans = transform;
        var theScale = trans.localScale;
        theScale.x *= -1;
        trans.localScale = theScale;
    }
}