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
  public partial class FeatureNodeTreeViewer : Form
  {
    public FeatureNodeTreeViewer()
    {
      InitializeComponent();
    }
    
    public TreeNode Root
    {
      get
      {
        //Fancy bit if syntax that says if there is no Nodes collection to index into, return null instead
        return featureNodeTreeView.Nodes?[0];
      }
      set
      {
        featureNodeTreeView.Nodes.Clear();
        featureNodeTreeView.Nodes.Add(value);
      }
    }
  }
}
