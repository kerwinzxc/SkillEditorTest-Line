using SuperCLine.ActionEngine;
using UnityEngine;

public interface IUnitMove
{
    public CharacterController Controller { get; }
    public bool Enabled { get; set; }
    public bool CanJump { get; }
    void Move(Unit unit, Vector3 offset, ref bool mOnGround, ref bool mOnTouchSide);
}