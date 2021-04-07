using UnityEngine;
using System.Collections.Generic;

public class MinimaxProcessor<I>
{
    private readonly Mode phase;
    private readonly Tree<I> tree;

    public enum Mode { MIN, MAX }

    public MinimaxProcessor(Mode phase, Tree<I> tree)
    {
        this.phase = phase;
        this.tree = tree;
    }

    public void Process()
    {
        Process(tree.GetRoot());
    }

    private float Process(Node<I> currentNode)
    {
        Mode mode = SetMode(currentNode);
        if(float.IsNaN(currentNode.GetValue()))
        {
            if(currentNode.GetChildren().Count == 0)
                return float.NaN;

            Node<I> bestChild = currentNode.GetChildren()[0];
            bestChild.SetValue(Process(bestChild));

            foreach(Node<I> child in currentNode.GetChildren())
            {
                if (child == bestChild)
                    continue;

                child.SetValue(Process(child));

                if(child.GetValue().CompareTo(bestChild.GetValue()) == (int)mode * 2 - 1)
                {
                    bestChild = child;
                }
                else if (child.GetValue() == bestChild.GetValue() && Random.value < 0.1)
                {
                    bestChild = child;
                }
            }
            currentNode.SetValue(bestChild.GetValue());
            currentNode.SetBestChild(bestChild);
        }
        return currentNode.GetValue();
    }

    private Mode SetMode(Node<I> node)
    {
        if(node.getDepth() % 2 == 0)
        {
            return phase;
        }
        return SwitchMode(phase);
    }

    private Mode SwitchMode(Mode mode)
    {
        return (Mode) (((int)mode + 1) % 2);
    }

    public List<I> GetSequence()
    {
        List<I> sequence = new List<I>();
        Node<I> node = tree.GetRoot();

        while(node.GetBestChild() != null)
        {
            node = node.GetBestChild();
            sequence.Add(node.GetIdentifier());
        }

        return sequence;
    }
}
