Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Windows.Forms
Imports DevExpress.XtraRichEdit
Imports DevExpress.XtraRichEdit.Utils
Imports DevExpress.XtraRichEdit.API.Native
Imports DevExpress.XtraRichEdit.Commands

Namespace RichEditNestedFields
	Partial Public Class Form1
		Inherits Form
		Public Sub New()
			InitializeComponent()

			Dim dvf As String = "DOCVARIABLE dvf"
			Dim mf As String = "MERGEFIELD mf"
			Dim fields As String = dvf & " " & mf

			richEditControl1.Text = "This is a " & fields & " with a nested field. Its value is calculated dynamically in the CalculateDocumentVariable event handler."

			Dim dvFieldRanges() As DocumentRange = richEditControl1.Document.FindAll(fields, DevExpress.XtraRichEdit.API.Native.SearchOptions.None)

			For Each range As DocumentRange In dvFieldRanges
				richEditControl1.Document.Fields.Add(range)
			Next range

			dvFieldRanges = richEditControl1.Document.FindAll(mf, DevExpress.XtraRichEdit.API.Native.SearchOptions.None)

			For Each range As DocumentRange In dvFieldRanges
				richEditControl1.Document.Fields.Add(range)
			Next range

			Dim objects As New List(Of SampleObject)()

			objects.Add(New SampleObject())
			objects(0).mf = "Test"

			richEditControl1.Options.MailMerge.DataSource = objects

			richEditControl1.Options.Fields.HighlightMode = FieldsHighlightMode.Always
			richEditControl1.Options.Fields.HighlightColor = System.Drawing.Color.Yellow

			CType(New ToggleViewMergedDataCommand(richEditControl1), ToggleViewMergedDataCommand).Execute()
			CType(New ShowAllFieldCodesCommand(richEditControl1), ShowAllFieldCodesCommand).Execute()
		End Sub

		Private Sub richEditControl1_CalculateDocumentVariable(ByVal sender As Object, ByVal e As CalculateDocumentVariableEventArgs) Handles richEditControl1.CalculateDocumentVariable
			e.Value = e.VariableName & "'s value"

			If e.Arguments.Count > 0 Then
				e.Value += String.Format(" (first argument: {0})", e.Arguments(0).Value)
			End If

			e.Handled = True
		End Sub
	End Class

	Public Class SampleObject
		Private _mf As String

		Public Property mf() As String
			Get
				Return _mf
			End Get
			Set(ByVal value As String)
				_mf = value
			End Set
		End Property
	End Class
End Namespace