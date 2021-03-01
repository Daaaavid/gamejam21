using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using System;
using UnityEngine.UIElements;

namespace Subtegral.DialogueSystem.DataContainers {
    public class GraphSaveUtility {
        private DialogueGraphView _targetGraphView;
        private DialogueContainer _containerCache;

        private List<Edge> Edges => _targetGraphView.edges.ToList();
        private List<DialogueNode> Nodes => _targetGraphView.nodes.ToList().Cast<DialogueNode>().ToList();

        private List<Group> CommentBlocks =>
            _targetGraphView.graphElements.ToList().Where(x => x is Group).Cast<Group>().ToList();

        public static GraphSaveUtility GetInstance(DialogueGraphView targetGraphView) {
            return new GraphSaveUtility {
                _targetGraphView = targetGraphView
            };
        }

        public void SaveGraph(string fileName) {

            var dialogueContainer = ScriptableObject.CreateInstance<DialogueContainer>();
            if (!SaveNodes(dialogueContainer)) return;
            SaveExposedProperties(dialogueContainer);
            SaveCommentBLocks(dialogueContainer);
            //autocreate resource folder in case of absense
            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                AssetDatabase.CreateFolder("Assets", "Resources");

            AssetDatabase.CreateAsset(dialogueContainer, $"Assets/Resources/{fileName}.asset");
            AssetDatabase.SaveAssets();
        }

        private bool SaveNodes(DialogueContainer dialogueContainer) {
            if (!Edges.Any()) return false; // if there are no edges(no connections) then return.

            var connectedPorts = Edges.Where(x => x.input.node != null).ToArray();
            for (var i = 0; i < connectedPorts.Length; i++) {
                var outputNode = connectedPorts[i].output.node as DialogueNode;
                var inputNode = connectedPorts[i].input.node as DialogueNode;

                dialogueContainer.NodeLinks.Add(new NodeLinkData {
                    BaseNodeGuid = outputNode.GUID,
                    PortName = connectedPorts[i].output.portName,
                    TargetNodeGuid = inputNode.GUID
                });
            }

            foreach (var dialogueNode in Nodes.Where(node => !node.entryPoint)) {
                dialogueContainer.DialogueNodeData.Add(item: new DialogueNodeData {
                    Guid = dialogueNode.GUID,
                    DialogueText = dialogueNode.dialogueText,
                    Position = dialogueNode.GetPosition().position,
                    type = dialogueNode.type,
                    succes = dialogueNode.succes
                });
            }
            return true;
        }

        private void SaveExposedProperties(DialogueContainer dialogueContainer) {
            dialogueContainer.exposedProperties.AddRange(_targetGraphView.ExposedProperties);
        }

        private void SaveCommentBLocks(DialogueContainer dialogueContainer) {
            foreach (var block in CommentBlocks) {
                var nodes = block.containedElements.Where(x => x is DialogueNode).Cast<DialogueNode>().Select(x => x.GUID).ToList();

                dialogueContainer.CommentBlockData.Add(new CommentBlockData {
                    ChildNodes = nodes,
                    Title = block.title,
                    Position = block.GetPosition().position,
                });
            }
        }

        public void LoadGraph(string fileName) {
            _containerCache = Resources.Load<DialogueContainer>(fileName);
            if (_containerCache == null) {
                EditorUtility.DisplayDialog("File not found", "Target dialogue graph file does not exist!", "ok");
                return;
            }
            ClearGraph();
            CreateNodes();
            ConnectNodes();
            CreateExposedProperties();
            GenerateCommentBlocks();
        }

        private void CreateExposedProperties() {
            //Clear existing properties on hot reload
            _targetGraphView.ClearBlackboardAndExposedProperties();
            //Add properties from data.
            foreach (var exposedProperty in _containerCache.exposedProperties) {
                _targetGraphView.AddPoropertyToBlackBoard(exposedProperty);
            }
        }

        private void ClearGraph() {
            //Set entry points guid back from the save. Discard existing guid.
            Nodes.Find(x => x.entryPoint).GUID = _containerCache.NodeLinks[0].BaseNodeGuid;

            foreach (var node in Nodes) {
                if (node.entryPoint) continue;

                // remove edges that connect to this node. 
                Edges.Where(x => x.input.node == node).ToList() // List<edge>
                    .ForEach(edge => _targetGraphView.RemoveElement(edge));

                // remove this node.
                _targetGraphView.RemoveElement(node);
            }
        }

        private void ConnectNodes() {
            for (var i = 0; i < Nodes.Count; i++) {
                var connections = _containerCache.NodeLinks.Where(x => x.BaseNodeGuid == Nodes[i].GUID).ToList();
                for (var j = 0; j < connections.Count; j++) {
                    var targetNodeGuid = connections[j].TargetNodeGuid;
                    var targetNode = Nodes.First(x => x.GUID == targetNodeGuid);
                    LinkNodes(Nodes[i].outputContainer[j].Q<Port>(), (Port)targetNode.inputContainer[0]);

                    targetNode.SetPosition(new Rect(_containerCache.DialogueNodeData.First(x => x.Guid == targetNodeGuid).Position,
                        _targetGraphView.defeaultNodeSize
                    ));
                }
            }
        }

        private void LinkNodes(Port output, Port input) {
            var tempEdge = new Edge {
                output = output,
                input = input
            };
            tempEdge.input.Connect(tempEdge);
            tempEdge.output.Connect(tempEdge);
            _targetGraphView.Add(tempEdge);
        }

        private void CreateNodes() {
            foreach (var nodeData in _containerCache.DialogueNodeData) {
                var tempNode = _targetGraphView.CreateDialogueNode(nodeData.DialogueText, Vector2.zero,nodeData.type, nodeData.succes);
                tempNode.GUID = nodeData.Guid;
                _targetGraphView.AddElement(tempNode);
                if(nodeData.type == 2) {
                    var nodePorts = _containerCache.NodeLinks.Where(x => x.BaseNodeGuid == nodeData.Guid).ToList();
                    nodePorts.ForEach(x => _targetGraphView.AddChoicePort(tempNode, x.PortName));
                }
                if(nodeData.type == 3 || nodeData.type == 4) {
                    var nodePorts = _containerCache.NodeLinks.Where(x => x.BaseNodeGuid == nodeData.Guid).ToList();
                    foreach(NodeLinkData x in nodePorts) {
                        int.TryParse(x.PortName, out int n);
                        if (n > 1)
                            _targetGraphView.AddIntPort(tempNode, int.Parse(x.PortName));
                    }
                }
            }
        }

        private void GenerateCommentBlocks() {
            foreach (var commentBlock in CommentBlocks) {
                _targetGraphView.RemoveElement(commentBlock);
            }

            foreach(var commentBlockData in _containerCache.CommentBlockData) {
                var block = _targetGraphView.CreateCommentBlock(new Rect(commentBlockData.Position, _targetGraphView.defeaultNodeSize),
                    commentBlockData);
                block.AddElements(Nodes.Where(x => commentBlockData.ChildNodes.Contains(x.GUID)));
            }
        }
    }
}
