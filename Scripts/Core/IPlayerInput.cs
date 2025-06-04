public interface IPlayerInput
{
    float GetHorizontal();
    //float GetVertical();
    bool IsJumpInput();
    bool IsJumpInputReleased();
}