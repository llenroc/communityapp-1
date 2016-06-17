using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Microsoft.ApplicationInsights;
using Microsoft.WindowsAzure.MobileServices;
using TechReady.Helpers.Storage;
using TechReady.ViewModels;
using TechReady.Views;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace TechReady
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
        // http://go.microsoft.com/fwlink/?LinkId=290986&clcid=0x409
        public static Microsoft.WindowsAzure.MobileServices.MobileServiceClient TechReadyServicesClient = new Microsoft.WindowsAzure.MobileServices.MobileServiceClient(
        "https://indiatechcommunityms.azure-mobile.net/",
        "NtCVhrhoQLLRblkwOMyQQGHOthBBjA60");

        private TransitionCollection transitions;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            WindowsAppInitializer.InitializeAsync();


            this.InitializeComponent();
            this.Suspending += this.OnSuspending;

            HardwareButtons.BackPressed += (sender, args) =>
            {
                if (this.RootFrame != null)
                {
                    if (RootFrame.CanGoBack)
                    {
                        this.RootFrame.GoBack();
                        args.Handled = true;
                    }
                    
                }
            };
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            // Windows Phone 8.1 requires you to handle the respose from the WebAuthenticationBroker.
            if (args.Kind == ActivationKind.WebAuthenticationBrokerContinuation)
            {
                // Completes the sign-in process started by LoginAsync.
                // Change 'MobileService' to the name of your MobileServiceClient instance. 
                App.TechReadyServicesClient.LoginComplete(args as WebAuthenticationBrokerContinuationEventArgs);
            }

            base.OnActivated(args);
        }

        Frame RootFrame { get; set; }
        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

           this.RootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (this.RootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                this.RootFrame = new Frame();

                // TODO: change this value to a cache size that is appropriate for your application
                this.RootFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = this.RootFrame;
            }

            if (RootFrame.Content == null)
            {
                // Removes the turnstile navigation for startup.
                if (RootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in RootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                RootFrame.ContentTransitions = null;
                RootFrame.Navigated += this.RootFrame_FirstNavigated;

                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter

                var model = await LocalStorage.ReadJsonFromFile<UserRegistrationPageViewModel>("registration");

                if (model == null)
                {

                    if (!RootFrame.Navigate(typeof (TechReady.Views.LoginPage), e.Arguments))
                    {
                        throw new Exception("Failed to create initial page");
                    }
                }
                else
                {
                    model.SaveUserRegitration();
                    RootFrame.Navigate(typeof (HubPage));
                }
            }

            // Ensure the current window is active
            Window.Current.Activate();

            // http://go.microsoft.com/fwlink/?LinkId=290986&clcid=0x409
          
        }

        /// <summary>
        /// Restores the content transitions after the app has launched.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the navigation event.</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}