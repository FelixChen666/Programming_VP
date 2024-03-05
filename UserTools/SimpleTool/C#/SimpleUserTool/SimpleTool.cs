/*******************************************************************************
Copyright (C) 2008 Cognex Corporation

Subject to Cognex Corporations terms and conditions and license agreement,
you are authorized to use and modify this source code in any way you find
useful, provided the Software and/or the modified Software is used solely in
conjunction with a Cognex Machine Vision System.  Furthermore you acknowledge
and agree that Cognex has no warranty, obligations or liability for your use
of the Software.
*******************************************************************************/
// This sample program is designed to illustrate certain VisionPro
// features or techniques in the simplest way possible. It is not
// intended as the framework for a complete application. In
// particular, the sample program may not provide proper error
// handling, event handling, cleanup, repeatability, and other
// mechanisms that a commercial quality application requires.


using System;
using System.Runtime.Serialization;
using System.ComponentModel; // needed for Editor attribute
using System.Windows.Forms;  // needed for Editor attribute
using Cognex.VisionPro;
using Cognex.VisionPro.Implementation;

// This file contains the source code for SimpleTool.
// The user must do the following things to create a Tool:
// 1. Create a new Class Library project.
// 2. Include project references to:
//    - Cognex.VisionPro
//    - Cognex.VisionPro.Core
// 3. Create your Tool class.  Derive it from CogToolBase.
// 4. Define Tool-Specific private fields. Examples:
//    - Input Image
//    - Output Image
//    - Region
// 5. Define Constructors and Clone method.
// 6. Make the tool serializable.
// 7. Define a 64-bit state flag constant for each property or method that
//    returns changeable data.
// 8. Create properties for accessing tool fields.
//    - Each "setter" must call OnChanged(appropriate_state_flags).
// 9. Create other methods, as needed.
// 10. Override the following CogToolBase methods:
//    - InternalRun() - Executed when tool is run programmatically, or from
//      the edit control.
//    - InternalCreateCurrentRecord() - Creates a "run record"
//      containing images and graphcs to be displayed in the edit 
//      control before the tool runs.  Usually includes:
//       * Input Image
//       * Region
//    - InternalCreateLastRunRecord() - Creates a "run record"
//      containing images and graphcs to be displayed in the edit 
//      control after the tool runs.  Usually includes:
//       * Output Image(s)


namespace SimpleTool
{
    /// <summary>
    /// SimpleTool is the most basic example of a user-built tool.  When run,
    /// it copies the input image to the output image.  If CopyTwice is true,
    /// it copies the image twice.
    /// </summary>
    [Serializable]
    [Editor(typeof(SimpleToolEditV2), typeof(Control))]
    [CogDefaultToolInputTerminal(0, "InputImage", "InputImage")]
    [CogDefaultToolOutputTerminal(0, "OutputImage", "OutputImage")]
    [System.ComponentModel.DesignerCategory("")]
    public class SimpleTool : CogToolBase
    {
        #region Private Fields
        // This attribute identifies the following field as an input image.
        // (Users can choose not to serialize VisionPro input images.)
        [CogSerializationOptionsAttribute(CogSerializationOptionsConstants.InputImages)]
        private CogImage8Grey _InputImage;

        // This attribute identifies the following field as an output image.
        // (Users can choose not to serialize VisionPro output images.)
        [CogSerializationOptionsAttribute(CogSerializationOptionsConstants.OutputImages)]
        private CogImage8Grey _OutputImage;

        // CopyTwice will be modifiable in the edit control.
        private bool _CopyTwice;   // defaults to false
        #endregion

        #region Constructors and Clone
        // Construct this tool in a default state.
        public SimpleTool()
        {
        }

        // Construct this tool as a deep copy of the given one.
        public SimpleTool(SimpleTool otherTool)
            : base(otherTool)
        {

            // Make a deep copy of the input image
            if (otherTool._InputImage != null)
                _InputImage = otherTool._InputImage.Copy();

            // Make a deep copy of the output image, but only if it's not
            // already the same object as the input image.
            if (otherTool._OutputImage != null)
            {
                if (Object.ReferenceEquals(otherTool._InputImage,
                                           otherTool._OutputImage))
                    _OutputImage = _InputImage;  // Same object
                else  // Deep copy
                    _OutputImage = otherTool._OutputImage.Copy();
            }

            _CopyTwice = otherTool._CopyTwice;
        }

        // Construct this tool from the given serialization info.
        // The call to the base class will initialize all tool fields.
        private SimpleTool(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        // Return a deep copy of this object via ICloneable.Clone()
        protected override object Clone() { return new SimpleTool(this); }
        #endregion

        #region State Flags
        // Define a 64-bit state flag constant for each property or method
        // that returns changeable data.  The name of the constant must be
        // "Sf" followed by the name of the property or method.  Make sure
        // that the bit constants defined here do not overlap with the ones
        // from our base class: The first state flag should be set to
        // "CogToolBase.SfNextSf".
        private const long Sf0 = CogToolBase.SfNextSf;
        public const long SfInputImage = Sf0 << 0;
        public const long SfOutputImage = Sf0 << 1;
        public const long SfCopyTwice = Sf0 << 2;
        protected new const long SfNextSf = Sf0 << 3;  // for derived classes
        #endregion

        #region Public Properties
        public CogImage8Grey InputImage
        {
            get { return _InputImage; }
            set
            {
                if (!Object.ReferenceEquals(_InputImage, value))
                {
                    _InputImage = value;
                    // The state flags in the Changed event below indicate that the
                    // current run record has changed (along with the input image).
                    // This is done because the input image is contained inside
                    // the current run record.
                    // Note that SfCreateCurrentRecord is provided by CogToolBase.
                    OnChanged(SfInputImage | SfCreateCurrentRecord);
                }
            }
        }

        public CogImage8Grey OutputImage
        {
            // This property has no setter since it is an output of the tool.
            get { return _OutputImage; }
        }

        public bool CopyTwice
        {
            get { return _CopyTwice; }
            set
            {
                if (_CopyTwice != value)
                {
                    _CopyTwice = value;
                    OnChanged(SfCopyTwice);  // Fire the CopyTwice Changed event
                }
            }
        }
        #endregion

        #region CogToolBase overrides
        /// <summary>
        /// This is the "Run" method for the SimpleTool.  It overwrites the 
        /// existing OutputImage with a deep copy of the InputImage.
        /// </summary>
        /// <param name="message">
        /// A string that will be copied into the message property of the 
        /// tool's RunStatus object (if no exceptions are thrown).
        /// </param>
        /// <returns>An enumeration value describing the tool result.</returns>
        protected override CogToolResultConstants InternalRun(ref string message)
        {
            // It's an error to run the tool without an input image...
            if (_InputImage == null)
                throw new Cognex.VisionPro.Exceptions.CogOperatorNoInputImageException();

            // Make a deep copy of the image.  Doing it twice isn't really 
            // useful except to illustrate how tool properties can be used
            // to alter tool execution (and waste CPU cycles).
            _OutputImage = _InputImage.Copy();
            if (_CopyTwice)
            {
                _OutputImage = _InputImage.Copy();
                message = "Copied twice!";
            }

            // Fire a Changed event indicating that the output image -- and
            // the LastRunRecord containing that image -- have changed.
            OnChanged(SfOutputImage | SfCreateLastRunRecord);

            // If the code reaches here, the tool has run successfully.
            return CogToolResultConstants.Accept;
        }

        // The two methods below create "run records" for the SimpleTool.
        // The "Current" run record contains images and graphics that represent
        // inputs to the tool.  The "Last" run record holds images and graphics
        // that were computed during the last run of the tool.
        //
        // A run record is a hierarchical structure whose top-level node 
        // contains a collection of image records.  Each image record can
        // be selected in the dropdown list of the tool's edit control.
        // The control will display the selected image record plus all 
        // graphics contained in sub-records of that image record.
        //
        // Each method below adds an appropriate image record to the (newly 
        // created) top-level record called "newRecord".  Usually each routine
        // would use the provided bit flags to decide what images and graphics
        // to add to the top-level record, but for the sake of simplicity we
        // ignore the bit flags (and add no graphics).


        /// <summary>
        /// Creates the CurrentRecord.  For this tool,
        /// the CurrentRecord includes the InputImage.
        /// </summary>
        /// <param name="newRecord">
        /// A newly-created record to which image sub-records may be added.
        /// </param>
        /// <param name="currentRecordEnable">
        /// An integer bit pattern indicating what images and graphics to add.
        /// </param>
        protected override void InternalCreateCurrentRecord(ICogRecord newRecord,
                                                            int currentRecordEnable)
        {
            // Add a new image record to the top-level "newRecord".
            newRecord.SubRecords.Add(
              new CogRecord(
                "InputImage",                  // Subrecord key for this record,
                typeof(CogImage8Grey),         // Type of its content, 
                CogRecordUsageConstants.Input, // Record represents an input,
                false,                         // Record content is unchanging,
                _InputImage,                   // The record's content,
                "InputImage"));                // Annotation (for display)
        }

        /// <summary>
        /// Creates the LastRunRecord.  For this tool,
        /// the LastRunRecord includes the OutputImage.
        /// </summary>
        /// <param name="newRecord">
        /// A newly-created record to which image sub-records may be added.
        /// </param>
        /// <param name="lastRunRecordEnable">
        /// An integer bit pattern indicating what images and graphics to add.
        /// </param>
        /// <param name="lastRunRecordDiagEnable">
        /// An integer bit pattern indicating what 'Diag' images and graphics
        /// to add.  'Diag' images and graphics are specified using a separate
        /// set of bits because they make the tool run significantly slower
        /// (or use significantly more memory).  Users must re-run the tool
        /// to see these items.
        /// </param>
        protected override void InternalCreateLastRunRecord(ICogRecord newRecord,
                                                            int lastRunRecordEnable,
                                                            int lastRunRecordDiagEnable)
        {
            // Add a new image record to the top-level "newRecord".
            newRecord.SubRecords.Add(
              new CogRecord(
                "OutputImage",                  // Subrecord key for this record,
                typeof(CogImage8Grey),          // Type of its content, 
                CogRecordUsageConstants.Result, // Record represents a result,
                false,                          // Record content is unchanging,
                _OutputImage,                   // The record's content,
                "OutputImage"));                // Annotation (for display)
        }
        #endregion
    }
}
