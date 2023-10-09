using UnityEngine;

[CreateAssetMenu(fileName = "userFeedback", menuName = "ScriptableObjects/Feedback_SO", order = 1)]
public class Feedback_SO : ScriptableObject {
    public int[] questions;
    public string comment;  //contains the string of the provided comment
}
