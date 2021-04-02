using System;
using System.Collections.Generic;
using UnityEngine;

public class Node<I, C>
{
    private readonly I identifier;
    private readonly C content;
    private float value;

    private Node<I, C> parent;
    private Node<I, C> bestChild;
    private List<Node<I, C>> children;

    public Node()
    {
        identifier = default(I);
        content = default(C);
        value = float.NaN;
        parent = null;
        bestChild = null;
        children = new List<Node<I, C>>();
    }

    public Node(I identifier, C content)
    {
        this.identifier = identifier;
        this.content = content;
        value = float.NaN;
        parent = null;
        bestChild = null;
        children = new List<Node<I, C>>();
    }

    public int GetSize()
    {
        int size = 1;

        foreach (Node<I, C> child in children)
        {
            size += child.GetSize();
        }

        return size;
    }

    public int MaxDepth()
    {
        int max = 0;
        foreach (Node<I, C> child in children)
        {
            int childDepth = child.MaxDepth();
            if (childDepth > max)
            {
                max = childDepth;
            }
        }

        return max + 1;
    }

    public Node(I identifier, C content, float value)
    {
        this.identifier = identifier;
        this.content = content;
        this.value = value;
        parent = null;
        bestChild = null;
        children = new List<Node<I, C>>();
    }

    public void AddChild(Node<I, C> node)
    {
        node.parent = this;
        this.children.Add(node);
    }

    public void AddChild(I identifier, C content)
    {
        AddChild(new Node<I, C>(identifier, content));
    }

    public void AddChild(I identifier, C content, float value)
    {
        AddChild(new Node<I, C>(identifier, content, value));
    }

    public int getDepth()
    {
        int depth = 0;

        Node<I, C> node = this;
        while (node.GetParent() != null)
        {
            depth++;
            node = node.GetParent();
        }
        return depth;
    }

    public Node<I, C> GetParent()
    {
        return this.parent;
    }

    public void SetBestChild(Node<I, C> bestChild)
    {
        this.bestChild = bestChild;
    }

    public Node<I, C> GetBestChild()
    {
        return this.bestChild;
    }

    public void SetValue(float value)
    {
        this.value = value;
    }

    public float GetValue()
    {
        return value;
    }

    public C GetContent()
    {
        return content;
    }

    public I GetIdentifier()
    {
        return identifier;
    }

    public List<Node<I, C>> GetChildren()
    {
        return children;
    }

    public Node<I, C> getChild(I identifier)
    {
        foreach(Node<I, C> child in children)
        {
            if (child.identifier.Equals(identifier))
            {
                return child;
            }
        }

        return null;
    }

    public bool IsLeaf()
    {
        return children.Count == 0;
    }

    public override string ToString()
    {
        return $"{getDepth()}, {identifier}: {value}\n{content}";
    }
}
