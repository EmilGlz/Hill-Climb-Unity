using System;

public class PausePopup : Popup
{
    private const string prefabPath = "Prefabs/Popups/PausePopup";

    public PausePopup(Action onClose = null, bool addBackground = true, bool closeOnBackground = true) 
        : base(prefabPath, onClose, addBackground, closeOnBackground)
    {
    }

    public static void Create(Action onClose = null, bool addBackground = true, bool closeOnBackground = true)
    {
        if (PopupsManager.IsShown<PausePopup>())
            return;
        var popup = new PausePopup(onClose, addBackground, closeOnBackground);
        popup.Show();
    }
    protected override void Show()
    {
        base.Show();
    }
    public override void Dispose()
    {
        base.Dispose();
    }
}
