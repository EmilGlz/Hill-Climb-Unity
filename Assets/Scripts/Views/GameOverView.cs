using System.Threading.Tasks;

namespace Scripts.Views
{
    public class GameOverView : View
    {
        public override Task EnterView()
        {
            gameObject.SetActive(true);
            return base.EnterView();
        }

        public override void ExitView()
        {
            gameObject.SetActive(false);
            base.ExitView();
        }
    }
}
