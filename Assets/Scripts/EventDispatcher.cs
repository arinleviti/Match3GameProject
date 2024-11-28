using System;

public static class EventDispatcher
{
    public static event Action DisableInputEvent;
    public static event Action EnableInputEvent;

    public static void TriggerDisableInput()
    {
        DisableInputEvent?.Invoke();
    }

    public static void TriggerEnableInput()
    {
        EnableInputEvent?.Invoke();
    }
}
