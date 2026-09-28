using UnityEngine;
using UnityEngine.InputSystem;

public class TestController : Controller<TestMovement> 
{
    private Vector2 _input;
    private float _jump;
    public void OnMove(InputAction.CallbackContext ctx)
    {
        Debug.Log("Move");
        _input = ctx.ReadValue<Vector2>();
        _controllerPort.ExecuteAction(this, Move);
    }
    private void Move( TestMovement movement)
    {
        movement.Move(_input);
    }
    public void OnJump(InputAction.CallbackContext ctx)
    {
        Debug.Log("Jump");
        _jump = ctx.ReadValue<float>();
        _controllerPort.ExecuteAction(this, Jump);
    }
    private void Jump(TestMovement movement)
    {
        movement.Jump(_jump);
    }
  
       
}
