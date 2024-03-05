/*******************************************************************************
Copyright (C) 2008 Cognex Corporation

Subject to Cognex Corporations terms and conditions and license agreement,
you are authorized to use and modify this source code in any way you find
useful, provided the Software and/or the modified Software is used solely in
conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
and agree that Cognex has no warranty, obligations or liability for your use
of the Software.
*******************************************************************************
 This sample program is designed to illustrate certain VisionPro features or 
 techniques in the simplest way possible. It is not intended as the framework 
 for a complete application. In particular, the sample program may not provide
 proper error handling, event handling, cleanup, repeatability, and other 
 mechanisms that a commercial quality application requires.

This sample demonstrates how a user can create their own Vision Tools using 
base classes the Cognex provides.  The tool created in this class converts 3-plane
RGB images to individual R, G, and B planes, or converts a 3-plane RGB image to 
individual H, S, and I planes.

The tool created is called the CogColorConversionTool.  It makes use of an operator, which
does all the real work, called CogColorConversion.

*/

using System;
using Cognex.VisionPro;
using Cognex.VisionPro.Implementation;
using Cognex.VisionPro.ImageProcessing;
using System.ComponentModel; // needed for Editor attribute
using System.Windows.Forms;  // needed for Editor attribute
using System.Runtime.Serialization;
using Cognex.VisionPro.Exceptions;

// In this sample, we are adding this new tool to the Cognex.VisionPro.ImageProcessing namespace
// You can add your tool to any Cognex.VisionPro namespace, or create your own namespace.
namespace CogColorConversionTool
{
    /// <summary>
    /// CogColorConversionResults:
    /// The results provided by the CogColorConversion operator.  The results consist of 
    /// three planes, representing R, G, and B or H, S, and I.  We mark the class with the
    /// .NET Serializable attribute so it can use persistence.  It is derived from ICloneable so
    /// that we can use the Clone() method to make a copy of this object.  Results should also
    /// derive from CogSerializableObject base so they can be saved/restored properly.
    /// </summary>
    [Serializable]
    public class CogColorConversionResults : CogSerializableObjectBase, ICloneable
    {
        #region Private Fields
        CogImage8Grey _plane0;
        CogImage8Grey _plane1;
        CogImage8Grey _plane2;
        #endregion

        #region Constructors
        public CogColorConversionResults()
        {
        }
        public CogColorConversionResults(CogColorConversionResults other)
        {
            _plane0 = other.Plane0;
            _plane1 = other.Plane1;
            _plane2 = other.Plane2;
        }
        public CogColorConversionResults(CogImage8Grey plane0,
            CogImage8Grey plane1,
            CogImage8Grey plane2)
        {
            _plane0 = plane0;
            _plane1 = plane1;
            _plane2 = plane2;
        }

        /// <summary>
        /// Serialization Constructor - necessary for save/restore.
        /// </summary>
        private CogColorConversionResults(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion

        #region Public Properties
        public CogImage8Grey Plane0
        {
            get { return _plane0; }
        }
        public CogImage8Grey Plane1
        {
            get { return _plane1; }
        }
        public CogImage8Grey Plane2
        {
            get { return _plane2; }
        }
        #endregion

        #region ICloneable Implementation
        /// <summary>
        /// Clones the results object
        /// </summary>
        /// <returns>A new copy of the results object</returns>
        public object Clone()
        {
            return new CogColorConversionResults(this);
        }
        #endregion
    }
    /// <summary>
    /// CogColorConversion:
    /// An operator used by the CogColorConversionTool to generate
    /// individual color planes from an RGB Image.  It is derived from CogSerializableChangedEventBase
    /// so that it will generate changed events and so that it can be serialized.  It is derived from ICloneable so
    /// that we can use the Clone() method to make a copy of this object. We mark the class with the
    /// .NET Serializable attribute so it can use persistence.  
    /// 
    /// When an object derives from CogSerializableChangedEventBase, it is able to generate changed events.
    /// When a changed event is generated, it is the responsiblity of the object to indicate
    /// which element of the class has changed.  To do this, StateFlags are used.  StateFlags
    /// are a bitfield where each bit represents a particular element that has changed.
    /// In this object, the only thing that can change is the _convertToHSI private field.
    /// When this element changes, a changed event is issued with SfConvertToHSI state flag
    /// asserted.   
    /// </summary>
    [Serializable]
    public class CogColorConversion : CogSerializableChangedEventBase, ICloneable
    {
        #region Constructors
        public CogColorConversion()
        {
            _convertToHSI = false;
        }
        public CogColorConversion(CogColorConversion other)
        {
            _convertToHSI = other._convertToHSI;
        }

        /// <summary>
        /// Serialization Constructor - necessary for save/restore.
        /// </summary>
        private CogColorConversion(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion

        #region StateFlags
        /// <summary>
        /// Note that one always uses the next StateFlag from the base class
        /// as the first StateFlag for the new class.  One should always assign SfNextSf as the
        /// next unassigned StateFlag
        /// so that derived classes can determine where they should begin assigning 
        /// their StateFlags.
        /// </summary>

        private const long Sf0 = CogSerializableChangedEventBase.SfNextSf;
        public const long SfConvertToHSI = Sf0 << 0;
        protected new const long SfNextSf = Sf0 << 1;
        #endregion

        #region Private Fields
        bool _convertToHSI;
        #endregion

        #region Public Properties
        /// <summary>
        /// These properties get/set the _convertToHSI value. When _convertToHSI value is 
        /// changed, a changed event is fired (OnChanged) with the appropriate StateFlag
        /// (SfConvertToHSI).
        /// </summary>

        public bool ConvertToHSI
        {
            get { return _convertToHSI; }
            set
            {
                if (value != _convertToHSI)
                {
                    _convertToHSI = value;
                    OnChanged(SfConvertToHSI);
                }
            }
        }
        #endregion

        #region ICloneable Implementation
        /// <summary>
        /// Clones the CogColorConversion object
        /// </summary>
        /// <returns>A new copy of the CogColorConversion object</returns>
        public object Clone()
        {
            return new CogColorConversion(this);
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Execute
        /// The execute method converts the 3-plane RGB input image to the planar
        /// output image in the appropriate color format.
        /// Note: The Execute method fails to handle conversion of the region to 
        /// pixel space.  It assumes that the Region is already in Pixel space.
        /// </summary>
        /// <param name="InputImage"></param>
        /// <param name="Region"></param>
        /// <returns></returns>

        public CogColorConversionResults Execute(CogImage24PlanarColor InputImage,
                                        ICogRegion Region)
        {
            CogImage24PlanarColor runImage;
            CogCopyRegion myCopyRegion = new CogCopyRegion();
            CogImage8Grey plane0, plane1, plane2;
            ICogShape regionAsShape = Region;
            CogRectangle AOI;
            double X = 0, Y = 0, Width = 0, Height = 0;
            bool sourceClipped, destClipped;
            ICogRegion outRegion;


            if (Region != null)
            {
                AOI = regionAsShape.EnclosingRectangle(CogCopyShapeConstants.GeometryOnly);
                AOI.GetXYWidthHeight(out X, out Y, out Width, out Height);
            }
            else
                AOI = null;
            try
            {
                if (_convertToHSI)
                {
                    if (AOI != null)
                        runImage = (CogImage24PlanarColor)CogImageConvert.GetHSIImage(
                          InputImage,
                          System.Convert.ToInt32(X),
                          System.Convert.ToInt32(Y),
                          System.Convert.ToInt32(Width),
                          System.Convert.ToInt32(Height));
                    else
                        runImage = (CogImage24PlanarColor)CogImageConvert.GetHSIImage(
                          InputImage, 0, 0,
                          System.Convert.ToInt32(InputImage.Width),
                          System.Convert.ToInt32(InputImage.Height));

                    return new CogColorConversionResults(runImage.GetPlane(CogImagePlaneConstants.Red),
                      runImage.GetPlane(CogImagePlaneConstants.Green),
                      runImage.GetPlane(CogImagePlaneConstants.Blue));
                }
                else
                {
                    if (AOI != null)
                    {
                        myCopyRegion.RegionMode = CogRegionModeConstants.PixelAlignedBoundingBox;
                        myCopyRegion.FillBoundingBox = false;
                        plane0 = (CogImage8Grey)myCopyRegion.Execute(InputImage.GetPlane(CogImagePlaneConstants.Red), AOI, null, out sourceClipped,
                          out destClipped, out outRegion);
                        plane1 = (CogImage8Grey)myCopyRegion.Execute(InputImage.GetPlane(CogImagePlaneConstants.Green), AOI, null, out sourceClipped,
                          out destClipped, out outRegion);
                        plane2 = (CogImage8Grey)myCopyRegion.Execute(InputImage.GetPlane(CogImagePlaneConstants.Blue), AOI, null, out sourceClipped,
                          out destClipped, out outRegion);
                    }
                    else
                    {
                        plane0 = InputImage.GetPlane(CogImagePlaneConstants.Red).Copy(CogImageCopyModeConstants.CopyPixels);
                        plane1 = InputImage.GetPlane(CogImagePlaneConstants.Green).Copy(CogImageCopyModeConstants.CopyPixels); ;
                        plane2 = InputImage.GetPlane(CogImagePlaneConstants.Blue).Copy(CogImageCopyModeConstants.CopyPixels); ;
                    }

                    return new CogColorConversionResults(plane0, plane1, plane2);
                }
            }
            catch (Cognex.VisionPro.Exceptions.CogException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new CogColorConversionToolException(ex.Message);
            }
        }
    }

        #endregion

    #region Enumerations
    /// <summary>
    /// These enumerations allow the Tool user to select what data they want to appear in 
    /// the CurrentRunRecord and the LastRunRecord.  The Flags attributes causes these
    /// enumerations to be treated as bitfields.
    /// </summary>

    [Flags]
    public enum CogColorConversionLastRunRecordConstants
    {
        None = 0x00,
        OutputImages = 0x01,
        All = OutputImages
    }

    [Flags]
    public enum CogColorConversionCurrentRunRecordConstants
    {
        None = 0x00,
        InputImage = 0x01,
        Region = 0x02,
        All = InputImage | Region
    }
    #endregion

    /// <summary>
    /// CogColorConversionTool
    /// This class represents the actual tool that converts images.  To associate this tool with
    /// a tool edit control, we use the Editor attribute as shown below.  We mark the class with the
    /// Serializable attribute so it can use persistence.  
    /// The Tool derives from CogToolBase, which provides a lot of implicit functionality.  It
    /// is the user's responsibilty to override the following methods from CogToolBase in order
    /// to supply user specific functionality:
    /// InternalRun(): This method is executed when "Run" is called.  Fill this in with whatever
    /// your tool should be doing when it is run.
    /// InternalCreateRunRecord(): This method creates the CurrentRunRecord for the Tool.
    /// The CurrentRunRecord contains a set of CogRecords that appear in the Tool's Edit Control
    /// when Current.InputImage is selected in the display.  In this case, the input image and
    /// the input region are placed in the CurrentRunRecord.
    /// InternalCreateLastRunRecord(): This method creates the LastRunRecord for the Tool.  The
    /// LastRunRecord() contains a set of CogRecords that are used to show results in the
    /// Tool Edit Control's display.  In this case, the three output planes are placed in the
    /// LastRunRecord.
    /// </summary>

    [Serializable]
    [Editor(typeof(CogColorConversionToolEditor), typeof(Control))]
    [CogDefaultToolInputTerminal(0, "InputImage", "InputImage")]
    [CogDefaultToolOutputTerminal(0, "OutputImage0", "OutputImage0")]
    [CogDefaultToolOutputTerminal(1, "OutputImage1", "OutputImage1")]
    [CogDefaultToolOutputTerminal(2, "OutputImage2", "OutputImage2")]
    public class CogColorConversionTool : CogToolBase
    {
        #region Constructors & Clone
        /// <summary>
        /// The CurrentRecordEnable_ and LastRunRecordEnable_ member variables are provided
        /// by the CogToolBase class. Set these members to indicate which records should appear
        /// in the CurrentRunRecord and the LastRunRecord.  In this case, we are defaulting
        /// to indicate that all produced data should appear.
        /// </summary>
        public CogColorConversionTool()
        {
            CurrentRecordEnable_ = (int)CogColorConversionCurrentRunRecordConstants.All;
            LastRunRecordEnable_ = (int)CogColorConversionLastRunRecordConstants.All;
            _HSI = false;
            _operator.ConvertToHSI = _HSI;
        }

        public CogColorConversionTool(CogColorConversionTool other)
        {
            if (other._OutputImage0 != null)
                _OutputImage0 = (CogImage8Grey)((ICloneable)other._OutputImage0).Clone();
            if (other._OutputImage1 != null)
                _OutputImage1 = (CogImage8Grey)((ICloneable)other._OutputImage1).Clone();
            if (other._OutputImage2 != null)
                _OutputImage2 = (CogImage8Grey)((ICloneable)other._OutputImage2).Clone();
            if (other._Region != null)
                _Region = (ICogRegion)((ICloneable)other._Region).Clone();
            _HSI = other._HSI;
            _operator.ConvertToHSI = _HSI;
        }

        /// <summary>
        /// Serialization Constructor - necessary for save/restore.
        /// </summary>
        private CogColorConversionTool(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        /// <summary>
        /// Clones the CogColorConversionTool object
        /// </summary>
        /// <returns>A new copy of the CogColorConversionTool object</returns>
        protected override object Clone() { return new CogColorConversionTool(this); }

        #endregion

        #region Private Fields
        private CogColorConversion _operator = new CogColorConversion();
        private CogImage8Grey _OutputImage0;
        private CogImage8Grey _OutputImage1;
        private CogImage8Grey _OutputImage2;
        private CogImage24PlanarColor _InputImage;
        private ICogRegion _Region;
        private bool _HSI;

        #endregion

        #region State flags
        /// <summary>
        /// Note that one always uses the next StateFlag from the base class
        /// as the first StateFlag for the new class.  One should always assign SfNextSf as the
        /// next unassigned StateFlag
        /// so that derived classes can determine where they should begin assigning 
        /// their StateFlags.  In this sample, we are assigning a unique StateFlag to each
        /// private field for this class.  We are also assigning StateFlags to the Current
        /// and LastRun record enable variables.
        /// </summary>

        private const long Sf0 = CogToolBase.SfNextSf;
        public const long SfInputImage = Sf0 << 0;
        public const long SfHSI = Sf0 << 1;
        public const long SfRegion = Sf0 << 2;
        public const long SfOutputImage0 = Sf0 << 3;
        public const long SfOutputImage1 = Sf0 << 4;
        public const long SfOutputImage2 = Sf0 << 5;
        public const long SfCurrentRunRecordEnable = Sf0 << 6;
        public const long SfLastRunRecordEnable = Sf0 << 7;
        protected new const long SfNextSf = Sf0 << 8;
        #endregion

        #region Public Properties

        // Note that we fire changed events (OnChanged) whenever a property changes.

        public CogColorConversionLastRunRecordConstants LastRunRecordEnable
        {
            get { return (CogColorConversionLastRunRecordConstants)LastRunRecordEnable_; }
            set
            {
                LastRunRecordEnable_ = (int)value;
                OnChanged(SfLastRunRecordEnable);
            }

        }

        public CogColorConversionCurrentRunRecordConstants CurrentRunRecordEnable
        {
            get { return (CogColorConversionCurrentRunRecordConstants)CurrentRecordEnable_; }
            set
            {
                CurrentRecordEnable_ = (int)value;
                OnChanged(SfCurrentRunRecordEnable);
            }

        }
        /// <summary>
        /// When the InputImage changes, we need to fire two changed events.  One changed
        /// event indicates that the InputImage has changed.  The other (SfCreateCurrentRecord)
        /// is used to update the CurrentRecord, of which the InputImage is part.
        /// </summary>	
        public CogImage24PlanarColor InputImage
        {
            get { return _InputImage; }
            set
            {
                if ((_InputImage == null) || (!_InputImage.Equals(value))) // simplest test - ref equivalence.
                {
                    _InputImage = value;
                    OnChanged(SfInputImage | SfCreateCurrentRecord);
                }
            }
        }
        public CogImage8Grey OutputImage0 { get { return _OutputImage0; } }
        public CogImage8Grey OutputImage1 { get { return _OutputImage1; } }
        public CogImage8Grey OutputImage2 { get { return _OutputImage2; } }
        /// <summary>
        /// When the Region changes, we need to fire two changed events.  One changed
        /// event indicates that the Region has changed.  The other (SfCreateCurrentRecord)
        /// is used to update the CurrentRecord, of which the Region is part.
        /// </summary>
        public ICogRegion Region
        {
            get { return _Region; }
            set
            {
                if ((_Region == null) || (!_Region.Equals(value)))  // simplest test - ref equivalence.
                {
                    _Region = value;
                    OnChanged(SfRegion | SfCreateCurrentRecord);
                }
            }
        }
        public bool HSI
        {
            get { return _HSI; }
            set
            {
                if (_HSI != value)
                {
                    _HSI = value;
                    OnChanged(SfHSI);
                }
            }
        }
        #endregion

        #region CogToolBase overrides
        /// <summary>
        /// InternalCreateRunRecord
        /// This method creates the CurrentRunRecord.  Run records consist of
        /// one or more CogRecords.  If a CogRecord contains an ICogImage, it will be shown
        /// on the display of the Tool's edit control.  It the ICogImage record contains a
        /// graphics subrecord, the graphics will appear on the ICogImage.  For this tool,
        /// the CurrentRunRecord can include the InputImage and the Region.  Note that 
        /// these records are conditionally added based on the values stored in 
        /// CurrentRunRecordEnable.
        /// </summary>
        /// <param name="newRecord"></param>
        /// <param name="mycurrentRecordEnable"></param>
        protected override void InternalCreateCurrentRecord(ICogRecord newRecord, int mycurrentRecordEnable)
        {
            CogRecord InputImageRecord;
            if ((CurrentRunRecordEnable & CogColorConversionCurrentRunRecordConstants.InputImage) > 0)
            {
                InputImageRecord = new CogRecord("InputImage", typeof(CogImage24PlanarColor),
                    CogRecordUsageConstants.Input, false, _InputImage, "InputImage");
                newRecord.SubRecords.Add(InputImageRecord);
                if ((CurrentRunRecordEnable & CogColorConversionCurrentRunRecordConstants.Region) > 0)
                    InputImageRecord.SubRecords.Add(new CogRecord("Region", typeof(ICogGraphicInteractive),
                        CogRecordUsageConstants.Configuration, false, _Region, "Region"));
            }
        }
        /// <summary>
        /// InternalCreateLastRunRecord
        /// This method creates the LastRunRecord.  Run records consist of
        /// one or more CogRecords.  If a CogRecord contains an ICogImage, it will be shown
        /// on the display of the Tool's edit control.  It the ICogImage record contains a
        /// graphics subrecord, the graphics will appear on the ICogImage. For this tool,
        /// the LastRunRecord can include the OutputImages (i.e. individual planes).  Note that 
        /// this record is conditionally added based on the values stored in 
        /// LastRunRecordEnable.
        /// </summary>
        /// <param name="newRecord"></param>
        /// <param name="lastRunRecordEnable"></param>
        /// <param name="lastRunRecordDiagEnable"></param>
        protected override void InternalCreateLastRunRecord(ICogRecord newRecord, int lastRunRecordEnable, int lastRunRecordDiagEnable)
        {

            if ((LastRunRecordEnable & CogColorConversionLastRunRecordConstants.OutputImages) > 0)
            {
                newRecord.SubRecords.Add(new CogRecord("OutputImage0", typeof(CogImage8Grey),
                    CogRecordUsageConstants.Result, false, _OutputImage0, "OutputImage0"));
                newRecord.SubRecords.Add(new CogRecord("OutputImage1", typeof(CogImage8Grey),
                    CogRecordUsageConstants.Result, false, _OutputImage1, "OutputImage1"));
                newRecord.SubRecords.Add(new CogRecord("OutputImage2", typeof(CogImage8Grey),
                    CogRecordUsageConstants.Result, false, _OutputImage2, "OutputImage2"));
            }

        }
        /// <summary>
        /// InternalRun
        /// This method is the "Run" method for the tool.  It calls the Execute() method
        /// on the CogColorConversion operator.  It generates changed events for OutputImages
        /// and CreateLastRunRecord.  
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected override CogToolResultConstants InternalRun(ref string message)
        {
            CogColorConversionResults opResults;
            _operator.ConvertToHSI = _HSI;
            if (_InputImage == null)
            {
                throw new Cognex.VisionPro.Exceptions.CogOperatorNoInputImageException();
            }
            opResults = _operator.Execute(_InputImage, _Region);
            _OutputImage0 = opResults.Plane0;
            _OutputImage1 = opResults.Plane1;
            _OutputImage2 = opResults.Plane2;
            OnChanged(SfOutputImage0 | SfOutputImage1 |
                      SfOutputImage2 | SfCreateLastRunRecord);
            return CogToolResultConstants.Accept;
        }
        #endregion

    }
    class CogColorConversionToolException : Cognex.VisionPro.Exceptions.CogException
    {
        public String msg;
        public CogColorConversionToolException()
        {
        }
        public CogColorConversionToolException(String srcMsg)
        {
            msg = srcMsg;
        }
        /// <summary>
        /// Serialization Constructor - necessary for save/restore.
        /// </summary>
        private CogColorConversionToolException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        public override String Message
        {
            get { return msg; }
        }
    }
}
