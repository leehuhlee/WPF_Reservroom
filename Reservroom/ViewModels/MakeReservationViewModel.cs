﻿using Reservroom.Commands;
using Reservroom.Models;
using Reservroom.Services;
using Reservroom.Stores;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Reservroom.ViewModels
{
    public class MakeReservationViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private string _username;
        public string Username
        {
            get 
            { 
                return _username; 
            }
            set 
            { 
                _username = value; 
                OnPropertyChanged(nameof(Username));

                ClearErrors(nameof(Username));

                if (!HasUsername)
                {
                    AddError("Username cannot be empty", nameof(Username));
                }

                OnPropertyChanged(nameof(CanCreateReservation));
            }
        }

        private int _floorNumber = 1; 
        public int FloorNumber
        {
            get 
            {
                return _floorNumber;
            }
            set 
            {
                _floorNumber = value;
                OnPropertyChanged(nameof(FloorNumber));

                ClearErrors(nameof(FloorNumber));

                if (!HasFloorNumberGeneraterThanZero)
                {
                    AddError("Floor number must be greater than zero.", nameof(FloorNumber));
                }

                OnPropertyChanged(nameof(CanCreateReservation));
            }
        }

        private int _roomNumber;
        public int RoomNumber
        {
            get 
            { 
                return _roomNumber; 
            }
            set 
            { 
                _roomNumber = value; 
                OnPropertyChanged(nameof(RoomNumber));
            }
        }

        private DateTime _startDate = DateTime.Now;
        public DateTime StartDate
        {
            get
            {
                return _startDate;
            }
            set 
            { 
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));

                ClearErrors(nameof(StartDate));
                ClearErrors(nameof(EndDate));

                if(!HasStartDateBeforeEndDate)
                {
                    AddError("The start date cannot be after the end date.", nameof(StartDate));
                }

                OnPropertyChanged(nameof(CanCreateReservation));
            }
        }

        private DateTime _endDate = DateTime.Now.AddDays(1);
        public DateTime EndDate
        {
            get 
            { 
                return _endDate; 
            }
            set
            {
                _endDate = value;
                OnPropertyChanged(nameof(EndDate));
                ClearErrors(nameof(EndDate));

                if (!HasStartDateBeforeEndDate)
                {
                    AddError("The end date cannot be before the start date.", nameof(EndDate));
                }

                OnPropertyChanged(nameof(CanCreateReservation));
            }
        }

        private void AddError(string errorMessage, string propertyName)
        {
            if (!_propertyNameToErrorsDictionary.ContainsKey(propertyName))
            {
                _propertyNameToErrorsDictionary.Add(propertyName, new List<string>());
            }

            _propertyNameToErrorsDictionary[propertyName].Add(errorMessage);
            OnErrorsChanged(propertyName);
        }

        private void OnErrorsChanged(string parameterName)
        {
            ErrorsChanged.Invoke(this, new DataErrorsChangedEventArgs(parameterName));
        }

        public ICommand SubmitCommand { get; }
        public ICommand CancelCommand { get; }

        private readonly Dictionary<string, List<string>> _propertyNameToErrorsDictionary;

        public bool HasErrors => _propertyNameToErrorsDictionary.Any();

        public bool CanCreateReservation =>
            HasUsername &&
            HasFloorNumberGeneraterThanZero &&
            HasStartDateBeforeEndDate &&
            !HasErrors;

        private bool HasUsername => !string.IsNullOrEmpty(Username);
        private bool HasFloorNumberGeneraterThanZero => FloorNumber > 0;
        private bool HasStartDateBeforeEndDate => StartDate < EndDate;

        private string _submitErrorMessage;
        public string SubmitErrorMessage
        {
            get
            {
                return _submitErrorMessage;
            }
            set
            {
                _submitErrorMessage = value;
                OnPropertyChanged(nameof(SubmitErrorMessage));

                OnPropertyChanged(nameof(HasSubmitErrorMessage));
            }
        }
        public bool HasSubmitErrorMessage => !string.IsNullOrEmpty(SubmitErrorMessage);

        private bool _isSubmitting;
        public bool IsSubmitting 
        {
            get 
            {
                return _isSubmitting;
            }
            set
            {
                _isSubmitting = value;
                OnPropertyChanged(nameof(IsSubmitting));
            } 
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public MakeReservationViewModel(HotelStore hotelStore, NavigationService<ReservationListingViewModel> reservationViewNavigationService)
        {
            SubmitCommand = new MakeReservationCommand(this, hotelStore, reservationViewNavigationService);
            CancelCommand = new NavigateCommand<ReservationListingViewModel>(reservationViewNavigationService);

            _propertyNameToErrorsDictionary = new Dictionary<string, List<string>>();
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return _propertyNameToErrorsDictionary.GetValueOrDefault(propertyName, new List<string>());
        }
        private void ClearErrors(string propertyName)
        {
            _propertyNameToErrorsDictionary.Remove(propertyName);
            OnErrorsChanged(propertyName);
        }
    }
}
