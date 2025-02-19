using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UIElements;

public class Mainmenu : MonoBehaviour
{
    private UIDocument _Doc;
    private Button Button;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _Doc = GetComponent<UIDocument>();
        Button = _Doc.rootVisualElement.Q("AButton") as Button;
        Button.RegisterCallback<ClickEvent>(OnPlayGameClick);


    }

    // Update is called once per frame
private void OnPlayGameClick(ClickEvent evt){
    Debug.Log("sdad");
}
}
