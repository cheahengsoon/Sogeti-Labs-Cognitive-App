using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CognitiveApp.ViewModels {

    public class BaseViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool OnPropertyChanged<T>(ref T currentValue, T newValue, [CallerMemberName] string propertyName = "") {
            // ReSharper disable once ExplicitCallerInfoArgument
            return PropertyChanged.SetProperty(this, ref currentValue, newValue, propertyName);
        }
    }
}

namespace System.ComponentModel {

    public static class BaseViewModel {

        public static bool SetProperty<T>(this PropertyChangedEventHandler handler, object sender, ref T currentValue, T newValue, [CallerMemberName] string propertyName = "") {
            if(EqualityComparer<T>.Default.Equals(currentValue, newValue)) {
                return false;
            }

            currentValue = newValue;

            handler?.Invoke(sender, new PropertyChangedEventArgs(propertyName));

            return true;
        }
    }
}
