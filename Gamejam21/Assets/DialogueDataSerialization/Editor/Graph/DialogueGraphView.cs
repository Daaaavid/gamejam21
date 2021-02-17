using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using Subtegral.DialogueSystem.DataContainers;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

public class DialogueGraphView : GraphView
{
    public readonly Vector2 defeaultNodeSize = new Vector2(x: 100, y:150);

    public Blackboard blackboard;
    public List<ExposedProperty> ExposedProperties = new List<ExposedProperty>();
    private NodeSearchWindow _searchWindow;

    public DialogueGraphView(EditorWindow editorWindow) {

        styleSheets.Add(styleSheet: Resources.Load<StyleSheet>(path: "DialogueGraph"));
        SetupZoom(ContentZoomer.DefaultMinScale,ContentZoomer.DefaultMaxScale);

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var grid = new GridBackground();
        Insert(index: 0, grid);
        grid.StretchToParentSize();

        AddElement(GenerateEntryPointNode());
        AddSearchWindow(editorWindow);
    }

    private void AddSearchWindow(EditorWindow editorWindow) {
        _searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
        _searchWindow.Init(editorWindow, this);
       nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);

    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter) {
        var compatiblePorts = new List<Port>();

        ports.ForEach(funcCall: (port) => {
            var portView = port;
            if (startPort != port && startPort.node != port.node)
                compatiblePorts.Add(port);
        });

        return compatiblePorts;
    }

    private Port GeneratePort(DialogueNode node, Direction portDirection, Port.Capacity capacity = Port.Capacity.Single) {
        return node.InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(float)); // arbitrary type
    }

    private DialogueNode GenerateEntryPointNode() {
        var node = new DialogueNode() {
            title = "start",
            GUID = Guid.NewGuid().ToString(),
            dialogueText = "ENTRYPOINT",
            entryPoint = true
        };

        var generatedPort = GeneratePort(node, Direction.Output);
        generatedPort.portName = "Next";
        node.outputContainer.Add(generatedPort);

        node.capabilities &= ~Capabilities.Movable;
        node.capabilities &= ~Capabilities.Deletable;

        node.RefreshExpandedState();
        node.RefreshPorts();

        node.SetPosition(new Rect(x: 100, y: 100, width: 100, height: 150));
        return node;
    }

    public void CreateNode(string nodeName, Vector2 position, int type, bool succes) {
        AddElement(CreateDialogueNode(nodeName, position, type, succes));
    }

    public DialogueNode CreateDialogueNode(string nodeName, Vector2 position, int type, bool succes) {
        var dialogueNode = new DialogueNode() {
            title = nodeName,
            dialogueText = nodeName,
            GUID = Guid.NewGuid().ToString(),
            type = type,
            succes = succes
        };

        var inputPort = GeneratePort(dialogueNode, Direction.Input, Port.Capacity.Multi);
        inputPort.portName = "Input";
        dialogueNode.inputContainer.Add(inputPort);

        switch (type) {
            case 0:
                dialogueNode.title = "End";
                var toggle = new Toggle() {text = "succes"};
                toggle.SetValueWithoutNotify(dialogueNode.succes);
                toggle.RegisterValueChangedCallback(evt => {
                    dialogueNode.succes = evt.newValue;
                });
                dialogueNode.outputContainer.Add(toggle);
                break;
            case 1:
                dialogueNode.styleSheets.Add(Resources.Load<StyleSheet>("NPCNode"));
                var textField = new TextField(string.Empty);
                textField.RegisterValueChangedCallback(evt => {
                    dialogueNode.dialogueText = evt.newValue;
                    dialogueNode.title = evt.newValue;
                });
                textField.SetValueWithoutNotify(dialogueNode.title);
                dialogueNode.mainContainer.Add(textField);
                var outPutPort = GeneratePort(dialogueNode, Direction.Output, Port.Capacity.Single);
                outPutPort.portName = "Output";
                dialogueNode.outputContainer.Add(outPutPort);
                break;
            case 2:
                dialogueNode.styleSheets.Add(Resources.Load<StyleSheet>("Node"));
                dialogueNode.title = "Player Dialogue";
                var button = new Button(clickEvent: () => { AddChoicePort(dialogueNode); });
                button.text = "new Choice";
                dialogueNode.titleContainer.Add(button);
                break;
            case 3:
                dialogueNode.title = "Switch";
                dialogueNode.tooltip = "Starts at 0 and itterates 1 each time the node is accessed. Returns from the highest number back to 0 and resets when the game restarts.";
                var port1 = GeneratePort(dialogueNode, Direction.Output, Port.Capacity.Single);
                port1.portName = "0";
                dialogueNode.outputContainer.Add(port1);
                var port2 = GeneratePort(dialogueNode, Direction.Output, Port.Capacity.Single);
                port2.portName = "1";
                dialogueNode.outputContainer.Add(port2);
                var button1 = new Button(clickEvent: () => { AddIntPort(dialogueNode, dialogueNode.outputContainer.Query(name: "connector").ToList().Count); });
                button1.text = "new Choice";
                dialogueNode.titleContainer.Add(button1);
                break;
            case 4:
                dialogueNode.title = "Splitter";
                var textField1 = new TextField(string.Empty);
                textField1.RegisterValueChangedCallback(evt => {
                    dialogueNode.dialogueText = evt.newValue;
                });
                textField1.SetValueWithoutNotify(dialogueNode.dialogueText == "NPC Dialogue" ? "*value*": dialogueNode.dialogueText);
                dialogueNode.inputContainer.Add(textField1);
                dialogueNode.tooltip = "Takes route based on the value of a variable with the same name.";
                var port3 = GeneratePort(dialogueNode, Direction.Output, Port.Capacity.Single);
                port3.portName = "0";
                dialogueNode.outputContainer.Add(port3);
                var port4 = GeneratePort(dialogueNode, Direction.Output, Port.Capacity.Single);
                port4.portName = "1";
                dialogueNode.outputContainer.Add(port4);
                var button2 = new Button(clickEvent: () => { AddIntPort(dialogueNode, dialogueNode.outputContainer.Query(name: "connector").ToList().Count); });
                button2.text = "new Choice";
                dialogueNode.titleContainer.Add(button2);
                break;
        }

        dialogueNode.RefreshExpandedState();
        dialogueNode.RefreshPorts();
        dialogueNode.SetPosition(new Rect(position: position, defeaultNodeSize));

        return dialogueNode;
    }
    public Group CreateCommentBlock(Rect rect, CommentBlockData commentBlockData = null) {
        if (commentBlockData == null)
            commentBlockData = new CommentBlockData();
        var group = new Group {
            autoUpdateGeometry = true,
            title = commentBlockData.Title
        };
        AddElement(group);
        group.SetPosition(rect);
        return group;
    }

    public void AddIntPort(DialogueNode dialogueNode, int portValue, string overriddenPortName = "") {
        var port1 = GeneratePort(dialogueNode, Direction.Output, Port.Capacity.Single);
        port1.portName = portValue.ToString();
        dialogueNode.outputContainer.Add(port1);
    }

    public void AddChoicePort(DialogueNode dialogueNode, string overriddenPortName = "") {
        var generatedPort = GeneratePort(dialogueNode, Direction.Output);

        var oldLabel = generatedPort.contentContainer.Q<Label>("type");
        generatedPort.contentContainer.Remove(oldLabel);

        var outputPortCount = dialogueNode.outputContainer.Query(name: "connector").ToList().Count;

        var choicePortName = string.IsNullOrEmpty(overriddenPortName) ? $"Choice {outputPortCount + 1}" : overriddenPortName;

        generatedPort.portName = choicePortName;

        var textField = new TextField {
            name = string.Empty,
            value = choicePortName
        };
        textField.RegisterValueChangedCallback(evt => generatedPort.portName = evt.newValue);
        generatedPort.contentContainer.Add(new Label("  "));
        generatedPort.contentContainer.Add(textField);

        var deleteButton = new Button(clickEvent: () => RemovePort(dialogueNode, generatedPort)) { text = "X"};
        generatedPort.contentContainer.Add(deleteButton);

        generatedPort.portName = choicePortName;
        dialogueNode.outputContainer.Add(generatedPort);
        dialogueNode.RefreshPorts();
        dialogueNode.RefreshExpandedState();
    }

    private void RemovePort(DialogueNode dialogueNode, Port generatedPort) {
        var targetEdge = edges.ToList().Where(x => 
        x.output.portName == generatedPort.portName && x.output.node == generatedPort.node);

        if (targetEdge.Any()) {
            var edge = targetEdge.First();
            edge.input.Disconnect(edge);
            RemoveElement(targetEdge.First());
        }

        dialogueNode.outputContainer.Remove(generatedPort);
        dialogueNode.RefreshPorts();
        dialogueNode.RefreshExpandedState();
    }

    public void ClearBlackboardAndExposedProperties() {
        ExposedProperties.Clear();
        blackboard.Clear();
    }

    internal void AddPoropertyToBlackBoard(ExposedProperty exposedProperty) {
        var localPropertyName = exposedProperty.PropertyName;
        var localPropertyValue = exposedProperty.PropertyValue;
        while (ExposedProperties.Any(x => x.PropertyName == localPropertyName))
            localPropertyName = $"{localPropertyName}(1)";// NAME + 1;

        var property = new ExposedProperty();
        property.PropertyName = localPropertyName;
        property.PropertyValue = localPropertyValue;
        ExposedProperties.Add(property);

        var container = new VisualElement();
        var blackboardField = new BlackboardField { text = property.PropertyName, typeText = "string.property" };
        container.Add(blackboardField);

        var propertyValueTextField = new TextField("Value:") {
            value = localPropertyValue
        };
        propertyValueTextField.RegisterValueChangedCallback(evt => {
            var changingPropertyIndex = ExposedProperties.FindIndex(x => x.PropertyName == property.PropertyName);
            ExposedProperties[changingPropertyIndex].PropertyValue = evt.newValue;
        });

        var blackBoardValueRow = new BlackboardRow(blackboardField, propertyValueTextField);
        container.Add(blackBoardValueRow);

        blackboard.Add(container);
    }
}
