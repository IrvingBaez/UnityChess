using System;
using System.Collections.Generic;
using UnityEngine;

public class Node<I>
{
    private readonly I identifier;
    private float value;

    private Node<I> parent;
    private Node<I> bestChild;
    private List<Node<I>> children;

    public Node()
    {
        identifier = default(I);
        value = float.NaN;
        parent = null;
        bestChild = null;
        children = new List<Node<I>>();
    }

    public Node(I identifier)
    {
        this.identifier = identifier;
        value = float.NaN;
        parent = null;
        bestChild = null;
        children = new List<Node<I>>();
    }

    public int GetSize()
    {
        int size = 1;

        foreach (Node<I> child in children)
        {
            size += child.GetSize();
        }

        return size;
    }

    public int MaxDepth()
    {
        int max = 0;
        foreach (Node<I> child in children)
        {
            int childDepth = child.MaxDepth();
            if (childDepth > max)
            {
                max = childDepth;
            }
        }

        return max + 1;
    }

    public Node(I identifier, float value)
    {
        this.identifier = identifier;
        this.value = value;
        parent = null;
        bestChild = null;
        children = new List<Node<I>>();
    }

    public void AddChild(Node<I> node)
    {
        node.parent = this;
        this.children.Add(node);
    }

    public void AddChild(I identifier){
        AddChild(new Node<I>(identifier));
    }

    public void AddChild(I identifier, float value)
    {
        AddChild(new Node<I>(identifier, value));
    }

    public int getDepth()
    {
        int depth = 0;

        Node<I> node = this;
        while (node.GetParent() != null)
        {
            depth++;
            node = node.GetParent();
        }
        return depth;
    }

    public Node<I> GetParent()
    {
        return this.parent;
    }

    public void SetBestChild(Node<I> bestChild)
    {
        this.bestChild = bestChild;
    }

    public Node<I> GetBestChild()
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

    public I GetIdentifier()
    {
        return identifier;
    }

    public List<Node<I>> GetChildren()
    {
        return children;
    }

    public Node<I> getChild(I identifier)
    {
        foreach(Node<I> child in children)
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
        return $"{getDepth()}, {identifier}: {value}";
    }
}
