using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface that allows an element to provide its probability.
/// Used by <see cref="RandomWeightedElement"/>.
/// </summary>

public interface ProbableElement
{
    /// <summary>
    /// The probability this element is randomly chosen with from a collection.
    /// </summary>
    /// <remarks>
    /// The probability is relative to the probabilities of the other
    /// elements in the collection, it's value is therefore arbitrary.
    /// An item with probability 0 is never chosen, the behavior with
    /// negative probabilities is undefined.
    /// </remarks>

    [SerializeField] public float Probability { get; set; }

    /// <summary>
    /// Choose a random element in a collection, respecting the elements' probability.
    /// </summary>
    /// <returns>The chosen element or `default(T)` if not element could be chosen
    /// (collection was empty or all probabilities 0).</returns>
    public static T RandomWeightedElement<T>(IEnumerable<T> collection) where T : ProbableElement
    {
        // Calculate sum probabilities of all elements
        var total = 0f;
        foreach (var el in collection)
        {
            total += el.Probability;
        }

        // Choose a random value inside the total probability
        var random = UnityEngine.Random.value * total;

        // Go through the elements again, until the chosen value is in the element's probability range
        var current = 0f;
        foreach (var el in collection)
        {
            if (current <= random && random < current + el.Probability)
            {
                return el;
            }
            current += el.Probability;
        }

        return default(T);
    }
}
