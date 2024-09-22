using UnityEngine;

namespace RPG
{
    public class NoteContainer : MonoBehaviour, IHasNotes
    {
        [TextArea(3, 10)]
        [SerializeField] protected string contents = string.Empty;

        public virtual string Notes { get { return contents; } }
    } 
}
