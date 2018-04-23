using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.Utils;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.XtraRichEdit.Commands;

namespace RichEditNestedFields {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();

            string dvf = "DOCVARIABLE dvf";
            string mf = "MERGEFIELD mf";
            string fields = dvf + " " + mf;

            richEditControl1.Text = "This is a " + fields + " with a nested field. Its value is calculated dynamically in the CalculateDocumentVariable event handler.";

            DocumentRange[] dvFieldRanges = richEditControl1.Document.FindAll(fields, DevExpress.XtraRichEdit.API.Native.SearchOptions.None);

            foreach (DocumentRange range in dvFieldRanges) {
                richEditControl1.Document.Fields.Add(range);
            }

            dvFieldRanges = richEditControl1.Document.FindAll(mf, DevExpress.XtraRichEdit.API.Native.SearchOptions.None);

            foreach (DocumentRange range in dvFieldRanges) {
                richEditControl1.Document.Fields.Add(range);
            }

            List<SampleObject> objects = new List<SampleObject>();

            objects.Add(new SampleObject());
            objects[0].mf = "Test";

            richEditControl1.Options.MailMerge.DataSource = objects;

            richEditControl1.Options.Fields.HighlightMode = FieldsHighlightMode.Always;
            richEditControl1.Options.Fields.HighlightColor = System.Drawing.Color.Yellow;

            new ToggleViewMergedDataCommand(richEditControl1).Execute();
            new ShowAllFieldCodesCommand(richEditControl1).Execute();
        }

        private void richEditControl1_CalculateDocumentVariable(object sender, CalculateDocumentVariableEventArgs e) {
            e.Value = e.VariableName + "'s value";

            if (e.Arguments.Count > 0) { 
                e.Value += string.Format(" (first argument: {0})", e.Arguments[0].Value);
            }

            e.Handled = true;
        }
    }

    public class SampleObject {
        private string _mf;

        public string mf {
            get { return _mf; }
            set { _mf = value; }
        }
    }
}