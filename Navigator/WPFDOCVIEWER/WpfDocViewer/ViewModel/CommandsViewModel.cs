using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace WpfDocViewer.ViewModel
{
    internal class CommandsViewModel : ICommand
    {
        
        /// <summary>
        /// The Commands Update Class
        /// </summary>
        /// needs access to services viewmodel.
        /// <param name="viewModel"></param>
        public CommandsViewModel(SearchAccounts viewModel)
        {
            this.viewModel = viewModel;
        }

        private SearchAccounts viewModel;

        #region ICommand Members
        public bool CanExecute(object parameter)
        {
           return String.IsNullOrWhiteSpace("Model.IDataErrorInfo");
        }
        public event System.EventHandler CanExecuteChanged
        {
            //passes off our events to the commandmanager
            //wired back in WPF command system.
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public void Execute(object parameter)
        {
            viewModel.Search("");
        }
        #endregion
    }
}
