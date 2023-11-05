using System.Threading.Tasks;
using TMPro;

namespace Scripts.Views
{
    public class GameOverView : View
    {
        public override Task EnterView()
        {
            gameObject.SetActive(true);
            return base.EnterView();
        }

        public void Init(int distance, int coin)
        {
            var titleText = Utils.FindGameObject("TitleText", gameObject).GetComponent<TMP_Text>();
            var distanceText = Utils.FindGameObject("DistanceText", gameObject).GetComponent<TMP_Text>();
            var coinText = Utils.FindGameObject("CoinText", gameObject).GetComponent<TMP_Text>();
            titleText.text = "Driver Down";
            distanceText.text = distance.ToString() + " m";
            coinText.text = coin.ToString();
        }

        public override void ExitView()
        {
            gameObject.SetActive(false);
            base.ExitView();
        }
    }
}
