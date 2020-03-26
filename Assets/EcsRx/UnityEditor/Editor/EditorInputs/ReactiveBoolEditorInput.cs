using UniRx;
using UnityEditor;

namespace EcsRx.UnityEditor.Editor.EditorInputs
{
    public class ReactiveBoolEditorInput : SimpleEditorInput<BoolReactiveProperty>
    {
        protected override BoolReactiveProperty CreateTypeUI(string label, BoolReactiveProperty value)
        {
            value.Value = EditorGUILayout.Toggle(label, value.Value);
            return null;
        }
    }
}