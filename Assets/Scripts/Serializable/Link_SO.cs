using UnityEngine;

[CreateAssetMenu(fileName = "XXX City", menuName = "ScriptableObjects/Link_SO", order = 1)]
public class Link_SO : ScriptableObject
{
    public Coordinates city_center;
    public company_link[] company_link;
}
