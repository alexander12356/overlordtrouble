using UnityEngine;

public static class ControlSystem
{
    public static bool ExitButton()
    {
        if (Input.GetKeyUp(KeyCode.X) || Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape))
        {
            return true;
        }
        return false;
    }

    public static bool EnterButton()
    {
        if (Input.GetKeyUp(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            return true;
        }
        return false;
    }
}
