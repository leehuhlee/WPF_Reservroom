using Reservroom.Stores;
using Reservroom.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservroom.Services
{
    public class NavigationService
    {
        private readonly NavigationStore _navigationStore;
        private readonly Func<ViewModelBase> _currentViewModel;

        public NavigationService(NavigationStore navigationStore, Func<ViewModelBase> currentViewModel)
        {
            _navigationStore = navigationStore;
            _currentViewModel = currentViewModel;
        }

        public void Navigate()
        {
            _navigationStore.CurrentViewModel = _currentViewModel();
        }
    }
}
