using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Subtegral.DialogueSystem.DataContainers {
    [Serializable]
    public class DialogueContainer : ScriptableObject {
        public List<NodeLinkData> NodeLinks = new List<NodeLinkData>();
        public List<DialogueNodeData> DialogueNodeData = new List<DialogueNodeData>();
        public List<ExposedProperty> exposedProperties = new List<ExposedProperty>();
        public List<CommentBlockData> CommentBlockData = new List<CommentBlockData>();
    }
}

