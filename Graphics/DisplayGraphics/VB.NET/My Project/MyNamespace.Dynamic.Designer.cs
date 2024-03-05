using System;
using System.ComponentModel;
using System.Diagnostics;

namespace DisplayGraphics.My
{
    internal static partial class MyProject
    {
        internal partial class MyForms
        {

            [EditorBrowsable(EditorBrowsableState.Never)]
            public DisplayGraphicsForm m_DisplayGraphicsForm;

            public DisplayGraphicsForm DisplayGraphicsForm
            {
                [DebuggerHidden]
                get
                {
                    m_DisplayGraphicsForm = Create__Instance__<DisplayGraphicsForm>(m_DisplayGraphicsForm);
                    return m_DisplayGraphicsForm;
                }
                [DebuggerHidden]
                set
                {
                    if (object.ReferenceEquals(value, m_DisplayGraphicsForm))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__<DisplayGraphicsForm>(ref m_DisplayGraphicsForm);
                }
            }

        }


    }
}