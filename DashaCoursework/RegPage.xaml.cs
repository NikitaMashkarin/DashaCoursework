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
    /// Логика взаимодействия для RegPage.xaml
    /// </summary>
    public partial class RegPage : Page
    {
        public RegPage()
        {
            InitializeComponent();
        }

        private void BtnSingIn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errStr = new StringBuilder();

            if (string.IsNullOrWhiteSpace(FNameTxtBox.Text))
                errStr.AppendLine("Поле 'Фамилия' не должно быть пустым");

            if (string.IsNullOrWhiteSpace(LogintxtBox.Text))
                errStr.AppendLine("Поле 'Имя' не должно быть пустым");

            if (string.IsNullOrWhiteSpace(RoleComboBox.Text))
                errStr.AppendLine("Поле 'Должность' не должно быть пустым");

            int post = 0;
            switch (RoleComboBox.Text)
            {
                case "Администратор":
                    {
                        post = 1;
                        break;
                    }
                case "Менеджер":
                    {
                        post = 2;
                        break;
                    }
                case "Продавец":
                    {
                        post = 3;
                        break;
                    }
                default:
                    errStr.AppendLine("Такой должность не существует");
                    break;
            }

            if (string.IsNullOrWhiteSpace(PwdtxtBox.Text))
            {
                errStr.AppendLine("Поле 'Логин' не должно быть пустым");
            }

            if (string.IsNullOrWhiteSpace(Extra2txtBox.Text))
                errStr.AppendLine("Поле 'Пароль' не должно быть пустым");

            if (string.IsNullOrWhiteSpace(Extra3txtBox.Text))
            {
                errStr.AppendLine("Поле 'Повторите пароль' не должно быть пустым");
            }
            else if (Extra2txtBox.Text != Extra3txtBox.Text)
            {
                errStr.AppendLine("Пароли не совпадают");
            }

            if (errStr.Length > 0)
            {
                MessageBox.Show(errStr.ToString(), "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var context = new courseworkEntities())
            {
                context.User.Add(new User
                {
                    Login = LogintxtBox.Text,
                    Password = PwdtxtBox.Text,
                    Post = post,
                    Name = FNameTxtBox.Text + " " + LogintxtBox.Text
                });

                context.SaveChanges();
            }
            using (var context = new courseworkEntities())
            {
                var user = context.User
                          .FirstOrDefault(u => u.Login.Trim() == LogintxtBox.Text);

                switch (post)
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
                }
            }
        }


        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AuthPage());
        }

    }
}
