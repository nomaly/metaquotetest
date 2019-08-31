namespace GeobaseModel.Comparers
{
    public interface IGComparer<TKey, TCompare>
    {
        int Compare(TKey key, TCompare target);
    }
}
