Imports System.Windows
Imports DevExpress.Xpf.Core

Namespace WpfSpreadsheetProgressSample

    ''' <summary>
    ''' Interaction logic for App.xaml
    ''' </summary>
    Public Partial Class App
        Inherits Application

        Protected Overrides Sub OnStartup(ByVal e As StartupEventArgs)
            ApplicationThemeHelper.ApplicationThemeName = Theme.Office2019ColorfulName
            MyBase.OnStartup(e)
        End Sub
    End Class
End Namespace
