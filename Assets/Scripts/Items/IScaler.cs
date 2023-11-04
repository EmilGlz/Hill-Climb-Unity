namespace Scripts.Items
{
    public interface IScaler
    {
        public float MinimumScale { get; }
        public float MaximumScale { get; }
        public void UpdateScale(float scaleValue);
    }
}
