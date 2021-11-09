Imports System
Imports System.Threading
Imports DevExpress.Mvvm
Imports DevExpress.Office.Services
Imports DevExpress.Office.Services.Implementation
Imports DevExpress.Services
Imports DevExpress.Xpf.Core

Namespace WpfSpreadsheetProgressSample

    Public Partial Class MainWindow
        Inherits ThemedWindow
        Implements IProgressIndicationService

        Private cancellationTokenSource As CancellationTokenSource

        Private savedCancellationTokenProvider As ICancellationTokenProvider

        Private splashScreenManager As SplashScreenManager

        Public Sub New()
            InitializeComponent()
            ' Replace the default progress indication service
            ' with a custom service.
            spreadsheetControl1.ReplaceService(Of IProgressIndicationService)(Me)
        End Sub

        Private Sub Begin(ByVal displayName As String, ByVal minProgress As Integer, ByVal maxProgress As Integer, ByVal currentProgress As Integer)
            cancellationTokenSource = New CancellationTokenSource()
            ' Register a new CancellationTokenProvider instance
            ' to process cancellation requests. Save the reference
            ' to the previously registered service.
            savedCancellationTokenProvider = spreadsheetControl1.ReplaceService(Of ICancellationTokenProvider)(New CancellationTokenProvider(cancellationTokenSource.Token))
            ' Create a CustomSplashScreen instance.
            ' Display the name and progress of the running operation 
            ' in the splash screen.
            splashScreenManager = SplashScreenManager.Create(Function() New CustomSplashScreen(cancellationTokenSource), New DXSplashScreenViewModel With {.Title = displayName, .Progress = currentProgress})
            ' Display the splash screen.
            splashScreenManager.Show()
        End Sub

        Private Sub [End]()
            ' Close the splash screen.
            splashScreenManager?.Close()
            splashScreenManager = Nothing
            ' Restore previous CancellationTokenProvider.
            spreadsheetControl1.ReplaceService(savedCancellationTokenProvider)
            spreadsheetControl1.UpdateCommandUI()
            ' Dispose the CancellationTokenSource object.
            cancellationTokenSource?.Dispose()
            cancellationTokenSource = Nothing
        End Sub

        Private Sub SetProgress(ByVal currentProgress As Integer)
            ' Display the progress of the running operation in the splash screen.
            splashScreenManager.ViewModel.Progress = currentProgress
        End Sub

        Private Sub spreadsheetControl1_UnhandledException(ByVal sender As Object, ByVal e As DevExpress.XtraSpreadsheet.SpreadsheetUnhandledExceptionEventArgs)
            ' Handle OperationCanceledException
            ' that is thrown when a user cancels the operation.
            If TypeOf e.Exception Is OperationCanceledException Then e.Handled = True
        End Sub
    End Class
End Namespace
