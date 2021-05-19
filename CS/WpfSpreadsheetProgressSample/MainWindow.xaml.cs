using System;
using System.Threading;
using DevExpress.Mvvm;
using DevExpress.Office.Services;
using DevExpress.Office.Services.Implementation;
using DevExpress.Services;
using DevExpress.Xpf.Core;

namespace WpfSpreadsheetProgressSample {
    public partial class MainWindow : ThemedWindow, IProgressIndicationService {
        CancellationTokenSource cancellationTokenSource;
        ICancellationTokenProvider savedCancellationTokenProvider;
        SplashScreenManager splashScreenManager;

        public MainWindow() {
            InitializeComponent();
            // Replace the default progress indication service
            // with a custom service.
            spreadsheetControl1.ReplaceService<IProgressIndicationService>(this);
        }

        void IProgressIndicationService.Begin(string displayName, int minProgress, int maxProgress, int currentProgress) {
            cancellationTokenSource = new CancellationTokenSource();
            // Register a new CancellationTokenProvider instance
            // to process cancellation requests. Save the reference
            // to the previously registered service.
            savedCancellationTokenProvider = spreadsheetControl1.ReplaceService<ICancellationTokenProvider>(new CancellationTokenProvider(cancellationTokenSource.Token));
            // Create a CustomSplashScreen instance.
            // Display the name and progress of the running operation 
            // in the splash screen.
            splashScreenManager = SplashScreenManager.Create(() => new CustomSplashScreen(cancellationTokenSource), new DXSplashScreenViewModel {
                Title = displayName,
                Progress = currentProgress
            });
            // Display the splash screen.
            splashScreenManager.Show();
        }

        void IProgressIndicationService.End() {
            // Close the splash screen.
            splashScreenManager?.Close();
            splashScreenManager = null;
            // Restore previous CancellationTokenProvider.
            spreadsheetControl1.ReplaceService(savedCancellationTokenProvider);
            spreadsheetControl1.UpdateCommandUI();
            // Dispose the CancellationTokenSource object.
            cancellationTokenSource?.Dispose();
            cancellationTokenSource = null;
        }

        void IProgressIndicationService.SetProgress(int currentProgress) {
            // Display the progress of the running operation in the splash screen.
            splashScreenManager.ViewModel.Progress = currentProgress;
        }

        void spreadsheetControl1_UnhandledException(object sender, DevExpress.XtraSpreadsheet.SpreadsheetUnhandledExceptionEventArgs e) {
            // Handle OperationCanceledException
            // that is thrown when a user cancels the operation.
            if (e.Exception is OperationCanceledException)
                e.Handled = true;
        }
    }
}
