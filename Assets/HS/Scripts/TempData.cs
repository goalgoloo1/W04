using UnityEngine; //For the Sprite variable type
public static class TempData
{
    public static GameController.CameraMonitor playerViewingCamera { get; set; } //Used in various locations to determine which camera the player is monitoring
    public static bool playerWon { get; set; }          //Set at the start of the win sequence to prevent various things from happening. Used by the main menu to determine player progress
    public static bool dying { get; set; }              //Set at the start of a jumpscare to prevent various things from happening

}