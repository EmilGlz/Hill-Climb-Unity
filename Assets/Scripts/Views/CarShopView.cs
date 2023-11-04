using Scripts.Items;
using Scripts.Managers;
using Scripts.UI;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
namespace Scripts.Views
{
    public class CarShopView : View
    {
        public static string PrefabPath = "Prefabs/Views/ShopScrollView";
        private CarItemList _carItemList;

        public override async Task EnterView()
        {
            await base.EnterView();
            ScrollScaleController scrollView = gameObject.GetComponentInChildren<ScrollScaleController>();
            if (scrollView == null)
                scrollView = ResourceHelper.InstantiatePrefab(PrefabPath, transform).GetComponent<ScrollScaleController>();

            if (_carItemList != null)
            {
                PauseResumeView(true);
                return;
            }
            var content = Utils.FindGameObject("Content", scrollView.gameObject);
            content.transform.DestroyAllChildren();
            _carItemList = await CarItemList.CreateAsync(ItemController.instance.userData.ownedCars, content.transform, scrollView);
            Utils.RunAsync(() =>
            {
                scrollView.InitItems(_carItemList.Items);
                StartCoroutine(scrollView.ScrollTo(_carItemList.Items.FirstOrDefault(i => i.Data is CarData carData && carData == Settings.User.currentSelectedCar)));
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
