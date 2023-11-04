namespace Scripts.Items
{
    public interface IDarker
    {
        public float DarkestAlphaValue { get; }
        public float LightestAlphaValue { get; }
        void UpdateOverlay(float value); // value 0 to 1
    }
}
