using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Drawing.Imaging;

namespace SpriteEditor
{
    public enum DrawingMode { Pencil = 0, Eraser, Picker };

    public partial class SpriteDesigner : Form
    {
        #region Properties

        /// <summary>
        /// Drawing mode of the designer, true = pencil, false = eraser
        /// </summary>
        protected DrawingMode drawingMode { get; set; } = DrawingMode.Pencil;

        /// <summary>
        /// Resolution of the sprite in pixels (Width, Height)
        /// </summary>
        public Tuple<int, int> canvasResolution { get; set; } = new Tuple<int, int>(32, 32);

        /// <summary>
        /// The canvas bitmap of the sprite being created
        /// </summary>
        public Bitmap canvasImage { get; protected set; }

        /// <summary>
        /// Image used for the preview
        /// </summary>
        protected Bitmap previewImage { get; set; }

        /// <summary>
        /// Image used for the color picker control
        /// </summary>
        protected Bitmap colorPickerImage { get; set; }

        /// <summary>
        /// The current layer being worked on by the user
        /// </summary>
        public int currentLayer { get; protected set; } = 0;

        /// <summary>
        /// Selected color being used right now for drawing
        /// </summary>
        protected Color selectedColor { get; set; } = Color.Black;

        /// <summary>
        /// Color of the grid lines
        /// </summary>
        protected Color gridColor { get; set; } = Color.Black;

        /// <summary>
        /// Boolean flag determining whether to show the grid lines or not
        /// </summary>
        public bool showGrid { get; set; } = true;

        /// <summary>
        /// List of images used in the layers
        /// </summary>
        protected List<Bitmap> LayerList { get; set; }

        /// <summary>
        /// List of opacities for the images used in the layers
        /// </summary>
        protected List<double> LayerOpacities { get; set; }

        /// <summary>
        /// Flags when the user is trying to draw while dragging the mouse
        /// </summary>
        protected bool isMousePressed { get; set; } = false;

        /// <summary>
        /// The percent that the sprite is blown up by in the viewer 1 = 1x, 2 = 2x, etc...
        /// </summary>
        protected double zoomPercentage { get; set; } = 1;

        #endregion

        public SpriteDesigner()
        {
            InitializeComponent();

            drawingMode = DrawingMode.Pencil;
            toolStripButtonPencil.Checked = true;
            toolStripButtonEraser.Checked = false;
            toolStripButtonPicker.Checked = false;
            toolStripButtonGrid.Checked = true;

            setupControlSizes();

            canvasImage = new Bitmap(canvasResolution.Item1, canvasResolution.Item2, PixelFormat.Format32bppArgb);  // Creates the new sprite bitmap
            Graphics g = Graphics.FromImage(canvasImage);
            g.Clear(Color.FromArgb(0, 255, 255, 255));                                                              // Fills the bitmap with a clear background
            g.Dispose();

            LayerList = new List<Bitmap>(1);
            LayerOpacities = new List<double>(1);
            LayerList.Add((Bitmap)canvasImage.Clone());                                                             // Copies the new image into the first layer
            LayerOpacities.Add(100);

                                                                                                                    // Updates the icon of the color button
            Bitmap temp = new Bitmap(10, 10);
            g = Graphics.FromImage(temp);
            lock (g)
            {
                g.Clear(selectedColor);
            }
            g.Dispose();
            toolStripButtonColor.Image = temp;

            colorControl();                                                                                         // Paints the picture
            updateLayerVisuals();
        }

        #region Methods

        /// <summary>
        /// Resizes the controls to fit the screen perfectly
        /// </summary>
        protected void setupControlSizes()
        {
            // Updates the canvas
            pictureBoxCanvas.Height = Height - pictureBoxCanvas.Location.Y - statusStrip.Height - canvasResolution.Item2;
            pictureBoxCanvas.Width = Width - pictureBoxCanvas.Location.X - canvasResolution.Item1;
            pictureBoxCanvas.Height -= pictureBoxCanvas.Height % canvasResolution.Item2;
            pictureBoxCanvas.Width -= pictureBoxCanvas.Width % canvasResolution.Item1;
            pictureBoxCanvas.Height = (pictureBoxCanvas.Height < pictureBoxCanvas.Width) ? pictureBoxCanvas.Height : pictureBoxCanvas.Width;
            pictureBoxCanvas.Width = (pictureBoxCanvas.Width < pictureBoxCanvas.Height) ? pictureBoxCanvas.Width : pictureBoxCanvas.Height;
        }

        #endregion

        #region Visual Functions

        /// <summary>
        /// Finds the array index of the pixel clicked on by the mouse on the canvas, since the canvas resolution is smaller than the control's resolution
        /// </summary>
        /// <param name="e">The mouse event arguments from the click event</param>
        /// <returns>Returns the index tuple of (x, y)</returns>
        protected Tuple<int, int> GetSelectedIndex(MouseEventArgs e)
        {
            int localX = (int)((double)e.X / (double)pictureBoxCanvas.Width * (double)canvasResolution.Item1);
            int localY = (int)((double)e.Y / (double)pictureBoxCanvas.Height * (double)canvasResolution.Item2);

            return new Tuple<int, int>(localX, localY);
        }

        /// <summary>
        /// Colors in the pixel on the sprite
        /// </summary>
        /// <param name="X">X coordinate of the pixel</param>
        /// <param name="Y">Y coordinate of the pixel</param>
        protected void colorCanvas(int X, int Y)
        {
            if (Y > 0 && X > 0 && Y < canvasResolution.Item2 && X < canvasResolution.Item1)
            {
                if (drawingMode != DrawingMode.Picker)
                    LayerList[currentLayer].SetPixel(X, Y, (drawingMode == DrawingMode.Pencil) ? selectedColor : Color.FromArgb(0, 0, 0, 0));
                else
                    selectedColor = LayerList[currentLayer].GetPixel(X, Y);
            }
        }

        /// <summary>
        /// Colors the Canvas Control
        /// </summary>
        protected void colorControl()
        {
            //TODO

            // Updates the scrollbars
            if (zoomPercentage <= 1)
            {
                canvasHScrollBar.Visible = false;
                canvasVScrollBar.Visible = false;
            }
            else
            {
                canvasHScrollBar.Visible = true;
                canvasHScrollBar.Minimum = 0;
                canvasHScrollBar.Maximum = (int)(pictureBoxCanvas.Width - (pictureBoxCanvas.Width / zoomPercentage));
                canvasVScrollBar.Visible = true;
                canvasVScrollBar.Minimum = 0;
                canvasVScrollBar.Maximum = (int)(pictureBoxCanvas.Height - (pictureBoxCanvas.Height / zoomPercentage));
            }

            resetImages();

            Graphics g = Graphics.FromImage(canvasImage);
            lock (g)
            {
                // Form the picture that is contained inside of the layer list and draw grid if necessary
                bool firstPassVer = true;
                Pen pen = new Pen(new SolidBrush(gridColor));

                for (int i = 0; i < canvasResolution.Item1; i++)
                {
                    bool firstPassHor = true;

                    for (int j = 0; j < canvasResolution.Item2; j++)
                    {
                        double dX = pictureBoxCanvas.Width / canvasResolution.Item1;
                        double dY = pictureBoxCanvas.Height / canvasResolution.Item2;

                        // Draws the grid as part of the pixel system to reduce iteration time
                        if(showGrid && firstPassVer)
                            g.DrawLine(pen, 0, (int)(j * dX), pictureBoxCanvas.Width, (int)(j * dX));

                        if(showGrid && firstPassHor)
                        {
                            firstPassHor = false;
                            g.DrawLine(pen, (int)(i * dY), 0, (int)(i * dY), pictureBoxCanvas.Height);
                        }

                        // Generates the color
                        Color newColor = Color.FromArgb(0, 255, 255, 255);
                        for(int k = 0; k < LayerList.Count; k++)
                        {
                            // Skip this layer if it isn't visible
                            if (LayerOpacities[k] == 0)
                                continue;

                            // Gets the color of the pixel
                            Color c = LayerList[k].GetPixel(i, j);

                            // Only occurs if the layer, and c is actually visible
                            if (c.A > 0)
                            {
                                // If the new layer's opacity is 100% then everything below it will be covered up, there's no sense in doing the algorithm
                                if (c.A < 255 || LayerOpacities[k] < 100)
                                {
                                    // Transforms the alpha transparencies into 0-1 doubles
                                    double alphaNP = newColor.A / 255, alphaCP = c.A / 255 * (LayerOpacities[k] / 100);

                                    // Calculates the new values of c over newColor using alpha compositing
                                    // https://en.wikipedia.org/wiki/Alpha_compositing
                                    int red = (int)((c.R * alphaCP + newColor.R * alphaNP * (1 - alphaCP)) / (alphaCP + alphaNP * (1 - alphaCP)));
                                    int green = (int)((c.G * alphaCP + newColor.G * alphaNP * (1 - alphaCP)) / (alphaCP + alphaNP * (1 - alphaCP)));
                                    int blue = (int)((c.B * alphaCP + newColor.B * alphaNP * (1 - alphaCP)) / (alphaCP + alphaNP * (1 - alphaCP)));
                                    int alpha = (int)(255 * (alphaCP + alphaNP * (1 - alphaCP)));

                                    newColor = Color.FromArgb(alpha, red, green, blue);
                                }
                                else
                                    newColor = c;
                            }
                        }

                        g.FillRegion(new SolidBrush(newColor),
                            new Region(new Rectangle((int)(i * dX),(int)(j * dY), (int)dX, (int)dY)));
                    }
                    if (showGrid && firstPassVer)
                    {
                        firstPassVer = false;
                        g.DrawLine(pen, 0, pictureBoxCanvas.Height - 1, pictureBoxCanvas.Width, pictureBoxCanvas.Height - 1);
                    }
                }

                if(showGrid)
                    g.DrawLine(pen, pictureBoxCanvas.Width - 1, 0, pictureBoxCanvas.Width - 1, pictureBoxCanvas.Height);

                pen.Dispose();

                // Shows the image on the preview section
                if (previewImage != null)
                    previewImage.Dispose();
                previewImage = new Bitmap(canvasImage, pictureBoxPreview.Width, pictureBoxPreview.Height);
                pictureBoxPreview.Image = previewImage;
               
            }
            g.Dispose();

            // Shows the image on the canvas
            pictureBoxCanvas.Image = canvasImage;

            pictureBoxCanvas.Invalidate();
            pictureBoxPreview.Invalidate();
        }

        /// <summary>
        /// Updates the layer list control
        /// </summary>
        protected void updateLayerVisuals()
        {
            // Clears the list
            listViewLayers.Items.Clear();

            // Adds an equivalent number of items as there are in the LayerList
            for (int i = 0; i < LayerList.Count; i++)
                listViewLayers.Items.Add(new ListViewItem(new string[] { "Frame " + i + " Op: " + LayerOpacities[i] }, i, (i == currentLayer) ? Color.CadetBlue : Color.White, Color.BlueViolet, DefaultFont));

            // Causes the bitmaps to be drawn on the layer list
            listViewLayers.RedrawItems(0, listViewLayers.Items.Count - 1, true);

            trackBarLayerOpacity.Value = (int)(LayerOpacities[currentLayer]);

        }

        /// <summary>
        /// Resets the image objects without consuming more RAM
        /// </summary>
        protected void resetImages()
        {
            if (canvasImage != null)
                canvasImage.Dispose();
            canvasImage = new Bitmap(pictureBoxCanvas.Width, pictureBoxCanvas.Height);

            if (previewImage != null)
                previewImage.Dispose();
            previewImage = new Bitmap(LayerList[currentLayer], pictureBoxPreview.Width, pictureBoxPreview.Height);
        }

        #endregion

        #region ToolStrip Buttons

        private void toolStripButtonPencil_Click(object sender, EventArgs e)
        {
            drawingMode = DrawingMode.Pencil;
            toolStripButtonPencil.Checked = true;
            toolStripButtonEraser.Checked = false;
            toolStripButtonPicker.Checked = false;
        }

        private void toolStripButtonEraser_Click(object sender, EventArgs e)
        {
            drawingMode = DrawingMode.Eraser;
            toolStripButtonPencil.Checked = false;
            toolStripButtonEraser.Checked = true;
            toolStripButtonPicker.Checked = false;
        }

        private void toolStripButtonPicker_Click(object sender, EventArgs e)
        {
            drawingMode = DrawingMode.Picker;
            toolStripButtonPencil.Checked = false;
            toolStripButtonEraser.Checked = false;
            toolStripButtonPicker.Checked = true;
        }

        private void toolStripButtonGrid_Click(object sender, EventArgs e)
        {
            showGrid = !showGrid;
            toolStripButtonGrid.Checked = showGrid;
            colorControl();
        }

        private void toolStripButtonColor_Click(object sender, EventArgs e)
        {
            colorDialogPicker.ShowDialog();
            selectedColor = colorDialogPicker.Color;

            // Updates the icon of the color button
            Bitmap temp = new Bitmap(10, 10);
            Graphics g = Graphics.FromImage(temp);
            lock(g)
            {
                g.Clear(selectedColor);
            }
            g.Dispose();
            toolStripButtonColor.Image = temp;
        }

        #endregion

        #region MenuStrip Buttons

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        /// <summary>
        /// Creates a new layer in the layer list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void layerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap temp = new Bitmap(canvasResolution.Item1, canvasResolution.Item2);

            Graphics g = Graphics.FromImage(temp);
            lock(g)
            {
                g.Clear(Color.FromArgb(0, 0, 0, 0));
            }
            g.Dispose();

            LayerList.Add(temp);
            LayerOpacities.Add(100);

            currentLayer = LayerList.Count - 1;

            updateLayerVisuals();
            colorControl();
        }

        /// <summary>
        /// Clears the current layer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap temp = new Bitmap(canvasResolution.Item1, canvasResolution.Item2);

            Graphics g = Graphics.FromImage(temp);
            lock (g)
            {
                g.Clear(Color.FromArgb(0, 0, 0, 0));
            }
            g.Dispose();

            LayerList[currentLayer] = temp;

            colorControl();
        }

        #endregion

        private void pictureBoxCanvas_MouseClick(object sender, MouseEventArgs e)
        {
            Tuple<int, int> coord = GetSelectedIndex(e);

            colorCanvas(coord.Item1, coord.Item2);
            colorControl();
        }

        private void listViewLayers_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentLayer = listViewLayers.SelectedIndices[0];
            updateLayerVisuals();
            colorControl();
        }

        private void SpriteDesigner_MouseMove(object sender, MouseEventArgs e)
        {
            Tuple<int, int> coord = GetSelectedIndex(e);
            toolStripStatusLabelCursorPosition.Text = "X: " + e.X + 
                " Y: " + e.Y + " Pixel Location: {" + coord.Item1 + ", " + coord.Item2 + "} Opacity: " + LayerOpacities[currentLayer] +
                " Zoom: " + zoomPercentage + "x";

            // Colors the pixels if the mouse is pressed
            if(isMousePressed)
            {
                colorCanvas(coord.Item1, coord.Item2);
                colorControl();
            }
        }

        private void SpriteDesigner_ResizeEnd(object sender, EventArgs e)
        {
            setupControlSizes();
            updateLayerVisuals();
            colorControl();
        }

        private void pictureBoxCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            isMousePressed = true;
        }

        private void pictureBoxCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            isMousePressed = false;
        }

        private void trackBarLayerOpacity_ValueChanged(object sender, EventArgs e)
        {
            LayerOpacities[currentLayer] = trackBarLayerOpacity.Value;
            updateLayerVisuals();
            colorControl();
        }
    }
}
