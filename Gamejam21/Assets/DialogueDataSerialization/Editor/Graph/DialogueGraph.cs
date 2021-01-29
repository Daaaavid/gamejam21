using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine;
using System.Linq;
using Subtegral.DialogueSystem.DataContainers;

    public class DialogueGraph : EditorWindow
{
    private DialogueGraphView _graphView;
    private string _fileName = "new Narrative";

    [MenuItem("Graph/DIalogue Graph")]
    public static void OpenGraphWindow() {
        var window = GetWindow<DialogueGraph>();
        window.titleContent = new UnityEngine.GUIContent("Dialogue Graph");
    }

    private void OnEnable() {
        ConstructGraphView();
        GenerateToolBar();
        GenerateMiniMap();
        GenerateBlackBoard();
    }

    private void GenerateBlackBoard() {
        var blackboard = new Blackboard(_graphView);
        blackboard.Add(new BlackboardSection { title = "Doesn't work"}); //Exposed Properties 
        blackboard.addItemRequested = _blackboard => { _graphView.AddPoropertyToBlackBoard(new ExposedProperty()); };
        blackboard.editTextRequested = (blackboard1, element, newValue) => {
            var oldPropertyName = ((BlackboardField)element).text;
            if (_graphView.ExposedProperties.Any(x => x.PropertyName == newValue)) {
                EditorUtility.DisplayDialog("Error", "This property name already exists, please choose another one!", ok: "ok");
                return;
            }

            var propertyIndex = _graphView.ExposedProperties.FindIndex(x => x.PropertyName == oldPropertyName);
            _graphView.ExposedProperties[propertyIndex].PropertyName = newValue;
            ((BlackboardField)element).text = newValue;

        };

        blackboard.SetPosition(new Rect(x: 10, y: 30, width: 200, height: 300));
        _graphView.Add(blackboard);
        _graphView.blackboard = blackboard;
    }

    private void ConstructGraphView() {
        _graphView = new DialogueGraphView(this) {
            name = "Dialogue Graph"
        };
        _graphView.StretchToParentSize();
        rootVisualElement.Add(_graphView);
    }

    private void GenerateToolBar() {
        var toolbar = new Toolbar();

        var fileNameTextField = new TextField(label: "File Name");
        fileNameTextField.SetValueWithoutNotify(_fileName);
        fileNameTextField.MarkDirtyRepaint();
        fileNameTextField.RegisterCallback<ChangeEvent<string>>(evt => _fileName = evt.newValue);
        toolbar.Add(fileNameTextField);

        toolbar.Add(child: new Button(clickEvent: () => RequestDataOperation(true)) {text = "Save Data" });
        toolbar.Add(child: new Button(clickEvent: () => RequestDataOperation(false)) { text = "Load Data" });

        rootVisualElement.Add(toolbar);
    }


    private void GenerateMiniMap() {
        var minimap = new MiniMap { anchored = true };
        // this will give 10 px offset from left side
        var cords = _graphView.contentViewContainer.WorldToLocal(p: new Vector2(this.maxSize.x -10, y: 30));//
        Debug.Log(this.minSize.x);
        minimap.SetPosition(new Rect(cords.x, cords.y, width: 200, height: 140));
        _graphView.Add(minimap);
    }

    private void RequestDataOperation(bool save) {
        if (string.IsNullOrEmpty(_fileName)) {
            EditorUtility.DisplayDialog("Invalid file name!", message: "Please enter a valid file name.", ok: "OK");
        }

        var saveUtility = GraphSaveUtility.GetInstance(_graphView);

        if (save) {;
            saveUtility.SaveGraph(_fileName);
        } else {
            saveUtility.LoadGraph(_fileName);
        }
    }

    private void OnDisable() {
        rootVisualElement.Remove(_graphView);
    }


}
