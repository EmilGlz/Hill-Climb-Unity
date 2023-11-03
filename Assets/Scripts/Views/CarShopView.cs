using Scripts.Items;
using Scripts.Managers;
using Scripts.UI;
namespace Scripts.Views
{
    public class CarShopView : View
    {
        public static string PrefabPath = "Prefabs/Views/ShopScrollView";
        private CarItemList _carItemList;

        public override void EnterView()
        {
            ScrollScaleController scrollView = gameObject.GetComponentInChildren<ScrollScaleController>();
            if (scrollView == null)
                scrollView = ResourceHelper.InstantiatePrefab(PrefabPath, transform).GetComponent<ScrollScaleController>();

            if (_carItemList != null)
            {
                PauseResumeView(true);
                return;
            }
            base.EnterView();
            var content = Utils.FindGameObject("Content", scrollView.gameObject);
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

            ScrollScaleController scrollView = gameObject.GetComponentInChildren<ScrollScaleController>();
            if (scrollView != null)
                Destroy(scrollView.gameObject);
        }
    }
}
