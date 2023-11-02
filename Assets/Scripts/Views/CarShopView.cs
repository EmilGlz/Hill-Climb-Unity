using Scripts.Items;
using Scripts.Managers;
using Scripts.UI;
using Unity.VisualScripting;
namespace Scripts.Views
{
    public class CarShopView : View
    {
        public static string PrefabPath = "Prefabs/Views/CarShopView";
        private CarItemList _carItemList;

        public override void EnterView()
        {
            if (!gameObject.TryGetComponent(out ScrollScaleController val))
                val = ResourceHelper.InstantiatePrefab(PrefabPath, transform).GetComponent<ScrollScaleController>();
            if(_carItemList != null)
            {
                PauseResumeView(true);
                return;
            }
            base.EnterView();
            var content = Utils.FindGameObject("Content", val.gameObject);
            _carItemList = new CarItemList(ItemController.instance.userData.ownedCars, content.transform);
            Utils.RunAsync(() =>
            {
                ScrollScaleController.instance.InitItems();
            });
        }

        public override void ExitView()
        {
            if (_carItemList != null)
            {
                _carItemList.Dispose();
                _carItemList = null;
            }
        }
    }
}
