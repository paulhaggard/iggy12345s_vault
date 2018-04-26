using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkFundamentals
{
    /* Linking can take on the form of 2 types
     * Inline:
     *  The layer gets squeezed between the output of this layer, and one of the next layers
     * 
     *    L
     *    v    >>> L=L=L
     * L === L
     * 
     * Branching:
     *  The layer gets it's own output from the layer you're connecting it to.
     *       L               L
     *       v        >>>    |
     *    == L == L       == L === L
     */
    public enum LinkingType { Inline=0, Branching}

    // The class for the linking event arguments
    public class LinkingEventArgs : EventArgs
    {
        // The event arguments for the linking events.

        private LinkingType type;
        private int targetLayer;        // Target layer to link to for input.
        private int targetOutputLayer;  // Target layer to link to, if doing an inline link

        public LinkingType Type { get => type; set => type = value; }
        public int TargetLayer { get => targetLayer; set => targetLayer = value; }
        public int TargetOutputLayer { get => targetOutputLayer; set => targetOutputLayer = value; }

        public LinkingEventArgs(LinkingType type, int targetLayer, int targetOutputLayer = -1) : base()
        {
            this.type = type;
            this.targetLayer = targetLayer;
            this.targetOutputLayer = targetOutputLayer;
        }
    }

    public class Layer
    {
        /* This class will help with grouping neurons together
         * It will be passable to the neural network's constructor and accessor methods
         * The previous accessor methods will still be there, though.
         * 
         * It will have events for linking layers together.
         * The type of linking event will determine how weight matrices are formed.
         */

        private List<Neuron> neurons;   // The list of neurons in this layer
        private int id;                 // A unique identifier for this layer
        private bool input;             // Flag determining if this layer is an input
        private bool output;            // Flag determining if this layer is an output

        private static int layerCount;

        public delegate void LinkingEventHandler(object sender, LinkingEventArgs e);

        public event LinkingEventHandler LinkingEvent;

        public void OnLinkingEvent(object sender, LinkingEventArgs e)
        {
            LinkingEvent?.Invoke(this, e);
        }

        public Layer(List<Neuron> neurons, bool input = false, bool output = false)
        {
            this.neurons = neurons;
            this.input = input;
            this.output = output;
            id = layerCount++;      // Assigns the unique layer id;
        }

        public void RequestLinkIn(object sender, LinkingType type)
        {
            // Handles links to the input of the layer
        }

        public void RequestLinkOut(object sender, LinkingType type)
        {
            // Handles links to the output of the layer
        }

        public void RequestDeLink(object sender)
        {
            // Handles de-linking two things.
        }
    }
}
