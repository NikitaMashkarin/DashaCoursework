using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DashaCoursework
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            var errStr = new StringBuilder();

            if (string.IsNullOrWhiteSpace(LoginTxtBox.Text))
                errStr.AppendLine("Поле Login не должно быть пустым");

            if (string.IsNullOrWhiteSpace(PswdTxtBox.Text))
                errStr.AppendLine("Поле Password не должно быть пустым");

            using (var context = new courseworkEntities())
            {
                var user = context.User
                           .FirstOrDefault(u => u.Login.Trim() == LoginTxtBox.Text);


                if (user == null || user.Password.Trim() != PswdTxtBox.Text)
                    errStr.AppendLine("Логин или пароль не верен");

                if (errStr.Length > 0)
                {
                    MessageBox.Show(errStr.ToString(), "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                MessageBox.Show($"{user.Name}", "Добро пожаловать!", MessageBoxButton.OK, MessageBoxImage.Information);

                switch (user.Post)
                {
                    case 1:
                        {
                            Manager.MainFrame.Navigate(new AdministratorPage(user.Id));
                            break;
                        }
                    case 2:
                        {
                            Manager.MainFrame.Navigate(new ManagerPage(user.Id));
                            break;
                        }
                    case 3:
                        {
                            Manager.MainFrame.Navigate(new SellerPage(user.Id));
                            break;
                        }
                    default:
                        return;
                }
            }
        }

        private void BtnSingIn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new RegPage());
        }
    }
}
