using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenTLUSB3Basler
{
  public partial class SnapshotViewer : Form
  {
    public SnapshotViewer()
    {
      InitializeComponent();
    }

    public String SnapshotText
    {
      get
      {
        return snapshotContentRichTextBox.Text;
      }
      set
      {
        snapshotContentRichTextBox.Text = value;
      }
    }
  }
}
