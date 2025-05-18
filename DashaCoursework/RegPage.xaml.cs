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

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
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
                    Login = PwdtxtBox.Text,
                    Password = Extra3txtBox.Text,
                    Post = post,
                    Name = FNameTxtBox.Text + " " + LogintxtBox.Text
                });

                context.SaveChanges();
            }
            Manager.MainFrame.GoBack();
        }

    }
}
