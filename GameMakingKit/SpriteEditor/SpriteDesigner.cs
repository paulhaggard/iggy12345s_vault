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
        /// The physical bitmap of the sprite being created
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

        #endregion

        public SpriteDesigner()
        {
            InitializeComponent();

            drawingMode = DrawingMode.Pencil;
            toolStripButtonPencil.Checked = true;
            toolStripButtonEraser.Checked = false;
            toolStripButtonPicker.Checked = false;
            toolStripButtonGrid.Checked = true;

            canvasImage = new Bitmap(canvasResolution.Item1, canvasResolution.Item2, PixelFormat.Format32bppArgb);  // Creates the new sprite bitmap
            Graphics g = Graphics.FromImage(canvasImage);
            g.Clear(Color.FromArgb(0, 0, 0, 0));                                                                    // Fills the bitmap with a clear background
            g.Dispose();

            LayerList = new List<Bitmap>(1);
            LayerList.Add((Bitmap)canvasImage.Clone());                                                             // Copies the new image into the first layer

                                                                                                                    // Updates the icon of the color button
            Bitmap temp = new Bitmap(2, 2);
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
            Graphics g = Graphics.FromImage(LayerList[currentLayer]);

            lock(g)
            {
                Pen p = new Pen(new SolidBrush(selectedColor));
                g.FillRegion(new SolidBrush(selectedColor), new Region(new Rectangle(X, Y, 1, 1)));
                p.Dispose();
            }

            g.Dispose();


            // Shows the image on the preview section
            if (previewImage != null)
                previewImage.Dispose();
            previewImage = new Bitmap(canvasImage, pictureBoxPreview.Width, pictureBoxPreview.Height);
            pictureBoxPreview.Image = previewImage;

        }

        /// <summary>
        /// Colors the Canvas Control
        /// </summary>
        protected void colorControl()
        {
            // Updates the scrollbars
            canvasHScrollBar.Visible = false;
            canvasVScrollBar.Visible = false;

            resetImages();

            Graphics g = Graphics.FromImage(canvasImage);
            lock (g)
            {
                // Form the picture that is contained inside of the layer list
                g.DrawImage(LayerList[currentLayer], 0, 0, pictureBoxCanvas.Width, pictureBoxCanvas.Height);


                // Shows the image on the preview section
                if (previewImage != null)
                    previewImage.Dispose();
                previewImage = new Bitmap(canvasImage, pictureBoxPreview.Width, pictureBoxPreview.Height);
                pictureBoxPreview.Image = previewImage;
                

                // Draws the grid lines if they're shown
                if (showGrid)
                {
                    Pen pen = new Pen(new SolidBrush(gridColor));

                    // Vertical lines
                    for (int i = 0; i < pictureBoxCanvas.Width; i += pictureBoxCanvas.Width / canvasResolution.Item1)
                        g.DrawLine(pen, i, 0, i, pictureBoxCanvas.Height);
                    g.DrawLine(pen, pictureBoxCanvas.Width - 1, 0, pictureBoxCanvas.Width, pictureBoxCanvas.Height);

                    // Horizontal lines
                    for (int i = 0; i < pictureBoxCanvas.Height; i += pictureBoxCanvas.Height / canvasResolution.Item2)
                        g.DrawLine(pen, 0, i, pictureBoxCanvas.Width, i);
                    g.DrawLine(pen, 0, pictureBoxCanvas.Height, pictureBoxCanvas.Width, pictureBoxCanvas.Height);

                    pen.Dispose();
                }
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
                listViewLayers.Items.Add(new ListViewItem("Frame " + i, i));

            // Causes the bitmaps to be drawn on the layer list
            listViewLayers.RedrawItems(0, listViewLayers.Items.Count - 1, true);
        }

        /// <summary>
        /// Resets the image objects without consuming more RAM
        /// </summary>
        protected void resetImages()
        {
            if (canvasImage != null)
                canvasImage.Dispose();
            canvasImage = new Bitmap(LayerList[currentLayer], pictureBoxCanvas.Width, pictureBoxCanvas.Height);

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
            Bitmap temp = new Bitmap(2, 2);
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

        private void layerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap temp = new Bitmap(canvasResolution.Item1, canvasResolution.Item2, PixelFormat.Format32bppArgb);

            Graphics g = Graphics.FromImage(temp);
            lock(g)
            {
                g.Clear(Color.FromArgb(0, 0, 0, 0));
            }
            g.Dispose();

            LayerList.Add(new Bitmap(canvasResolution.Item1, canvasResolution.Item2, PixelFormat.Format32bppArgb));

            currentLayer = LayerList.Count - 1;
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
            toolStripStatusLabelCursorPosition.Text = "X: " + e.X + " Y: " + e.Y + " Pixel Location: {" + coord.Item1 + ", " + coord.Item2 + "}";
        }
    }
}
