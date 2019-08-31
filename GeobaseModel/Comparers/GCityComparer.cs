namespace GeobaseModel.Comparers
{
    public class GCityComparer : IGComparer<string, GLocation>
    {
        public int Compare(string key, GLocation target)
            => string.Compare(key.Trim(), target.City.Trim());
    }
}
