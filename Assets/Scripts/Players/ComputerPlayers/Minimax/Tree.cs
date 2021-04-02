using UnityEngine;
using System;

public class Tree<I, C>
{
    private Node<I, C> root;

    public Tree()
    {
        this.root = new Node<I, C>();
    }

    public Tree(Node<I, C> root)
    {
        this.root = root;
    }

    public Tree(I identifier, C content)
    {
        this.root = new Node<I, C>(identifier, content);
    }

    public Tree(I identifier, C content, float value)
    {
        this.root = new Node<I, C>(identifier, content, value);
    }

    public Node<I, C> GetRoot()
    {
        return root;
    }

    public int Size()
    {
        if(root == null)
        {
            return 0;
        }

        return root.GetSize();
    }

    public int MaxDepth()
    {
        return root.MaxDepth() - 1;
    }

    public override string ToString()
    {
        return this.Print(root, "\t");
    }

    private string Print(Node<I, C> node, string separator)
    {
        string result = $"\n{separator}{node}";

        foreach(Node<I, C> child in node.GetChildren())
        {
            result += Print(child, separator + "\t");
        }

        return result;
    }
}
