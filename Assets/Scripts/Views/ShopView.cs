using System.Threading.Tasks;
using UnityEngine;

namespace Scripts.Views
{
    public class ShopView : View
    {
        private const string ShopNotReadyPrefab = "Prefabs/Views/ShopNotReadyCaption";
        public override async Task EnterView()
        {
            await base.EnterView();
            var notReadyText = await ResourceHelper.InstantiatePrefabAsync(ShopNotReadyPrefab, transform);
            var rect = notReadyText.GetComponent<RectTransform>();
            rect.anchorMax = new Vector2(1, .5f);
            rect.anchorMin = new Vector2(0, .5f);
            rect.SetHeight(60);
            rect.SetPosY(0);
        }
    }
}