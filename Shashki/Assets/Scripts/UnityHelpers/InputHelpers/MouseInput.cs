namespace Assets.Scripts.UnityHelpers.InputHelpers
{
    public static class MouseInput
    {
        public static bool GetButtonDown(MouseButtonCode code)
        {
            return UnityEngine.Input.GetMouseButtonDown((int)code);
        }
    }
}
