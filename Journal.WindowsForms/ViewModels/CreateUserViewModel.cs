using System;
using System.Collections.Generic;
using System.Text;

namespace Journal.WindowsForms.ViewModels
{
    public class CreateUserViewModel : ViewModel
    {
        public CreateUserViewModel()
        {
        }

        public string FirstName 
        {
            get => _firstName;
            set => ChangeProperty( ref _firstName, value );
        }

        public string Surname 
        {
            get => _surname;
            set => ChangeProperty( ref _surname, value );
        }

        public string Patronymic
        {
            get => _lastName;
            set => ChangeProperty( ref _lastName, value );
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set => ChangeProperty( ref _phoneNumber, value );
        }


        public void CreateUser(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private string _firstName
                        , _surname
                        , _lastName
                        , _phoneNumber;
    }
}
