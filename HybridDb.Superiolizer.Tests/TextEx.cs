namespace HybridDb.Superiolizer.Tests
{
    public static class TextEx
    {
        public static TObj Roundtrip<TObj>(this Superiolizer superiolizer, TObj obj)
        {
            return (TObj)superiolizer.Deserialize(superiolizer.Serialize(obj), typeof(TObj));
        }
    }
}