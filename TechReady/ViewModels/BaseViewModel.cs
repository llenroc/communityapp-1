using System.ComponentModel;
using System.Runtime.InteropServices.WindowsRuntime;

namespace TechReady.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public void RefreshBindings()
        {
            this.OnPropertyChanged("");
        }


        private int _operationsInProgress;

        private bool _operationInProgress;

        public virtual bool OperationInProgress
        {
            get
            {
                if (_operationsInProgress > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            set
            {
                if (value)
                {
                    this._operationsInProgress++;
                }
                else
                {
                    this._operationsInProgress--;
                }
                OnPropertyChanged("OperationInProgress");
                OnPropertyChanged("ShowCommandBar");
            }
        }


        private bool _showError;

        public bool ShowError
        {
            get { return _showError; }
            set
            {
                if (_showError != value)
                {
                    _showError = value;
                    OnPropertyChanged("ShowError");
                    OnPropertyChanged("ShowCommandBar");
                }
            }
        }

        private bool _showCommandBar;

        public bool ShowCommandBar
        {
            get
            {
                if (this.OperationInProgress || this.ShowError)
                {
                    return false;
                }
                return true;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this,new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
