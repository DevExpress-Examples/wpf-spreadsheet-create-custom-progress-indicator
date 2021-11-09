Imports System.Threading
Imports System.Windows.Input
Imports DevExpress.Xpf.Core

Namespace WpfSpreadsheetProgressSample

    Public Partial Class CustomSplashScreen
        Inherits SplashScreenWindow

        Private ReadOnly cancellationTokenSource As CancellationTokenSource

        Public Sub New(ByVal cancellationTokenSource As CancellationTokenSource)
            Me.cancellationTokenSource = cancellationTokenSource
            InitializeComponent()
        End Sub

        Private Sub Cancel_Click(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
            cancellationTokenSource?.Cancel()
        End Sub
    End Class
End Namespace
