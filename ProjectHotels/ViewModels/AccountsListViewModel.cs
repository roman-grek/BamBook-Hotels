using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.ComponentModel;
using ProjectHotels.Views;
using System.Collections.Generic;
using System.Collections;
using System.Net.Mail;
using System.Net;
using System;
using System.IO;
using System.Globalization;
using Newtonsoft.Json;

namespace ProjectHotels.ViewModels
{
    public class AccountsListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected List<AccountViewModel> Accounts { get; set; }

        public ICommand CreateAccountCommand { protected set; get; }
        public ICommand EnterAccountCommand { protected set; get; }
        public ICommand DeleteAccountCommand { protected set; get; }
        public ICommand RegisterCommand { protected set; get; }
        public ICommand LoginCommand { protected set; get; }
        public ICommand BackCommand { protected set; get; }

        public INavigation Navigation { get; set; }

        public AccountsListViewModel()
        {
            Accounts = new List<AccountViewModel>();
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, "accounts.json");
            try
            {
                var accountItem = JsonConvert.DeserializeObject<Models.Account>(File.ReadAllText(path));
                var accountVM = new AccountViewModel(accountItem);
                Accounts.Add(accountVM);
                Current = accountVM;
            }
            catch { }
            EnterAccountCommand = new Command(EnterAccount);
            CreateAccountCommand = new Command(CreateAccount);
            DeleteAccountCommand = new Command(DeleteAccount);
            RegisterCommand = new Command(RegisterAccount);
            LoginCommand = new Command(Login);
            BackCommand = new Command(Back);
            Resources = new LocalizedResources(typeof(Resources.Resources), App.CurrentSettings.SelectedLanguage);
        }

        private AccountViewModel current;
        public AccountViewModel Current
        {
            get { return current; }
            private set
            {
                if (current != value)
                {
                    current = value;
                    OnPropertyChanged("Current");
                    OnPropertyChanged("AccountButtonImage");
                    OnPropertyChanged("AccountButtonText");
                }
            }
        }
        public ImageSource AccountButtonImage
        {
            get
            {
                if (current != null)
                    return Current.Image;
                else
                    return ImageSource.FromUri(new Uri("https://image.flaticon.com/icons/png/512/23/23373.png"));
            }
        }

        public string AccountButtonText
        {
            get
            {
                return current != null ? Resources["SearchAccountButtonChange"] :
                    Resources["LoginButton"];
            }
        }

        private LocalizedResources resources;
        public LocalizedResources Resources
        {
            get { return resources; }
            private set
            {
                if (resources != value)
                {
                    resources = value;
                    OnPropertyChanged("Resources");
                }
            }
        }

        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private void CreateAccount()
        {
            Navigation.PushAsync(new RegisterPage(new AccountViewModel() { ListViewModel = this }));
        }

        private void EnterAccount()
        {
            Navigation.PushAsync(new LoginPage(new AccountViewModel() { ListViewModel = this }));
        }

        private void Back()
        {
            Navigation.PopAsync();
        }

        private void Login(object accountObject)
        {
            AccountViewModel account = accountObject as AccountViewModel;
            if (account != null && Accounts.Contains(account))
            {
                Current = account;
                Back();
            }
            else
            {
                MessagingCenter.Send(this, "Account not found");
            }

        }

        private void DeleteAccount(object accountObject)
        {
            AccountViewModel account = accountObject as AccountViewModel;
            if (account != null)
            {
                Accounts.Remove(account);
            }
            Back();
        }

        public void LeaveAccount()
        {
            Current = null;
        }

        private void RegisterAccount(object accountObject)
        {
            AccountViewModel account = accountObject as AccountViewModel;
            if (account == null || !account.IsValid)
            {
                MessagingCenter.Send(this, "Invalid register");
                return;
            }           
            try
            {
                MailAddress from = new MailAddress("bambookhotels@gmail.com", "BamBook Hotels");
                MailAddress to = new MailAddress(account.Email, account.Name);
                MailMessage msg = new MailMessage(from, to);
                msg.Subject = "Регистрация в приложении BamBook Hotels";
                msg.Body = string.Format("<h2>Добро пожаловать в BamBook Hotels!</h2> <p>Здравствуйте {0}! На этот адрес Вам будут приходить сообщения о бронировании отелей.</p>", account.Name.Trim());
                msg.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential("bambookhotels@gmail.com", "isp.project2019");
                smtp.EnableSsl = true;
                smtp.SendMailAsync(msg);
            }
            catch
            {
                MessagingCenter.Send(this, "Email check failed");
                return;
            }
            Accounts.Add(account);
            Login(account);
            Back();
        }

    }
}
