using Android.App;
using Plugin.CurrentActivity;

namespace CognitiveApp.Droid {

#if DEBUG //Moving this into MainApplication.cs after updating Connectivity Plugin, since it now request the CurrentActivity Plugin
    [Application(Debuggable = true)]
#else
    [Application(Debuggable=false)]
#endif
    public class MainApplication : Application, Application.IActivityLifecycleCallbacks {

        public MainApplication(System.IntPtr handle, Android.Runtime.JniHandleOwnership transer) : base(handle, transer) { }

        public override void OnCreate() {
            base.OnCreate();
            RegisterActivityLifecycleCallbacks(this);
            //CrossFingerprint.SetCurrentActivityResolver(() => CrossCurrentActivity.Current.Activity);
        }

        public override void OnTerminate() {
            base.OnTerminate();
            UnregisterActivityLifecycleCallbacks(this);
        }

        public void OnActivityCreated(Activity activity, Android.OS.Bundle savedInstanceState) {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityDestroyed(Activity activity) { }

        public void OnActivityPaused(Activity activity) { }

        public void OnActivityResumed(Activity activity) {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivitySaveInstanceState(Activity activity, Android.OS.Bundle outState) { }

        public void OnActivityStarted(Activity activity) {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityStopped(Activity activity) { }
    }
}