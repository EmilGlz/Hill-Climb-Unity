using System;
using System.Collections.Generic;

public class PopupsManager
{
    private static PopupsManager _instance;

    private readonly List<Popup> _activePopups;

    private PopupsManager()
    {
        _activePopups = new List<Popup>();
    }

    public static PopupsManager instance => _instance ?? (_instance = new PopupsManager());

    public static bool IsShown<T>()
        where T : Popup
    {
        return instance.Any(p => p is T);
    }

    public void AddPopup(Popup popup)
    {
        _activePopups.Add(popup);
    }

    public void RemovePopup(Popup popup)
    {
        if (_activePopups.Contains(popup))
            _activePopups.Remove(popup);
    }

    public bool IsAnyActive()
    {
        CleanupDestroyedPopups();
        return _activePopups?.Count > 0;
    }

    public bool Any(Predicate<Popup> predicate)
    {
        CleanupDestroyedPopups();
        return _activePopups?.Exists(predicate) == true;
    }

    public T GetActive<T>() where T : Popup
    {
        CleanupDestroyedPopups();
        foreach (var popup in _activePopups)
            if (popup is T resultPopup)
                return resultPopup;

        return default;
    }

    private void CleanupDestroyedPopups()
    {
        DisposeAll(popup => popup.ItemTemplate == null);
    }

    public void DisposeAll(Predicate<Popup> predicate = null)
    {
        var popups = _activePopups.ToArray();
        foreach (var popup in popups)
        {
            if (popup.ItemTemplate != null)
            {
                if (predicate != null && !predicate(popup))
                    continue;
            }
            popup.Dispose();
            _activePopups.Remove(popup);
        }
    }

    public void DisposePopup<T>() where T : Popup
    {
        var activePopup = GetActive<T>();
        if (activePopup != null)
            activePopup.Dispose();
    }

    //public void HidePopup<T>() where T : Popup
    //{
    //    var activePopup = GetActive<T>();
    //    if (activePopup != null)
    //        activePopup.HidePopup();
    //}


    /// <summary>
    ///     destroy the all active popups immediately without an animation
    /// </summary>
    public void DestroyAll()
    {
        var popups = _activePopups.ToArray();
        foreach (var popup in popups)
            popup.Dispose();
        _activePopups.Clear();
    }

    /// <summary>
    ///     destroy the all active popups immediately without an animation
    /// </summary>
    public void DestroyLast()
    {
        var lastPopup = GetActivePopup();
        if (lastPopup != null)
        {
            _activePopups.Remove(lastPopup);
            lastPopup.Dispose();
        }
    }

    /// <summary>
    ///     destroy the all active popups immediately without an animation
    /// </summary>
    public Popup GetActivePopup()
    {
        CleanupDestroyedPopups();
        if (_activePopups.Count > 0)
        {
            var lastPopup = _activePopups[_activePopups.Count - 1];
            return lastPopup;
        }
        return null;
    }

}
