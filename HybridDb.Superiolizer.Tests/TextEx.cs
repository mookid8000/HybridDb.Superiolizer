namespace HybridDb.Superiolizer.Tests
{
    public static class TextEx
    {
        public static TObj Roundtrip<TObj>(this Superiolizer superiolizer, TObj obj)
        {
            string dummy;
            return superiolizer.Roundtrip(obj, out dummy);
        }

        public static TObj Roundtrip<TObj>(this Superiolizer superiolizer, TObj obj, out string jsonText)
        {
            var bytes = superiolizer.Serialize(obj);

            jsonText = superiolizer.Encoding.GetString(bytes);

            return (TObj)superiolizer.Deserialize(bytes, typeof(TObj));
        }
    }
}