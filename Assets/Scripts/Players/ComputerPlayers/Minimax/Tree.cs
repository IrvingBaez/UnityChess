using UnityEngine;
using System;

public class Tree<I>
{
    private Node<I> root;

    public Tree()
    {
        this.root = new Node<I>();
    }

    public Tree(Node<I> root)
    {
        this.root = root;
    }

    public Tree(I identifier)
    {
        this.root = new Node<I>(identifier);
    }

    public Tree(I identifier, float value)
    {
        this.root = new Node<I>(identifier, value);
    }

    public Node<I> GetRoot()
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

    private string Print(Node<I> node, string separator)
    {
        string result = $"\n{separator}{node}";

        foreach(Node<I> child in node.GetChildren())
        {
            result += Print(child, separator + "\t");
        }

        return result;
    }
}
